using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcBooksList.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBooksList.Controllers
{
    public class HomeController : Controller
    {
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
    }
   }
