﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
                if(resopnse.IsSuccessStatusCode)
                {
                   var result =  resopnse.Content.ReadAsStringAsync();
                   categories= JsonConvert.DeserializeObject<List<Category>>(result.Result);
                    return View(categories);
                }
            }
                return View(null);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        //GET: CategoryController/Edit/5
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
