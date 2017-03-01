using BooksOnDemand.Models;
using MongoDB.Bson;
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

        public ActionResult Demand(string id, bool? demand)
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

            ViewBag.Id = id;
            //  CHECK IF Book IS DEMANDED OR NOT YET.

            if (Session["UserID"]!=null)
            {
                ViewBag.IsDemanded = bookObj.UserDemands.Contains(ObjectId.Parse(Session["UserID"].ToString()));
            }

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