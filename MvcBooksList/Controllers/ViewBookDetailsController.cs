using Microsoft.AspNetCore.Mvc;
using MvcBooksList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MvcBooksList.Controllers
{
    public class ViewBookDetailsController : Controller
    {
        string Baseurl = "https://localhost:44305/";
        public async Task<ActionResult> ViewBookDetails(string BookName)
        {
            Book bookdetails = new Book();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource Bookdetails using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("Book/api/GetBookByName?bookName=" + BookName);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var BooksResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the bookdetails list  
                    bookdetails = JsonConvert.DeserializeObject<Book>(BooksResponse);

                }
                return View(bookdetails);

            }

        }



       
    }
   
}

