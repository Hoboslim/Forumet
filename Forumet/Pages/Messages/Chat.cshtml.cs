using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Forumet.Pages.Messages
{
    public class ChatModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public string OtherUserId { get; set; }

        [BindProperty]
        public string MessageContent { get; set; }

        public List<MessageDto> Conversation { get; set; } = new();
        public string CurrentUserId { get; set; }
        public string OtherUserEmail { get; set; } 

        public async Task OnGetAsync()
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(CurrentUserId) && !string.IsNullOrEmpty(OtherUserId))
            {
                var client = _httpClientFactory.CreateClient("MessagingApi");

                // Fetch the other user's email
                var userResponse = await client.GetAsync($"api/messages/user/{OtherUserId}");
                if (userResponse.IsSuccessStatusCode)
                {
                    var userInfo = await userResponse.Content.ReadFromJsonAsync<UserInfoDto>();
                    OtherUserEmail = userInfo?.Email ?? OtherUserId;
                }
                else
                {
                    OtherUserEmail = OtherUserId;
                }

                var response = await client.GetAsync($"api/messages/conversation/{CurrentUserId}/{OtherUserId}");
                if (response.IsSuccessStatusCode)
                {
                    Conversation = await response.Content.ReadFromJsonAsync<List<MessageDto>>() ?? new();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(CurrentUserId) || string.IsNullOrEmpty(OtherUserId) || string.IsNullOrEmpty(MessageContent))
            {
                await OnGetAsync();
                return Page();
            }

            var client = _httpClientFactory.CreateClient("MessagingApi");
            var message = new
            {
                SenderId = CurrentUserId,
                ReceiverId = OtherUserId,
                Content = MessageContent
            };

            var response = await client.PostAsJsonAsync("api/messages", message);

            await OnGetAsync();
            MessageContent = string.Empty;
            return Page();
        }

        public class MessageDto
        {
            public string SenderId { get; set; }
            public string ReceiverId { get; set; }
            public string Content { get; set; }
            public DateTime SentAt { get; set; }
        }

        public class UserInfoDto 
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }
    }
}
