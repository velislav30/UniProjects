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
using VT.Models.Entities;
using VT.Website.Models;

namespace VT.Website.Controllers
{
    public class CatalogController : Controller
    {
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string AUTHORIZATION_HEADER_NAME = "Authorization";
        private readonly Uri catalogUri = new Uri("https://localhost:44335/api/Catalog");

        private UserDto GetLoggedUser()
        {
            return HttpContext.Session.GetCurrentUser("loggedUser") == null ? null : HttpContext.Session.GetCurrentUser("loggedUser");
        }

        private UserVM GetLoggedUserAsVM()
        {
            UserDto loggedUser = HttpContext.Session.GetCurrentUser("loggedUser") == null ? null : HttpContext.Session.GetCurrentUser("loggedUser");

            UserVM user = new UserVM();

            user.Id = loggedUser.Id;

            return user;
        }

        // GET: Catalog
        public async Task<ActionResult> Index(String title = null)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));

                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response;

                if (title.IsNullOrEmpty())
                {
                    response = await client.GetAsync(catalogUri);
                }
                else
                {
                    response = await client.GetAsync($"{catalogUri}/title={title}");
                }

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Login), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<CatalogVM>>(jsonResponse);

                return View(responseData);
            }
        }

        // GET: Catalog/Details/5
        public async Task<ActionResult> Details(int id)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response = await client.GetAsync($"{catalogUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<CatalogVM>(jsonResponse);

                return View(responseData);
            }
        }

        // GET: Catalog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catalog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CatalogVM catalog)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (!ModelState.IsValid)
                    {
                        return View(catalog);
                    }

                    catalog.Creator = GetLoggedUserAsVM();

                    var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                    client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                    var serializedContent = JsonConvert.SerializeObject(catalog);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PostAsync(catalogUri, stringContent);

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
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: Catalog/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response = await client.GetAsync($"{catalogUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<CatalogVM>(jsonResponse);

                return View(responseData);
            }
        }

        // POST: Catalog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CatalogVM catalog)
        {
            if (!ModelState.IsValid)
            {
                return View(catalog);
            }

            catalog.Id = id;
            catalog.Creator = GetLoggedUserAsVM();

            try
            {
                using (var client = new HttpClient())
                {
                    var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                    client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                    var serializedContent = JsonConvert.SerializeObject(catalog);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PutAsync($"{catalogUri}/{id}", stringContent);

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
                        return RedirectToAction("Index","Home");
                    }
                }
            }
            catch
            {
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
        }

        // GET: Catalog/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response = await client.GetAsync($"{catalogUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<CatalogVM>(jsonResponse);

                return View(responseData);
            }
        }

        // POST: Catalog/Delete/5
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

                    HttpResponseMessage response = await client.DeleteAsync($"{catalogUri}/{id}");

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