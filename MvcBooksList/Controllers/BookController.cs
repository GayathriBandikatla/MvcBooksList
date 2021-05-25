using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcBooksList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MvcBooksList.Controllers
{
    public class BookController : Controller
    {
        readonly Uri baseAddressOfBookApi;
        public BookController(IConfiguration configuration)
        {
            baseAddressOfBookApi = new Uri(configuration.GetSection("ApiAddress:BookAPI").Value);
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

        /* 
         public ActionResult Edit(string name)
         {

             return View(name); //repalce with the name of view
         }

         public ActionResult Delete(string name)
         {
             return View(name);  //repalce with the name of view
         }

         public ActionResult Delist(string name)
         {
             return View(name);   //repalce with the name of view
         }
        */

        public async Task<ActionResult> ViewDelistedBooks()
        {
            List<Book> bookdetails = new List<Book>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddressOfBookApi;
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
                        client.BaseAddress = baseAddressOfBookApi;
                        StringContent content = new StringContent(JsonConvert.SerializeObject(null), Encoding.UTF8, "application/json");
                        var response = await client.PutAsync("Book/EnlistBook?id="+item.BookName.ToString(),content);
                       // if (!response.IsSuccessStatusCode)
                       // { 
                        //   return new HttpNotFoundResult("Couldn't enlist the selected books"); ;
                       // }
                    }
                }
            }
            return RedirectToAction("Index","Home");
        }

    }
}
