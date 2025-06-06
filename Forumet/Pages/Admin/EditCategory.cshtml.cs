using Forumet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Forumet.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class EditCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditCategoryModel(ApplicationDbContext context, IWebHostEnvironment env)
        {   
            _context = context;
            _env = env;
            
        }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public IFormFile? Image { get; set; }

        public string? ExistingImagePath { get; set; }
        public bool Success { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if(category == null)
                return NotFound();

            id = category.Id;
            Name = category.Name;
            ExistingImagePath = category.ImagePath;
            return Page();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var category = await _context.Categories.FindAsync(Id);
                if(category == null )
                return NotFound();

                category.Name = Name;

            if(Image != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "uploads", "categories");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                category.ImagePath = $"images/uploads/categories/{fileName}";
            }

            await _context.SaveChangesAsync();
            ExistingImagePath = category.ImagePath;
            Success = true;
            return Page();

        }
    }
}
