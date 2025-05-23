using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forumet.Models
{
    public class Post
    {

       
        
            public int Id { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            public string Content { get; set; }

            public string? ImagePath { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.Now;

          
            public string? UserId { get; set; }

            [ForeignKey("UserId")]
            public IdentityUser? User { get; set; }  // REMOVE [Required] here!

            [Required]
            public int CategoryId { get; set; }

            [ForeignKey("CategoryId")]
            public Category? Category { get; set; }  // REMOVE [Required] here!

            // Initialize collections to empty lists to avoid null refs and validation errors
            public ICollection<Comment> Comments { get; set; } = new List<Comment>();

            public ICollection<Report> Reports { get; set; } = new List<Report>();
        

    }
}
