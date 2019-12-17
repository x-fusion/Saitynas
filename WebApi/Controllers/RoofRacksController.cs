using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoofRacksController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public RoofRacksController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/RoofRacks
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IEnumerable<RoofRack>>> GetRoofRacks()
        {
            return await _context.RoofRacks.ToListAsync();
        }

        // GET: api/RoofRacks/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoofRack>> GetRoofRack(int id)
        {
            var roofRack = await _context.RoofRacks.FindAsync(id);

            if (roofRack == null)
            {
                return NotFound();
            }

            return roofRack;
        }

        // PUT: api/RoofRacks/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoofRack(int id, RoofRack roofRack)
        {
            if (id != roofRack.ID)
            {
                return BadRequest();
            }

            _context.Entry(roofRack).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoofRackExists(id))
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

        // POST: api/RoofRacks
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RoofRack>> PostRoofRack(RoofRack roofRack)
        {
            _context.RoofRacks.Add(roofRack);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoofRack", new { id = roofRack.ID }, roofRack);
        }

        // DELETE: api/RoofRacks/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoofRack>> DeleteRoofRack(int id)
        {
            var roofRack = await _context.RoofRacks.FindAsync(id);
            if (roofRack == null)
            {
                return NotFound();
            }

            _context.RoofRacks.Remove(roofRack);
            await _context.SaveChangesAsync();

            return roofRack;
        }

        private bool RoofRackExists(int id)
        {
            return _context.RoofRacks.Any(e => e.ID == id);
        }
    }
}
