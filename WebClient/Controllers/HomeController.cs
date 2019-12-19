using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient httpClient;
        private readonly string baseURI = "https://localhost:44347/api/";
        private readonly string usersEndpoint = "Users/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("sub");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("token");
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            var json = JsonConvert.SerializeObject(login, Formatting.None);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync(baseURI + usersEndpoint + "authenticate", httpContent);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                AuthResponse response = JsonConvert.DeserializeObject<AuthResponse>(data);
                if (response.Message == "Authenticated")
                {
                    var handler = new JwtSecurityTokenHandler();
                    JwtSecurityToken token = handler.ReadToken(response.Token) as JwtSecurityToken;
                    HttpContext.Session.Set("sub", token.Subject);
                    HttpContext.Session.Set("role", token.Claims.SingleOrDefault(x => x.Type == "role").Value);
                    HttpContext.Session.Set("token", response.Token);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                AuthResponse response = JsonConvert.DeserializeObject<AuthResponse>(data);
                return View("Error",
                    new ErrorViewModel
                    {
                        ErrorMessage = response.Message
                    });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            if (register.Password != register.PasswordRepeat)
            {
                return View("Error",
                 new ErrorViewModel
                 {
                     ErrorMessage = "Nesutampa slaptažodžiai"
                 });
            }
            var json = JsonConvert.SerializeObject(register, Formatting.None);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync(baseURI + usersEndpoint, httpContent);
            try
            {
                responseMessage.EnsureSuccessStatusCode();
                var data = await responseMessage.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject<object>(data);
                var handler = new JwtSecurityTokenHandler();
                string temp = response.token;
                JwtSecurityToken token = handler.ReadToken(temp) as JwtSecurityToken;
                HttpContext.Session.Set("sub", token.Subject);
                HttpContext.Session.Set("role", token.Claims.SingleOrDefault(x => x.Type == "role").Value);
                HttpContext.Session.Set("token", temp);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                AuthResponse response = JsonConvert.DeserializeObject<AuthResponse>(data);
                return View("Error",
                    new ErrorViewModel
                    {
                        ErrorMessage = response.Message
                    });
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
