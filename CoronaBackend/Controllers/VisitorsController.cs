using CoronaBackend.Data;
using CoronaBackend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoronaBackend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class VisitorsController : ControllerBase
    {
        private readonly ILogger<VisitorsController> _logger;
        private readonly AppDbContext _contextDb;
        private readonly DateOnly minBirthDate = new DateOnly(1900,1,1);
        private readonly DateOnly maxBirthDate = new DateOnly(3000,1,1);

        public VisitorsController(AppDbContext context, ILogger<VisitorsController> logger)
        {
            _contextDb = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all visitors
        /// </summary>
        /// <returns>Returns a list of visitors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Visitor>>> Get()
        {
            _logger.LogDebug("This is a debug message");

            return Ok(await _contextDb.Visitor.ToListAsync());
        }

        /// <summary>
        /// Find visitor by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a single visitor</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Visitor>> Get(int id)
        {
            var visitor = await _contextDb.Visitor.FirstOrDefaultAsync(v => v.Id == id);

            if (visitor == null) return NotFound();

            return Ok(visitor);
        }

        /// <summary>
        /// Add a new visitor to the store 
        /// </summary>
        /// <returns>A newly created visitor</returns>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Visitor>> Post([FromBody] Visitor visitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            };

            if (visitor.BirthDate < minBirthDate || visitor.BirthDate > maxBirthDate)
            {
                return BadRequest();
            }

            var date = DateTime.UtcNow;
            visitor.DateAdded = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);

            _contextDb.Visitor.Add(visitor);

            try
            {
                await _contextDb.SaveChangesAsync();

                return Ok(visitor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Saving the visitor to the database");
                return Problem("Internal Server Error", "", 500);
            }
        }
    }
}