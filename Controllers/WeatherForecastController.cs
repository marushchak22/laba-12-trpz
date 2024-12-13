using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using YourNamespace.Models;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandTypeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BrandTypeApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandType>>> GetBrandTypes()
        {
            return await _context.BrandType
                .Include(bt => bt.BicycleType)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BrandType>> GetBrandType(int id)
        {
            var brandType = await _context.BrandType
                .Include(bt => bt.BicycleType)
                .FirstOrDefaultAsync(bt => bt.BrandTypeID == id);

            if (brandType == null)
            {
                return NotFound();
            }

            return brandType;
        }

        [HttpPost]
        public async Task<ActionResult<BrandType>> PostBrandType(BrandType brandType)
        {
            _context.BrandType.Add(brandType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrandType", new { id = brandType.BrandTypeID }, brandType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrandType(int id, BrandType brandType)
        {
            if (id != brandType.BrandTypeID)
            {
                return BadRequest();
            }

            _context.Entry(brandType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandTypeExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrandType(int id)
        {
            var brandType = await _context.BrandType.FindAsync(id);
            if (brandType == null)
            {
                return NotFound();
            }

            _context.BrandType.Remove(brandType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrandTypeExists(int id)
        {
            return _context.BrandType.Any(e => e.BrandTypeID == id);
        }

       
    }
}
