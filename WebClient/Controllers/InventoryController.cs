using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace WebClient.Controllers
{
    public class InventoryController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly string baseURI = "https://localhost:44347/api/";
        private readonly string endpoint = "Inventories/";
        public InventoryController(IHttpContextAccessor httpContext)
        {
            httpClient = new HttpClient();
            string token = httpContext.HttpContext.Session.Get<string>("token");
            
            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }
        // GET: Inventory
        public async Task<ActionResult> Index()
        {
            var responseMessage = await httpClient.GetAsync(baseURI + endpoint, HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                List<Inventory> temp = JsonConvert.DeserializeObject<List<Inventory>>(data);
                return View(temp);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = responseMessage.ReasonPhrase
                });
            }
        }

        // GET: Inventory/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var responseMessage = await httpClient.GetAsync(baseURI + endpoint + id.ToString(), HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                Inventory temp = JsonConvert.DeserializeObject<Inventory>(data);
                return View(temp);
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = responseMessage.ReasonPhrase
                });
            }
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Inventory inventory)
        {
            var json = JsonConvert.SerializeObject(inventory, Formatting.None);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync(baseURI + endpoint, httpContent);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = responseMessage.StatusCode + ", įvestis neįveikė API validacijos."
                });
            }
        }

        // GET: Inventory/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var responseMessage = await httpClient.GetAsync(baseURI + endpoint + id.ToString(), HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = responseMessage.StatusCode + ", užklausa nepavyko."
                });
            }
            var data = await responseMessage.Content.ReadAsStringAsync();
            Inventory temp = JsonConvert.DeserializeObject<Inventory>(data);
            return View(temp);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Inventory inventory)
        {
            inventory.Discriminator = "Inventory";
            var json = JsonConvert.SerializeObject(inventory, Formatting.None);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PutAsync(baseURI + endpoint + id.ToString(), httpContent);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = responseMessage.StatusCode + ", užklausa nepavyko."
                });
            }
        }

        // GET: Inventory/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var responseMessage = await httpClient.GetAsync(baseURI + endpoint + id.ToString(), HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = responseMessage.StatusCode + ", užklausa nepavyko."
                });
            }
            var data = await responseMessage.Content.ReadAsStringAsync();
            Inventory temp = JsonConvert.DeserializeObject<Inventory>(data);
            return View(temp);
        }

        // POST: Inventory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var responseMessage = await httpClient.DeleteAsync(baseURI + endpoint + id.ToString());
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = responseMessage.StatusCode + ", užklausa nepavyko."
                });
            }
        }
    }
}