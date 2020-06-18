using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VT.Business.DTOs;
using VT.Website.Models;

namespace VT.Website.Controllers
{
    public class UserController : Controller
    {
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string AUTHORIZATION_HEADER_NAME = "Authorization";
        private readonly Uri usersUri = new Uri("https://localhost:44335/api/User");
        
        private UserDto GetLoggedUser()
        {
            return HttpContext.Session.GetCurrentUser("loggedUser") == null ? null : HttpContext.Session.GetCurrentUser("loggedUser");
        }

        // GET: User
        public async Task<ActionResult> Index(String username = null)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));

                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response;

                if (username.IsNullOrEmpty())
                {
                    response = await client.GetAsync(usersUri);
                }
                else
                {
                    response = await client.GetAsync($"{usersUri}/username={username}");
                }

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<UserVM>>(jsonResponse);

                return View(responseData);
            }
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(int id)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response = await client.GetAsync($"{usersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<UserVM>(jsonResponse);

                return View(responseData);
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserVM user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                    client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                    var serializedContent = JsonConvert.SerializeObject(user);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PostAsync(usersUri, stringContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(HomeController.Error), "Home");
                    }

                    if (GetLoggedUser() != null && GetLoggedUser().isAdmin)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response = await client.GetAsync($"{usersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<UserVM>(jsonResponse);

                return View(responseData);
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UserVM user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.Id = id;

            try
            {
                using (var client = new HttpClient())
                {
                    var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                    client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                    var serializedContent = JsonConvert.SerializeObject(user);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PutAsync($"{usersUri}/{id}", stringContent);

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(HomeController.Error), "Home");
                    }

                    if (GetLoggedUser() != null && GetLoggedUser().isAdmin)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = GetLoggedUser().Id});
                    }
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                bool Admin = HttpContext.Session.GetCurrentUser("loggedUser") == null ? false : HttpContext.Session.GetCurrentUser("loggedUser").isAdmin;

                HttpResponseMessage response = await client.GetAsync($"{usersUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<UserVM>(jsonResponse);

                return View(responseData);
            }
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                    client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                    HttpResponseMessage response = await client.DeleteAsync($"{usersUri}/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(HomeController.Error), "Home");
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }


        public static string Base64Encode(string text)
        {
            var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return $"Basic {System.Convert.ToBase64String(textBytes)}";
        }
    }
}