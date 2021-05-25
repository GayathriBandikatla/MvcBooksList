using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcBooksList.jwt;
using MvcBooksList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

using System.Net.Http.Json;
using System.Text;

using System.Net.Http.Headers;

using System.Threading.Tasks;

namespace MvcBooksList.Controllers
{
    public class HomeController : Controller
    {


        readonly Uri baseaddressofbookapi;
        readonly Uri baseaddressofCategoryapi;
        public HomeController(IConfiguration configuration)
        {
            baseaddressofbookapi = new Uri(configuration.GetSection("ApiAddress:BookAPi").Value);
            baseaddressofCategoryapi = new Uri(configuration.GetSection("ApiAddress:CategoryAPI").Value);
            token = jwttokenProvider.generateJwtToken("user43");
        }


        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44305/";
        string token;
        JWTToken jwttokenProvider = new JWTToken();
        
        
        public async Task<ActionResult> Index()

        {
            List<Book> activebooks = new List<Book>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetActiveBooks using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("Book/api/GetActiveBooks");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var BooksResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the ActiveBooks list  
                    activebooks = JsonConvert.DeserializeObject<List<Book>>(BooksResponse);

                }
                //returning the activebooks list to view  
                return View(activebooks);
            }
        }


        public ActionResult DeleteBookName(string bookName)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseaddressofbookapi;

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Book/api/DeleteBookByName?bookName=" + bookName);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult DelistBookName(string bookName)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseaddressofbookapi;

                //HTTP DELETE
                var delistTask = client.PostAsync("Book/api/DelistBookByName?bookName=" + bookName, null);
                delistTask.Wait();

                var result = delistTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult AddBookDetails(Book value)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri("https://localhost:44305/");
                client.BaseAddress = baseaddressofbookapi;

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Book>("Book/api/AddBook", value);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            return View("AddBook");
        }

        [HttpGet]
        public async Task<ActionResult> EditBookDetails(string id)
        {
            Book b = new Book();
            using (var client = new HttpClient())
            {
                client.BaseAddress = baseaddressofbookapi;

                using (var response = await client.GetAsync("Book/GetBookByName?bookName=" + id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    b = JsonConvert.DeserializeObject<Book>(apiRes);
                }
            }
            return View("EditDetails", b);

        }

        [HttpPost]
        public async Task<ActionResult> EditBookDetails(string id, Book b)
        {
            Book bo = new Book();

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseaddressofbookapi;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                StringContent content = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
                var response = await client.PutAsync("Book/EditBookDetails?id=" + id, content);
                if (response.IsSuccessStatusCode)
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    // bo = JsonConvert.DeserializeObject<Book>(apiRes);
                }
            }
            return RedirectToAction("Index");
        }

    }
}