using Microsoft.AspNetCore.Http;
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
    public class CategoryController : Controller
    {
        readonly Uri baseAddressOfCategoryApi;
        public CategoryController(IConfiguration configuration)
        {
            baseAddressOfCategoryApi = new Uri(configuration.GetSection("ApiAddress:CategoryAPI").Value);
        }
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            List<Category> categories;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddressOfCategoryApi;

                // TO add token to header in future.
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","Token");

                var resopnse = await client.GetAsync("api/AdminCategory");
                if (resopnse.IsSuccessStatusCode)
                {
                    var result = resopnse.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<Category>>(result.Result);
                    return View(categories);
                }
            }
            return View(null);
        }

        // GET: CategoryController/Details/5
        public async Task<ActionResult> Details(string categoryName)
        {
            Category category;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddressOfCategoryApi;

                // TO add token to header in future.
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","Token");
                var resopnse = await client.GetAsync("api/AdminCategory/" + categoryName);
                if (resopnse.IsSuccessStatusCode)
                {
                    string result = await resopnse.Content.ReadAsStringAsync();
                    category = JsonConvert.DeserializeObject<Category>(result);
                    return View(category);
                }
            }
            return View();
        }

        public async Task<ActionResult> UpdateCategoryName(string oldName,string categoryName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddressOfCategoryApi;

                // TO add token to header in future.
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","Token");
                HttpContent putContent = new StringContent(categoryName);
                
                var resopnse = await client.PutAsync($"api/AdminCategory/{oldName}/{categoryName}",null);
                if (resopnse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details",routeValues:new { categoryName });
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> UpdateSubCategoryName(string categoryName, string oldName,string subCategoryName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddressOfCategoryApi;

                // TO add token to header in future.
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","Token");
                HttpContent putContent = new StringContent(subCategoryName);
                
                var resopnse = await client.PutAsync($"api/AdminSubCategory/{categoryName}/{oldName}/{subCategoryName}",null);
                if (resopnse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", routeValues: new { categoryName });
                }
            }
            return RedirectToAction("Index");
        }


    private class PostDateForAddSubCAtegory
        {
            public string categoryName { get; set; }
            public string subCategoryName { get; set; }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubCategory(string categoryName, string subCategoryName)
        {

            PostDateForAddSubCAtegory data = new PostDateForAddSubCAtegory
            {
                categoryName = categoryName.Replace("&nbsp"," "),
                subCategoryName = subCategoryName
            };
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = baseAddressOfCategoryApi;
                HttpContent httpContent = new StringContent(content: JsonConvert.SerializeObject(data), encoding: Encoding.Default, mediaType: "application/json");
                var resopnse = await client.PostAsync("api/AdminSubCategory", httpContent);
                if (resopnse.IsSuccessStatusCode)
                {
                    string responseContenet = await resopnse.Content.ReadAsStringAsync();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.CategoryMessage = "SubCategory cannot be created now. Try after some time";
                }
            }
                return RedirectToAction("Index");
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string categoryName)
        {
            using (HttpClient client = new HttpClient())
            {
                // client.BaseAddress = baseAddressOfCategoryApi;

                // TO add token to header in future.
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","Token");

                HttpContent httpContent = new StringContent(content: JsonConvert.SerializeObject(categoryName), encoding: Encoding.Default, mediaType: "application/json");
                client.BaseAddress = baseAddressOfCategoryApi;
                var resopnse = await client.PostAsync("api/AdminCategory", httpContent);
                if (resopnse.IsSuccessStatusCode)
                {
                    string responseContenet = await resopnse.Content.ReadAsStringAsync();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.CategoryMessage = "Book Cannot be created noe. Try after some time";
                }
            }
            return RedirectToAction("Index");

        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
