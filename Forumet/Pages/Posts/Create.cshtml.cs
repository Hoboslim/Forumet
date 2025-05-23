using Forumet.Data;
using Forumet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.CodeDom;

namespace Forumet.Pages.Posts
{




    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        [BindProperty]
        public Post Post { get; set; }

        
        [BindProperty]
        public IFormFile? Image { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CategoryId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Post = new Post { CategoryId = CategoryId };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in.");
                return Page();
            }

            Post.UserId = user.Id;
            Post.CreatedAt = DateTime.Now;

           
            if (Image != null && Image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                Post.ImagePath = Path.Combine("images", "uploads", fileName).Replace("\\", "/");
            }

            _context.Posts.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Posts/Index", new { CategoryId = Post.CategoryId });
        }



    }
}




