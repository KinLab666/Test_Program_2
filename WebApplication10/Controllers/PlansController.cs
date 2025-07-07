using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication10.Data;
using WebApplication10.models;

namespace WebApplication10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlansController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/plans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlans()
        {
            return await _context.Plans.Include(p => p.Tags).ToListAsync();
        }

        // GET: api/plans/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlan(int id)
        {
            var plan = await _context.Plans.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id);
            if (plan == null) return NotFound();
            return plan;
        }

        // POST: api/plans
        [HttpPost]
        public async Task<ActionResult<Plan>> CreatePlan(Plan plan)
        {
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlan), new { id = plan.Id }, plan);
        }

        // PUT: api/plans/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(int id, Plan updatedPlan)
        {
            if (id != updatedPlan.Id) return BadRequest();

            _context.Entry(updatedPlan).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/plans/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var plan = await _context.Plans.FindAsync(id);
            if (plan == null) return NotFound();

            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
