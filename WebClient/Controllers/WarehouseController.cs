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
    public class WarehouseController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly string baseURI = "https://localhost:44347/api/";
        private readonly string endpoint = "Warehouses/";
        public WarehouseController(IHttpContextAccessor httpContext)
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
                List<Warehouse> temp = JsonConvert.DeserializeObject<List<Warehouse>>(data);
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
        public async Task<ActionResult> Orders(int id)
        {
            var responseMessage = await httpClient.GetAsync(baseURI + endpoint + id + "/orders", HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                List<Order> temp = JsonConvert.DeserializeObject<List<Order>>(data);
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
                Warehouse temp = JsonConvert.DeserializeObject<Warehouse>(data);
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
        public async Task<ActionResult> DetailsOrder(int id, int orderId)
        {
            var responseMessage = await httpClient.GetAsync(baseURI + endpoint + id.ToString() + "/orders/" + orderId, HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                Order temp = JsonConvert.DeserializeObject<Order>(data);
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
        public async Task<ActionResult> CreateOrder(int id)
        {
            ViewBag.inventories = await GetInventories();
            ViewBag.bicycleRacks = await GetBicycleRacks();
            ViewBag.roofRacks = await GetRoofRacks();
            ViewBag.wheelChains = await GetWheelChains();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateOrder(int id, Order order)
        {
            order.WarehouseID = id;
            order.ID = 0;
            order.CreationDate = DateTime.Now;
            var json = JsonConvert.SerializeObject(order, Formatting.None);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync(baseURI + endpoint + id + "/orders", httpContent);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Orders), new { id = id });
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
        public async Task<ActionResult> EditOrder(int id, int orderId)
        {
            var responseMessage = await httpClient.GetAsync(baseURI + endpoint + id.ToString() + "/orders/" + orderId, HttpCompletionOption.ResponseHeadersRead);
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
            ViewBag.inventories = await GetInventories();
            ViewBag.bicycleRacks = await GetBicycleRacks();
            ViewBag.roofRacks = await GetRoofRacks();
            ViewBag.wheelChains = await GetWheelChains();
            var data = await responseMessage.Content.ReadAsStringAsync();
            Order temp = JsonConvert.DeserializeObject<Order>(data);
            return View(temp);
        }
        public async Task<ActionResult> DeleteOrder(int id, int orderId)
        {
            var responseMessage = await httpClient.GetAsync(baseURI + endpoint + id.ToString() + "/orders/" + orderId, HttpCompletionOption.ResponseHeadersRead);
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
            Order temp = JsonConvert.DeserializeObject<Order>(data);
            return View(temp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOrder(int id, int orderId, IFormCollection collection)
        {
            var responseMessage = await httpClient.DeleteAsync(baseURI + endpoint + id.ToString() + "/orders/" + orderId);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Orders), new { id = id });
            }
            catch
            {
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = responseMessage.StatusCode + ", užklausa nepavyko."
                });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditOrder(int id, int orderId, Order order)
        {
            order.WarehouseID = id;
            order.ID = orderId;
            order.CreationDate = DateTime.Now;
            var json = JsonConvert.SerializeObject(order, Formatting.None);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PutAsync(baseURI + endpoint + id + "/orders/" + orderId, httpContent);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Orders), new { id = id });
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
        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Warehouse warehouse)
        {
            var json = JsonConvert.SerializeObject(warehouse, Formatting.None);
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
            Warehouse temp = JsonConvert.DeserializeObject<Warehouse>(data);
            return View(temp);
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Warehouse warehouse)
        {
            var json = JsonConvert.SerializeObject(warehouse, Formatting.None);
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
            Warehouse temp = JsonConvert.DeserializeObject<Warehouse>(data);
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

        private async Task<List<BicycleRack>> GetBicycleRacks()
        {
            var responseMessage = await httpClient.GetAsync(baseURI + "BicycleRacks", HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                List<BicycleRack> temp = JsonConvert.DeserializeObject<List<BicycleRack>>(data);
                return temp;
            }
            catch
            {
                return new List<BicycleRack>();
            }
        }
        private async Task<List<Inventory>> GetInventories()
        {
            var responseMessage = await httpClient.GetAsync(baseURI + "Inventories", HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                List<Inventory> temp = JsonConvert.DeserializeObject<List<Inventory>>(data);
                return temp;
            }
            catch
            {
                return new List<Inventory>();
            }
        }
        private async Task<List<RoofRack>> GetRoofRacks()
        {
            var responseMessage = await httpClient.GetAsync(baseURI + "RoofRacks", HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                List<RoofRack> temp = JsonConvert.DeserializeObject<List<RoofRack>>(data);
                return temp;
            }
            catch
            {
                return new List<RoofRack>();
            }
        }
        private async Task<List<WheelChain>> GetWheelChains()
        {
            var responseMessage = await httpClient.GetAsync(baseURI + "WheelChains", HttpCompletionOption.ResponseHeadersRead);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                List<WheelChain> temp = JsonConvert.DeserializeObject<List<WheelChain>>(data);
                return temp;
            }
            catch
            {
                return new List<WheelChain>();
            }
        }
    }
}