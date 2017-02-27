using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}