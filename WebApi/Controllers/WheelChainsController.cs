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
    public class WheelChainsController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public WheelChainsController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/WheelChains
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WheelChain>>> GetWheelChains()
        {
            return await _context.WheelChains.ToListAsync();
        }

        // GET: api/WheelChains/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WheelChain>> GetWheelChain(int id)
        {
            var wheelChain = await _context.WheelChains.FindAsync(id);

            if (wheelChain == null)
            {
                return NotFound();
            }

            return wheelChain;
        }

        // PUT: api/WheelChains/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWheelChain(int id, WheelChain wheelChain)
        {
            if (id != wheelChain.ID)
            {
                return BadRequest();
            }

            _context.Entry(wheelChain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WheelChainExists(id))
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

        // POST: api/WheelChains
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<WheelChain>> PostWheelChain(WheelChain wheelChain)
        {
            _context.WheelChains.Add(wheelChain);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWheelChain", new { id = wheelChain.ID }, wheelChain);
        }

        // DELETE: api/WheelChains/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WheelChain>> DeleteWheelChain(int id)
        {
            var wheelChain = await _context.WheelChains.FindAsync(id);
            if (wheelChain == null)
            {
                return NotFound();
            }

            _context.WheelChains.Remove(wheelChain);
            await _context.SaveChangesAsync();

            return wheelChain;
        }

        private bool WheelChainExists(int id)
        {
            return _context.WheelChains.Any(e => e.ID == id);
        }
    }
}
