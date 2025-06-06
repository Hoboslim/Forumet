using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Forumet.Pages.Messages
{
    public class DmModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DmModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public string OtherUserId { get; set; }

        [BindProperty]
        public string MessageContent { get; set; }

        public string RecieverUserName { get; set; }
        public bool Success { get; set; }
        public async Task OnGetAsync()
        {
            RecieverUserName = OtherUserId;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(OtherUserId) || string.IsNullOrEmpty(MessageContent))
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("MessagingApi");
            var message = new
            {
                SenderId = senderId,
                Content = MessageContent,
                ReceiverId = OtherUserId,

            };

            var response = await client.PostAsJsonAsync("api/messages", message);
            Success = response.IsSuccessStatusCode;

            return Page();
        }
    }
}
