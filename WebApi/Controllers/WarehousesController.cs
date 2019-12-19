using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public WarehousesController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            return await _context.Warehouses.ToListAsync();
        }

        // GET: api/Warehouses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return warehouse;
        }
        // GET: api/Warehouses/1/orders
        [HttpGet("{id}/orders")]
        public ActionResult<List<Order>> GetWarehouseOrders(int id)
        {
            var orders =  _context.Orders.Where(x => x.WarehouseID == id).ToList();
            if (orders == null)
            {
                return NotFound();
            }
            return orders;
        }
        // GET: api/Warehouses/1/orders/4
        [HttpGet("{id}/orders/{orderId}")]
        public ActionResult<Order> GetWarehouseOrder(int id, int orderId)
        {
            var warehouse = _context.Warehouses.Where(x => x.ID == id)
                .Include(w => w.Orders)
                    .ThenInclude(o => o.BicycleRack)
                .Include(w => w.Orders)
                    .ThenInclude(o => o.RoofRack)
                .Include(w => w.Orders)
                    .ThenInclude(o => o.Inventory)
                .Include(w => w.Orders)
                    .ThenInclude(o => o.WheelChain)
                .FirstOrDefault();

            if (warehouse == null)
            {
                return NotFound();
            }
            if (warehouse.Orders.Count == 0)
            {
                return NoContent();
            }
            var result = warehouse.Orders.SingleOrDefault(x => x.ID == orderId);
            if(result == null)
            {
                return NotFound();
            }
            result.Warehouse = null;
            return result;
        }
        // PUT: api/Warehouses/5/order/1
        [HttpPut("{id}/orders/{orderId}")]
        public async Task<IActionResult> PutWarehouseOrder(int id, int orderId, Order order)
        {
            if (orderId != order.ID || order.WarehouseID != id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(orderId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE: api/Warehouses/1/orders/5
        [HttpDelete("{id}/orders/{orderId}")]
        public ActionResult<Order> DeleteWarehouseOrder(int id, int orderId)
        {
            var order = _context.Orders.SingleOrDefault(x => x.ID == orderId && x.WarehouseID == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return order;
        }
        [HttpPost("{id}/orders")]
        public async Task<ActionResult<Warehouse>> PostOrder(int id, Order order)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            warehouse.Orders.Add(order);

            await _context.SaveChangesAsync();
            int orderId = order.ID;
            return CreatedAtAction("GetWarehouseOrder", new { id, orderId }, orderId);
        }

        // PUT: api/Warehouses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(int id, Warehouse warehouse)
        {
            if (id != warehouse.ID)
            {
                return BadRequest();
            }

            _context.Entry(warehouse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Warehouses
        [HttpPost]
        public async Task<ActionResult<Warehouse>> PostWarehouse(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWarehouse", new { id = warehouse.ID }, warehouse);
        }

        // DELETE: api/Warehouses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Warehouse>> DeleteWarehouse(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();

            return warehouse;
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.ID == id);
        }
        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.ID == id);
        }
    }
}
