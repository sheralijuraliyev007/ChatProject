using System.Net;
using System.Security.Claims;
using Chat.Blazor.DTOs;
using Chat.Blazor.Repositories.Contracts;
using Chat.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Chat.Blazor.Pages.ChatPages
{
    public abstract class UserChatsBase:ComponentBase
    {

        [Inject]

        private IChatIntegration ChatIntegration { get; set; }


        [Inject]
        private NavigationManager NavigationManager { get; set; }


        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        protected List<ChatDto> Chats { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            

            var (statusCode, response) = await ChatIntegration.GetUserChats();

            if (statusCode == HttpStatusCode.OK)
            {
                Chats = (List<ChatDto>)response;
            }

            else if (statusCode==HttpStatusCode.NoContent)
            {
                NavigationManager.NavigateTo("/counter");
            }

        }

        protected async Task SeeChat(Guid chatId)
        {

            var chat = Chats.First(c => c.Id == chatId);

            var userId = await GetUserId();

            var userChat = chat.UserChats?.First(u => u.UserId == userId);

            

            NavigationManager.NavigateTo($"/see-chat/{userChat.ToUserId}");

            NavigationManager.Refresh(true);
        }

        private async Task<Guid> GetUserId()
        {
            var stateProvider = (CustomAuthHandler)AuthenticationStateProvider;

            var state = await stateProvider.GetAuthenticationStateAsync();


            var user = state.User;

            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            return Guid.Parse(userId);
        }

    }
}
