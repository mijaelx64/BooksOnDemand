using BooksOnDemand.Models;
using MongoDB.Bson;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BooksOnDemand.Controllers
{
    public class BooksController : Controller
    {

        // GET: Books
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            try
            {
                string jsonRespond = CrossoverClient.GetJSON("api/books");
                IEnumerable<Book> books = JsonConvert.DeserializeObject<IEnumerable<Book>>(jsonRespond);

                if (!String.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();
                    books = books.Where(s => s.Title.ToLower().Contains(searchString)
                                           || s.Authors.Where(x => x.ToLower().Contains(searchString)).Count() >= 1
                                           || s.Publisher.ToLower().Contains(searchString));
                }

                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(books.ToPagedList(pageNumber, pageSize));
            }
            catch (HttpResponseException ex)
            {
                return new HttpStatusCodeResult(ex.Response.StatusCode, ex.Message);
            }
        }

        public ActionResult Demand(string id, bool? demand)
        {
            if (string.IsNullOrEmpty(id))
            {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest, 
                     "An Id must be set in order to get a book information.");
            }

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login","Authentication");
            }

            ViewBag.IsDemanded = false;

            try
            {
                string jsonRespond = CrossoverClient.GetJSON("api/books/" + id);
                Book bookObj = JsonConvert.DeserializeObject<Book>(jsonRespond);

                ViewBag.Id = id;
                demand = demand == null ? false : true;

                if ((bool)demand && !ViewBag.IsDemanded)
                {
                    //ctx.DemandBook(id , Session["UserID"].ToString());
                    ViewBag.IsDemanded = true;
                }

                return View(bookObj);
            }
            catch (HttpResponseException ex)
            {
                return new HttpStatusCodeResult(ex.Response.StatusCode, ex.Message);
            }
        }
    }
}