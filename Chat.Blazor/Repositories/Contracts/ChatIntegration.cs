using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Chat.Blazor.DTOs;
using Chat.Blazor.Models;
using Chat.Blazor.Services;

namespace Chat.Blazor.Repositories.Contracts
{
    public class ChatIntegration(HttpClient httpClient, StorageService storageService) : IChatIntegration
    {
        private readonly HttpClient _httpClient = httpClient;

        private readonly StorageService _storageService = storageService;

        public async Task<Tuple<HttpStatusCode, object>> GetUserChats()
        {
            await AddTokenToHeader();


            var url = "api/users/user_id/chats";

            var result =await _httpClient.GetAsync(url);

            var statusCode = result.StatusCode;

            object response;

            if (statusCode == HttpStatusCode.OK)
            {
                response = (await result.Content.ReadFromJsonAsync<List<ChatDto>>())!;

            }

            else
            {
                response = (await result.Content.ReadFromJsonAsync<string>())!;
            }

            return new(statusCode, response);

        }

        public async Task<Tuple<HttpStatusCode, object>> GetUserChat(Guid toUserId)
        {
            var url = "api/users/user_id/chats";

            await AddTokenToHeader();

            var result = await _httpClient.PostAsJsonAsync(url, toUserId);

            var statusCode =  result.StatusCode;

            object response;

            if (statusCode==HttpStatusCode.OK)
            {
                response = result.Content.ReadFromJsonAsync<ChatDto>();
            }

            else
            {
                response = result.Content.ReadFromJsonAsync<string>();
            }

            return new(statusCode, response);
        }

        public async Task<Tuple<HttpStatusCode, object>> GetChatMessages(Guid chatId)
        {
            await AddTokenToHeader();

            string url = $"api/users/user_id/chats/{chatId}/messages";

            var result = await _httpClient.GetAsync(url);

            var statusCode = result.StatusCode;

            object response;

            if (statusCode == HttpStatusCode.OK)
            {
                response = result.Content.ReadFromJsonAsync<List<MessageDto>>();
            }

            else
            {
                response=result.Content.ReadFromJsonAsync<string>();
            }

            return new(statusCode, response);

        }

        public async Task<Tuple<HttpStatusCode, object>> SendTextMessage(Guid chatId, string text)
        {
            string url = $"api/users/user_id/chats/{chatId}/Messages/send-text-message";

            var model = new TextModel()
            {
                Text = text
            };


            var result = await httpClient.PostAsJsonAsync(url, model);

            var statusCode = result.StatusCode;

            object response;

            if (statusCode == HttpStatusCode.OK)
            {
                response= result.Content.ReadFromJsonAsync<MessageDto>();
            }
            else
            {
                response = result.Content.ReadFromJsonAsync<string>();
            }

            return new(statusCode,response);
        }


        private async Task AddTokenToHeader()
        {
            string? token = await _storageService.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
