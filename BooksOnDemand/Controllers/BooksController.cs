using BooksOnDemand.Models;
using MongoDB.Bson;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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

            var ctx = new DAL.AppContext();
            var books = ctx.GetBooks();

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                books = books.Where(s => s.Title.ToLower().Contains(searchString)
                                       || s.Authors.Where( x => x.ToLower().Contains(searchString)).Count() >= 1  
                                       || s.Publisher.ToLower().Contains(searchString));
            }


            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(books.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Demand(string id, bool? demand)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login","Authentication");
            }

            var ctx = new DAL.AppContext();
            Book bookObj = ctx.GetBook(id);
            // TODO URGENT: QUERY BOOK by ID
            if (bookObj == null)
            {
                return HttpNotFound();
            }

            ViewBag.Id = id;
            //  CHECK IF Book IS DEMANDED OR NOT YET.
            ViewBag.IsDemanded = bookObj.UserDemands.Contains(ObjectId.Parse(Session["UserID"].ToString()));

            demand = demand == null ? false: true;

            if ((bool)demand && !ViewBag.IsDemanded)
            {
                ctx.DemandBook(id , Session["UserID"].ToString());
                ViewBag.IsDemanded = true;
            }

            return View(bookObj);
        }
    }
}