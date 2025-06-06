using ForumetApi.Data;
using ForumetApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ForumetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MessageController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.PrivateMessages.ToListAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] PrivateMessage message)
        {
            message.SentAt = DateTime.UtcNow;
            _context.PrivateMessages.Add(message);
            await _context.SaveChangesAsync();
            return Ok(message);
        }
    }

    
    


       

}
