using BooksOnDemand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BooksOnDemand.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var context = new DAL.AppContext();

            // TODO : Use SessionSingIn 
            var list = context.GetUsers();

            var obj = context.GetUsers()
                .Where(x => x.Username.Equals(model.UserName) && x.Password.Equals(model.Password))
                .FirstOrDefault();

            if (obj != null)
            {
                
                Session["UserID"] = obj.Id.ToString();
                Session["Username"] = obj.Username.ToString();
                Session.Timeout = 2;

                return RedirectToAction("Index","Home",null);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}