using BookHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using DataAccessLayer.Entities;
using BookHub.Models.Details;

namespace BookHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly BookHubDbContext _context;

        public AuthorController(BookHubDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDetail>>> GetAuthors()
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            
            return await _context.Authors
                .Include(a => a.Books)
                .Select(a => ControllerHelpers.MapAuthorToAuthorDetail(a))
                .ToListAsync();
        }
        
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<AuthorDetail>> GetAuthorById(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context
                .Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound($"Author with ID:'{id}' not found");
            }

            return ControllerHelpers.MapAuthorToAuthorDetail(author);
        }

        // GET: api/Book/name
        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult<AuthorDetail>> GetAuthorByName(string name)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context
                .Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(b => b.Name == name);

            if (author == null)
            {
                return NotFound($"Author '{name}' not found");
            }

            return ControllerHelpers.MapAuthorToAuthorDetail(author);
        }
        
        [HttpPost]
        public async Task<ActionResult<AuthorDetail>> PostAuthor(AuthorModel authorModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid!");
            }

            if (_context.Authors == null)
            {
                return Problem("Entity set 'BookHubDbContext.Books'  is null.");
            }
            

            var author = new Author()
            {
                Name = authorModel.Name,
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return ControllerHelpers.MapAuthorToAuthorDetail(author);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}