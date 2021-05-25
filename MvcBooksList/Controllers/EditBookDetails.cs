using Microsoft.AspNetCore.Http;
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
    public class EditBookDetails : Controller
    {
        //Hosted web API REST Service base url  
        string Baseurl = "https://localhost:44305/";
        public async Task<ActionResult> Index()
        {
            List<Book> editbooks = new List<Book>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();

                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetActiveBooks using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("Book/api/EditBookDetails");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var BooksResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the ActiveBooks list  
                    editbooks = JsonConvert.DeserializeObject<List<Book>>(BooksResponse);
                }
                //returning the activebooks list to view  
                return View(editbooks);
                //// GET: EditBookDetails/Details/5
                //public ActionResult Details(int id)
                //{
                //    return View();
                //}

                //// GET: EditBookDetails/Create
                //public ActionResult Create()
                //{
                //    return View();
                //}

                //// POST: EditBookDetails/Create
                //[HttpPost]
                //[ValidateAntiForgeryToken]
                //public ActionResult Create(IFormCollection collection)
                //{
                //    try
                //    {
                //        return RedirectToAction(nameof(Index));
                //    }
                //    catch
                //    {
                //        return View();
                //    }
                //}

                //// GET: EditBookDetails/Edit/5
                //public ActionResult Edit(int id)
                //{
                //    return View();
                //}

                //// POST: EditBookDetails/Edit/5
                //[HttpPost]
                //[ValidateAntiForgeryToken]
                //public ActionResult Edit(int id, IFormCollection collection)
                //{
                //    try
                //    {
                //        return RedirectToAction(nameof(Index));
                //    }
                //    catch
                //    {
                //        return View();
                //    }
                //}

                //// GET: EditBookDetails/Delete/5
                //public ActionResult Delete(int id)
                //{
                //    return View();
                //}

                //// POST: EditBookDetails/Delete/5
                //[HttpPost]
                //[ValidateAntiForgeryToken]
                //public ActionResult Delete(int id, IFormCollection collection)
                //{
                //    try
                //    {
                //        return RedirectToAction(nameof(Index));
                //    }
                //    catch
                //    {
                //        return View();
                //    }
                //}
            }
        }
    }
}
