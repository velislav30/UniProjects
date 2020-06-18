using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VT.Business.DTOs;
using VT.Business.Services;
using VT.Website.Models;

namespace VT.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        UserService userService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            userService = new UserService();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                UserDto loggedUser = userService.GetUserByLoginCredentials(loginVM.Username,loginVM.Password);

                if (loggedUser == null)
                {
                    ModelState.AddModelError("AuthError", "Invalid Username or Password!");
                }
                else
                {
                    loggedUser.LastLoggedIn = DateTime.Now;
                    userService.Update(loggedUser);
                    HttpContext.Session.SetCurrentUser("loggedUser", loggedUser);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.SetCurrentUser("loggedUser", null);

            return RedirectToAction("Index", "Home");
        }
    }
}
