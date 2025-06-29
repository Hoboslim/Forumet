using Forumet.Data;
using Forumet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Forumet.Pages.Posts
{
    public class ReportModel : PageModel
    {
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<IdentityUser> _userManager;


        public ReportModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int PostId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CommentId { get; set; }

        [BindProperty]
        public string Reason { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (string.IsNullOrWhiteSpace(Reason))
            {
                ModelState.AddModelError("Reason", "Please provide a reason for the report");
                return Page();
            }

            var report = new Report
            {
                PostId = PostId,
                CommentId = CommentId,
                UserId = user.Id,
                Reason = Reason,
                ReportedAt = DateTime.Now,

            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            int redirectPostId = PostId;
            if(CommentId.HasValue)
            {
                if  (PostId==0)
                {
                    var comment = await _context.Comments.FindAsync(CommentId.Value);
                    if(comment != null)
                    {
                        redirectPostId = comment.PostId;
                    }
                }
            }
            return RedirectToPage("/Posts/Details", new { id = PostId });
        }
       
    }
}
