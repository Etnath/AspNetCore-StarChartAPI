using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);

            if (celestialObject == null) return NotFound();

            celestialObject.Satellites = _context.CelestialObjects.Where(f => f.OrbitedObjectId == id).ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(f => f.Name == name).ToList();

            if (celestialObjects.Count == 0) return NotFound();

            foreach (var celestialObject in _context.CelestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(f => f.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var celestialObjects = new List<CelestialObject>();
            foreach (var celestialObject in _context.CelestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(f => f.OrbitedObjectId == celestialObject.Id).ToList();
                celestialObjects.Add(celestialObject);
            }

            return Ok(celestialObjects);
        }
    }
}
