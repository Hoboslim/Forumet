using Forumet.Data;
using Forumet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Forumet.Pages.Posts
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public string NewComment { get; set; }

        public DetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Post Post { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Post == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if(!string.IsNullOrWhiteSpace(NewComment))
            {
                var comment = new Comment
                {
                    Text = NewComment,
                    CreatedAt = DateTime.Now,
                    UserId = user.Id,
                    PostId = post.Id
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { id });
        }
        
    }
}
