using Forumet.Data;
using Forumet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Forumet.Pages.Posts
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public IndexModel (ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<Post> Posts { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CategoryId { get; set; }

        public async Task OnGetAsync()
        {
            if (CategoryId == 0)
            {
                Posts = new List<Post>();
                return;
            }

            Posts = await _context.Posts
                        .Where(p => p.CategoryId == CategoryId)
                        .Include(p => p.User)
                        .Include(p => p.Category)
                        .ToListAsync();
        }

    }
}
