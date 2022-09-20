using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSproject.Models;

namespace PRSproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly PRSDbContext _context;

        public RequestsController(PRSDbContext context)
        {
            _context = context;
        }

        string REVIEW = "REVIEW";
        string NEW = "NEW";
        string CLOSED = "CLOSED";
        string PROCESSING = "PROCESSING";
        string OPEN = "OPEN";
        string APPROVED = "APPROVED";
        string REJECTED = "REJECTED";

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // GET: api/requests/reviews/{userid}
        [HttpGet("reviews/{userid}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetReviews(int userId) {
            
            return await _context.Requests.Where(x =>
                x.UserId != userId && x.Status == REVIEW).ToListAsync();


        }
        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        //PUT: api/requests/review/{requestId}
        [HttpPut("review/{id}")]
        public async Task<IActionResult> PutStatusRequest(int id, Request request) {
            if (id != request.Id)
                return BadRequest();
            _context.Entry(request).State = EntityState.Modified; //updates the entity

            request.Status = (request.Total >= 50) ? REVIEW : APPROVED; //change made to the property otherwise

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //PUT: api/requests/approve/{requestId}
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> PutStatusApproved(int id, Request request) {
            if (id != request.Id)
                return BadRequest();

            request.Status = APPROVED;

            _context.Entry(request).State= EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //PUT: api/requests/reject/{requestId}
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> PutStatusRejected(int id, Request request) {
            if (id != request.Id)
                return BadRequest();
            request.Status = REJECTED;

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
