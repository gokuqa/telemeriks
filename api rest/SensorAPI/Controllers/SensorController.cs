using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorAPI.Data;
using SensorAPI.Models;

namespace SensorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SensorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/sensor
        [HttpGet]
        public async Task<IActionResult> GetSensors(int page = 1, int limit = 10)
        {
            var sensors = await _context.Sensores
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            var total = await _context.Sensores.CountAsync();

            return Ok(new
            {
                total,
                page,
                limit,
                sensors
            });
        }
    }
}
