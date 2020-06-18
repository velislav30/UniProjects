using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using VT.Business.DTOs;
using VT.Website.Models;

namespace VT.Website.Controllers
{
    public class TorrentController : Controller
    {
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string AUTHORIZATION_HEADER_NAME = "Authorization";
        private readonly Uri torrentUri = new Uri("https://localhost:44335/api/Torrent");
        private readonly Uri subtypeUri = new Uri("https://localhost:44335/api/SubType");
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
        public async Task<ActionResult> Index(int subtypeId,string title = null)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));

                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response;

                if (title.IsNullOrEmpty())
                {
                    response = await client.GetAsync($"{torrentUri}/subtypeId={subtypeId}");
                }
                else
                {
                    response = await client.GetAsync($"{torrentUri}/subtypeId={subtypeId}/{title}");
                }

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<TorrentVM>>(jsonResponse);

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

                HttpResponseMessage response = await client.GetAsync($"{torrentUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<TorrentVM>(jsonResponse);

                return View(responseData);
            }
        }

        public async Task<ActionResult> DownloadTorrent(int subtypeId, int id)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));

                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                int downloaderId = HttpContext.Session.GetCurrentUser("loggedUser") == null ? 0 : HttpContext.Session.GetCurrentUserId("loggedUser");

                HttpResponseMessage response = await client.GetAsync($"{torrentUri}/id={id}/downloaderId={downloaderId}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                return RedirectToAction(nameof(Index),new { subtypeId = subtypeId });
            }
        }


            public async Task<ActionResult> Downloads(int downloaderId)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));

                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response = await client.GetAsync($"{torrentUri}/downloaderId={downloaderId}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<TorrentVM>>(jsonResponse);

                return View(responseData);
            }
        }

        public async Task<ActionResult> Uploads(int uploaderId)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));

                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response = await client.GetAsync($"{torrentUri}/uploaderId={uploaderId}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<IEnumerable<TorrentVM>>(jsonResponse);

                return View(responseData);
            }
        }

        // GET: Catalog/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Catalogs = await GetCatalogDropdownItemsAsync();

            return View();
        }

        public async Task<ActionResult> Create2(TorrentVM torrent)
        {
            ViewBag.Subtypes = await GetSubtypeDropdownItemsAsync(torrent.CatalogId);

            return View(torrent);
        }

        // POST: Catalog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TorrentVM torrent)
        {
            if (!ModelState.IsValid)
            {
                return View(torrent);
            }

            torrent.Uploader = GetLoggedUserAsVM();
            torrent.Catalog = new CatalogVM();
            torrent.Catalog.Id = torrent.CatalogId;
            torrent.SybType = new SubtypeVM();
            torrent.SybType.Id = torrent.SubtypeId;

            try
            {
                using (var client = new HttpClient())
                {
                    var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                    client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                    var serializedContent = JsonConvert.SerializeObject(torrent);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PostAsync(torrentUri, stringContent);

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

                HttpResponseMessage response = await client.GetAsync($"{torrentUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<TorrentVM>(jsonResponse);

                ViewBag.Catalogs = await GetCatalogDropdownItemsAsync();

                responseData.Id = id;

                return View(responseData);
            }
        }

        public async Task<ActionResult> Edit2(int id,TorrentVM torrent)
        {
            ViewBag.Subtypes = await GetSubtypeDropdownItemsAsync(torrent.CatalogId);
            return View(torrent);
        }

        // POST: Catalog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, TorrentVM torrent)
        {
            if (!ModelState.IsValid)
            {
                return View(torrent);
            }

            torrent.Catalog = new CatalogVM();
            torrent.Catalog.Id = torrent.CatalogId;
            torrent.SybType = new SubtypeVM();
            torrent.SybType.Id = torrent.SubtypeId;

            try
            {
                using (var client = new HttpClient())
                {
                    var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                    client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                    var serializedContent = JsonConvert.SerializeObject(torrent);
                    var stringContent = new StringContent(serializedContent, Encoding.UTF8, JSON_MEDIA_TYPE);

                    HttpResponseMessage response = await client.PutAsync($"{torrentUri}/{id}", stringContent);

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

        // GET: Catalog/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage response = await client.GetAsync($"{torrentUri}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonConvert.DeserializeObject<TorrentVM>(jsonResponse);

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

                    HttpResponseMessage response = await client.DeleteAsync($"{torrentUri}/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(HomeController.Error), "Home");
                    }
                }

                return RedirectToAction(nameof(Index), "Catalog");
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

        private async Task<IEnumerable<SelectListItem>> GetCatalogDropdownItemsAsync()
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage catalogsResponse = await client.GetAsync(catalogUri);

                if (!catalogsResponse.IsSuccessStatusCode)
                {
                    return Enumerable.Empty<SelectListItem>();
                }

                string catalogsJsonResponse = await catalogsResponse.Content.ReadAsStringAsync();

                var catalogs = JsonConvert.DeserializeObject<IEnumerable<CatalogVM>>(catalogsJsonResponse);

                return catalogs.Where(c => !GetSubtypeDropdownItemsAsync(c.Id).Result.IsNullOrEmpty())
                    .Select(c => new SelectListItem(c.Title, c.Id.ToString()));
            }
        }

        private async Task<IEnumerable<SelectListItem>> GetSubtypeDropdownItemsAsync(int catalogId)
        {
            using (var client = new HttpClient())
            {
                var token = Base64Encode(HttpContext.Session.GetCurrentUserCredentials("loggedUser"));
                client.DefaultRequestHeaders.Add(AUTHORIZATION_HEADER_NAME, token);

                HttpResponseMessage subtypeResponse = await client.GetAsync($"{subtypeUri}/catalogId={catalogId}");

                if (!subtypeResponse.IsSuccessStatusCode)
                {
                    return Enumerable.Empty<SelectListItem>();
                }

                string subtypessJsonResponse = await subtypeResponse.Content.ReadAsStringAsync();

                var subtypes = JsonConvert.DeserializeObject<IEnumerable<SubtypeVM>>(subtypessJsonResponse);

                return subtypes.Select(s => new SelectListItem(s.Title, s.Id.ToString()));
            }
        }
    }
}