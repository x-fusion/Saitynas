using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BicycleRacksController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public BicycleRacksController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/BicycleRacks
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<BicycleRack>>> GetBicycleRacks()
        {
            return await _context.BicycleRacks.ToListAsync();
        }

        // GET: api/BicycleRacks/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BicycleRack>> GetBicycleRack(int id)
        {
            var bicycleRack = await _context.BicycleRacks.FindAsync(id);

            if (bicycleRack == null)
            {
                return NotFound();
            }

            return bicycleRack;
        }

        // PUT: api/BicycleRacks/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBicycleRack(int id, BicycleRack bicycleRack)
        {
            if (id != bicycleRack.ID)
            {
                return BadRequest();
            }

            _context.Entry(bicycleRack).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BicycleRackExists(id))
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

        // POST: api/BicycleRacks
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BicycleRack>> PostBicycleRack(BicycleRack bicycleRack)
        {
            _context.BicycleRacks.Add(bicycleRack);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBicycleRack", new { id = bicycleRack.ID }, bicycleRack);
        }

        // DELETE: api/BicycleRacks/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BicycleRack>> DeleteBicycleRack(int id)
        {
            var bicycleRack = await _context.BicycleRacks.FindAsync(id);
            if (bicycleRack == null)
            {
                return NotFound();
            }

            _context.BicycleRacks.Remove(bicycleRack);
            await _context.SaveChangesAsync();

            return bicycleRack;
        }

        private bool BicycleRackExists(int id)
        {
            return _context.BicycleRacks.Any(e => e.ID == id);
        }
    }
}
