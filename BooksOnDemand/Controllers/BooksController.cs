using BooksOnDemand.Models;
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
        public ActionResult Index()
        {
            var ctx = new DAL.AppContext();
            return View(ctx.GetBooks());
        }

        public ActionResult Demand(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ctx = new DAL.AppContext();
            Book bookObj = ctx.GetBook(id);
            // TODO URGENT: QUERY BOOK by ID
            if (bookObj == null)
            {
                return HttpNotFound();
            }
            return View(bookObj);
        }

    }
}