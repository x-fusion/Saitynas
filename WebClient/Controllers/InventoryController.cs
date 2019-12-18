using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult Index()
        {
            List<Inventory> temp = new List<Inventory>
            {
                new Inventory
                {
                    ID = 1,
                    Amount = 3,
                    TotalRentDuration = 47,
                    Title = "Daiktas",
                    MonetaryValue = 49.8M,
                    Revenue = 99.8M
                },
                new Inventory
                {
                    ID = 2,
                    Amount = 3,
                    TotalRentDuration = 47,
                    Title = "Daiktas2",
                    MonetaryValue = 49.8M,
                    Revenue = 99.8M
                }
            };
            return View(temp);
        }

        // GET: Inventory/Details/5
        public ActionResult Details(int id)
        {
            return View(new Inventory
            {
                ID = 2,
                Amount = 3,
                TotalRentDuration = 47,
                Title = "Daiktas2",
                MonetaryValue = 49.8M,
                Revenue = 99.8M
            });
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventory inventory)
        {
            try
            {

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new Inventory
            {
                ID = 2,
                Amount = 3,
                TotalRentDuration = 47,
                Title = "Daiktas2",
                MonetaryValue = 49.8M,
                Revenue = 99.8M
            });
        }

        // POST: Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inventory inventory)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new Inventory
            {
                ID = 2,
                Amount = 3,
                TotalRentDuration = 47,
                Title = "Daiktas2",
                MonetaryValue = 49.8M,
                Revenue = 99.8M
            });
        }

        // POST: Inventory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}