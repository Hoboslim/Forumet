using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Forumet.Models
{
    public class Category
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }    

        public string? ImagePath { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
