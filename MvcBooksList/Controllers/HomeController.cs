using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcBooksList.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MvcBooksList.Controllers
{
    public class HomeController : Controller
    {

        //readonly Uri baseAddressOfBookApi;
        //public HomeController(IConfiguration configuration)
        //{
        //    baseAddressOfBookApi = new Uri(configuration.GetSection("ApiAddress:BookAPi").Value);
        //}


        private List<Book> bookview;
        public HomeController()
        {
            bookview = new List<Book>()
        {
            new Book()
            { BookName="HarryPotter", Author="xyz", Category="a", Subcategory="b", Publisher="au", Price=100 },
            new Book()
            { BookName="Invisible Man", Author="xyz", Category="a", Subcategory="b", Publisher="au", Price=50 },
            new Book()
            {BookName="Beloved", Author="xyz", Category="a", Subcategory="b", Publisher="au", Price=120 },
            new Book()
            { BookName="Anna Karenina", Author="xyz", Category="a", Subcategory="b", Publisher="au", Price=80},
            new Book()
            { BookName="Hamlet", Author="xyz", Category="a", Subcategory="b", Publisher="au", Price=200 },
            new Book()
            {BookName="Pride and Prejudice", Author="xyz", Category="a", Subcategory="b", Publisher="au", Price=90 },

        };
        }
        public ActionResult Index()
        {

            return View(bookview);
        }

        [HttpGet]
        public async Task<ActionResult> EditBookDetails(string id)
        {
            Book b = new Book();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync("https://localhost:44305/api/EditBookDetails/ViewBookByName?name=" + id))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    b = JsonConvert.DeserializeObject<Book>(apiRes);
                }
            }
            return View(b);
        }

        [HttpPost]
        public async Task<ActionResult> EditBookDetails(string id, Book b)
        {
            Book bo = new Book();

            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "aplication/json");
                using (var response = await client.PutAsync("https://localhost:44305/api/EditBookDetails/" + id, content))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    bo = JsonConvert.DeserializeObject<Book>(apiRes);
                }
            }
            return RedirectToAction("Index");
        }
        
        public ActionResult Edit()
        {
            return View("EditBookDetails");
        }
    }
   }
