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
using Microsoft.AspNetCore.Mvc.Rendering;

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
            string name = Environment.UserName;
            token = jwttokenProvider.generateJwtToken(name);
            
        }


        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44305/";
        string token;
        JWTToken jwttokenProvider = new JWTToken();
        
        
        public async Task<ActionResult> Index()

        {
            string username = User.Identity.Name;

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
        public async Task<ActionResult> Create()
        {

            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseaddressofCategoryapi;
                var response =await client.GetAsync("Category");
                if(response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<string> categories = JsonConvert.DeserializeObject<List<string>>(result);
                    List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach(string category in categories)
                    {
                        selectListItems.Add(new SelectListItem { Value = category, Text = category });

                    }
                    ViewBag.Categories = selectListItems;
                }
            }



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
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseaddressofCategoryapi;
                var response = await client.GetAsync("Category");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<string> categories = JsonConvert.DeserializeObject<List<string>>(result);
                    List<SelectListItem> selectListItems = new List<SelectListItem>();
                    foreach (string category in categories)
                    {
                        if(category==b.Category)
                            selectListItems.Add(new SelectListItem { Selected=true, Value = category, Text = category  });
                        else
                            selectListItems.Add(new SelectListItem {  Value = category, Text = category  });

                    }
                    ViewBag.Categories = selectListItems;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseaddressofCategoryapi;
                var response = await client.GetAsync("Category/"+b.Category);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<string> subcategories = JsonConvert.DeserializeObject<List<string>>(result);
                    List<SelectListItem> selectListItems = new List<SelectListItem>();
                    foreach (string subcategory in subcategories)
                    {
                        if(subcategory==b.Subcategory)
                            selectListItems.Add(new SelectListItem { Selected=true, Value = subcategory, Text = subcategory  });
                        else
                            selectListItems.Add(new SelectListItem {  Value = subcategory, Text = subcategory  });

                    }
                    ViewBag.Subcategories = selectListItems;
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


















        public async Task<ActionResult> ViewBookDetailsAsync(string BookName)
        {
            string Baseurl = "https://localhost:44305/";
            Book bookdetails = new Book();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Bookdetails using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("Book/GetBookByName?bookName=" + BookName);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var BooksResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the bookdetails list  
                    bookdetails = JsonConvert.DeserializeObject<Book>(BooksResponse);

                }
                return View("ViewBookDetails", bookdetails);

            }
        }

        public async Task<ActionResult> ViewDelistedBooks()
        {
            List<Book> bookdetails = new List<Book>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseaddressofbookapi;
                var response = await client.GetAsync("Book/ViewDelistedBooks");
                if (response.IsSuccessStatusCode)
                {
                    var BookResponse = response.Content.ReadAsStringAsync().Result;
                    bookdetails = JsonConvert.DeserializeObject<List<Book>>(BookResponse);
                }
            }
            return View(bookdetails);
        }

        public ActionResult Cancel()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> DelistedForm(IEnumerable<Book> fromDelist)
        {
            foreach (var item in fromDelist)
            {
                if (item.IsActive == true)
                //CALL
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = baseaddressofbookapi;
                        StringContent content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                        var response = await client.PutAsync("Book/EnlistBook?id=" + item.BookName.ToString(), content);

                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }





    }
}