using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
            var items = _context.CelestialObjects.Where(e => e.Name == name).ToList();
            if (!items.Any())
            {
                return NotFound();
            }

            foreach (var item in items)
            {
                item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(items);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var items = _context.CelestialObjects.ToList();
            foreach (var item in items)
            {
                item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(items);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject model)
        {
            _context.CelestialObjects.Add(model);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new {id = model.Id}, model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]CelestialObject model)
        {
            var item = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = model.Name;
            item.OrbitedObjectId = model.OrbitedObjectId;
            item.OrbitalPeriod = model.OrbitalPeriod;

            _context.CelestialObjects.Update(item);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var item = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = name;

            _context.CelestialObjects.Update(item);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var items = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id).ToList();
            if (!items.Any())
            {
                return NotFound();
            }

            _context.RemoveRange(items);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
