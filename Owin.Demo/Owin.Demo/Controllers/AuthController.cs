﻿using Owin.Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Owin.Demo.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login()
        {
            var model = new LoginModel();
            var providers = HttpContext.GetOwinContext()
                .Authentication.GetAuthenticationTypes(x => !string.IsNullOrEmpty(x.Caption))
                .ToList();
            model.AuthProviders = providers;
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if(model.UserName.Equals("tuan", StringComparison.OrdinalIgnoreCase) &&
                model.Password == "password")
            {
                var identity = new ClaimsIdentity("ApplicationCookie");
                identity.AddClaims(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, model.UserName),
                    new Claim(ClaimTypes.Name, model.UserName)
                });
                HttpContext.GetOwinContext().Authentication.SignIn(identity);
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }

        public ActionResult SocialLogin(string id)
        {
            HttpContext.GetOwinContext().Authentication.Challenge(new Microsoft.Owin.Security.AuthenticationProperties
            {
                RedirectUri = "/secret"
            }, id);
            return new HttpUnauthorizedResult();
        }
    }
}