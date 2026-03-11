using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q2_SE182117.Models;

namespace Q2_SE182117.Controllers
{
    public class BookController : Controller
    {
        private readonly PE_PRN_25FallB5_23Context _context;

        public BookController(PE_PRN_25FallB5_23Context context)
        {
            _context = context;
        }

        // Trang danh sách sách
        public async Task<IActionResult> Index(int? authorId)
        {
            ViewBag.Authors = await _context.Authors.ToListAsync();

            // Lấy sách kèm theo danh sách Tác giả (many-to-many)
            var query = _context.Books.Include(b => b.Authors).AsQueryable();

            if (authorId.HasValue)
            {
                query = query.Where(b => b.Authors.Any(a => a.AuthorId == authorId));
            }

            return View(await query.ToListAsync());
        }

        // Trang chi tiết sách
        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
    }
}
