using CryptSharp;
using Interactive.HateBin.Data;
using Interactive.HateBin.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace Interactive.HateBin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserRepository repository => new UserRepository();
        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = repository.GetByEmail(model.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = Crypter.Blowfish.Crypt(model.Password),
                        Roles = new List<string>()
                    };

                    repository.Save(user);
                    SetAuthCookie(user);
                    return RedirectToAction("Index", "Home");
                }
                model.Errors = "Det finns redan en användare med den e-post adressen.";
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = repository.GetByEmail(model.Email);
                if (user != null)
                {
                    if (Crypter.CheckPassword(model.Password, user.Password))
                    {
                        SetAuthCookie(user);
                        return RedirectToAction("Index", "Home");
                    }
                    model.Errors = "Felaktigt lösenord.";
                }
                else
                {
                    model.Errors = "Det finns ingen användare med den e-post adressen.";
                }
            }
            return View(model);
        }

        public ActionResult Token()
        {
            var currentUser = repository.GetByEmail(HttpContext.User.Identity.Name);
            if (currentUser.Token == Guid.Empty)
            {
                currentUser.Token = Guid.NewGuid();
                repository.Save(currentUser);
            } 
            return Content(JsonConvert.SerializeObject(new UserData { Name = currentUser.Name, Token = currentUser.Token}), "application/json");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private void SetAuthCookie(User user)
        {
            var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddYears(1), true, string.Join(",", user.Roles));
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(cookie);
        }
    }
}