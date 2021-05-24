using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcBooksList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MvcBooksList.Controllers
{
    public class HomeController : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44305/";
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
                client.BaseAddress = new Uri("https://localhost:44305/");

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
                client.BaseAddress = new Uri("https://localhost:44305/");

                //HTTP DELETE
                var delistTask = client.PostAsync("Book/api/DelistBookByName?bookName=" + bookName,null);
                delistTask.Wait();

                var result = delistTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}