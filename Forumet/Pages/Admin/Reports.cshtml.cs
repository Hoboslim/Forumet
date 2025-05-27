using Forumet.Data;
using Forumet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Forumet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ReportsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Report> Reports { get; set; } = new List<Report>();

        public async Task OnGetAsync()
        {
            Reports = await _context.Reports
                .Include(r => r.Post)
                .Include(r => r.User)
                .Include(r => r.Comment)
                .OrderByDescending(r => r.ReportedAt)
                .ToListAsync();

        }

        public async Task<IActionResult> OnPostDeleteAsync(int postId)
        {
            var post = await _context.Posts
                .Include(p => p.Comments) 
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post != null)
            {
               
                var relatedReports = _context.Reports.Where(r => r.PostId == postId);
                _context.Reports.RemoveRange(relatedReports);

               

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostRemoveCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment != null)
            {
                var relatedReports = _context.Reports.Where(r => r.CommentId == commentId);
                _context.Reports.RemoveRange(relatedReports);

                _context.Comments.Remove(comment);

                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostIgnoreAsync(int reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if( report != null )
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
                
        }
    }
}
