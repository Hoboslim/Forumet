using Forumet.Data;
using Forumet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Forumet.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateCategoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateCategoryModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public IFormFile? Image { get; set; }
        public bool Success { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                return Page();
            }

            string? imagePath = null;
            if(Image != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "uploads" ,"categories");
                Directory.CreateDirectory(uploadsFolder);
                var fileName = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                var FilePath = Path.Combine(uploadsFolder, fileName); 

                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
                imagePath = $"images/uploads/categories/{fileName}";
            }

            var category = new Category { Name = Name, ImagePath = imagePath};
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            Success = true;
            Name = string.Empty;
            return Page();
        }
    }
}
