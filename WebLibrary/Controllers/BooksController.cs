using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebLibrary.Models;

namespace WebLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private BooksContext LibraryDB { get; }

        public BooksController(BooksContext context)
        {
            if (!context.Books.Any())
            {
                context.Books.Add(new Book // Create test book if empty
                {
                    Title = "Успешный бизнес с нуля",
                    Author = "Мавроди С.П.",
                    Price = 1499.99m
                });
                context.SaveChanges();
            }

            LibraryDB = context;
        }

        // Read object
        // api/books?count=5
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAsync(int count = 10)
        {
            return await LibraryDB.Books.Take(count).ToArrayAsync();
        }

        // Read object by ID
        // api/books/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetByIdAsync(int id)
        {
            Book book = await LibraryDB.Books.FirstOrDefaultAsync(p => p.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return new ObjectResult(book);
        }

        // Create object
        [HttpPost]
        public async Task<ActionResult<Book>> PostAsync(Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            LibraryDB.Books.Add(book);
            await LibraryDB.SaveChangesAsync();

            return Ok(book);
        }

        // Edit object
        [HttpPut]
        public async Task<ActionResult<Book>> PutAsync(Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            if (!LibraryDB.Books.Any(p => p.Id == book.Id))
            {
                return NotFound();
            }

            LibraryDB.Update(book);
            await LibraryDB.SaveChangesAsync();

            return Ok(book);
        }

        // Delete object
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteAsync(int id)
        {
            Book book = await LibraryDB.Books.FirstOrDefaultAsync(p => p.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            LibraryDB.Books.Remove(book);
            await LibraryDB.SaveChangesAsync();

            return Ok(book);
        }
    }
}
