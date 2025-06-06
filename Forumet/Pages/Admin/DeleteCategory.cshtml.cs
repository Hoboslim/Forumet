using Forumet.Data;
using Forumet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Forumet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DeleteCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteCategoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category? Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _context.Categories.FindAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Categories/Index");
        }
    }
}
