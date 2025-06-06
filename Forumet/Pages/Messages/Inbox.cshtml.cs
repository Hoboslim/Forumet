using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Forumet.Pages.Messages
{
    public class InboxModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public InboxModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<ConversationDto> Conversations { get; set; } = new();

        public async Task OnGetAsync()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return;

            var client = _httpClientFactory.CreateClient("MessagingApi");
            var response = await client.GetAsync($"api/messages/conversations/{currentUserId}");
            if (response.IsSuccessStatusCode)
            {
                Conversations = await response.Content.ReadFromJsonAsync<List<ConversationDto>>() ?? new();
            }
        }

        public class ConversationDto
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
        }
    }
}

