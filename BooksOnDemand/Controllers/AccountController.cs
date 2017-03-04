using BooksOnDemand.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace BooksOnDemand.Controllers
{
    public class AccountController : Controller
    {
        // GET: Authentication
        public ActionResult Login()
        {
            return View();
        }
        
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var jsonRespond = CrossoverClient.PostRequestJSON<LoginViewModel>("api/login", model);
                string userId = JsonConvert.DeserializeObject<string>(jsonRespond);

                if (!string.IsNullOrEmpty(userId))
                {
                    Session["UserID"] = userId;
                    Session["Username"] = model.UserName;
                    Session.Timeout = 30;
                    return RedirectToAction("Index", "Home", null);
                }
            }
            catch (HttpResponseException ex)
            {
                return new HttpStatusCodeResult(ex.Response.StatusCode, ex.Message);
            }

            return View(model);
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var respond = CrossoverClient.PostRequest<RegisterViewModel>("api/register", model);

                if (respond.IsSuccessStatusCode)
                {
                    var jsonRespond = CrossoverClient.PostRequestJSON<LoginViewModel>("api/login", new LoginViewModel() { UserName = model.UserName, Password = model.Password });

                    string userId = JsonConvert.DeserializeObject<string>(jsonRespond);

                    if (!string.IsNullOrEmpty(userId))
                    {
                        Session["UserID"] = userId;
                        Session["Username"] = model.UserName;
                        Session.Timeout = 30;
                    }

                    return RedirectToAction("Index", "Home", null);
                }
                else
                {
                    return new HttpStatusCodeResult(respond.StatusCode,respond.ReasonPhrase);
                }
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}