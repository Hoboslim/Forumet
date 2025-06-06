using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumetApi.Models
{
    public class PrivateMessage
    {
        public int Id { get; set; }

        public string Messages { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public string SenderId { get; set; }

        [ForeignKey("SenderId")]
        public IdentityUser Sender { get; set; }

        public string RecieverId { get; set; }

        [ForeignKey("RecieverId")]
        public IdentityUser Reciever { get; set; }
    }
}
