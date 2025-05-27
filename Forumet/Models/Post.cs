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
            public IdentityUser? User { get; set; }  

            [Required]
            public int CategoryId { get; set; }

            [ForeignKey("CategoryId")]
            public Category? Category { get; set; }  

          
            public ICollection<Comment> Comments { get; set; } = new List<Comment>();

            public ICollection<Report> Reports { get; set; } = new List<Report>();
        

    }
}
