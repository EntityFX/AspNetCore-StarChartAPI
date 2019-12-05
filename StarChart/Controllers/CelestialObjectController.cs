using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == item.Id).ToList();

            return Ok(item);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var item = _context.CelestialObjects.FirstOrDefault(e => e.Name == name);
            if (item == null)
            {
                return NotFound();
            }

            item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == item.Id).ToList();

            return Ok(item);
        }

        [HttpGet()]
        public IActionResult GetResult()
        {
            var items = _context.CelestialObjects.ToList();
            foreach (var item in items)
            {
                item.Satellites = items;
            }
            return Ok(items);
        }
    }
}
