using System.Net;
using System.Security.Claims;
using Chat.Blazor.DTOs;
using Chat.Blazor.Repositories.Contracts;
using Chat.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using static System.Net.Mime.MediaTypeNames;

namespace Chat.Blazor.Pages.ChatPages
{
    public class ChattingPageBase : ComponentBase
    {
        [Parameter] public required List<ChatDto> Chats { get; set; }

        [Inject] IChatIntegration ChatIntegration { get; set; }

        [Inject] public AuthenticationStateProvider StateProvider { get; set; }

        [Inject] StorageService StorageService { get; set; }

        private HubConnection? HubConnection { get; set; }


        protected string Text { get; set; }

        protected bool IsSelected { get; set; }

    protected string? Username { get; set; }

        protected Guid? ChatId { get; set; }


        protected List<MessageDto> Messages { get; set; } = new();

        protected ChatDto? ChatDto { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await DisconnectHub();
            await ConnectToHub();


            if (ChatId is not null)
            {
                ChatDto = Chats.Single(c => c.Id == ChatId);

                Messages = ChatDto.Messages;

            }



            Username = await GetUserInfo();
            
        }


        private async Task<string> GetUserInfo()
        {
            var stateProvider = (CustomAuthHandler)StateProvider;

            var state = await stateProvider.GetAuthenticationStateAsync();

            var user = state.User;

            var username = user.Claims.First(c => c.Type == ClaimTypes.Name).Value;

            return username;
        }


        protected async Task SendMessage()
        {
            var (statusCode, response) = await ChatIntegration.SendTextMessage(ChatDto.Id, Text);

            if (statusCode == HttpStatusCode.OK)
            {
                Text = string.Empty;
            }
        }

        private async Task ConnectToHub()
        {
            var token = await StorageService.GetToken();

            if (!string.IsNullOrEmpty(token))
            {

                if (HubConnection == null)
                {

                    HubConnection = new HubConnectionBuilder().
                        WithUrl($"https://localhost:7156/chat-hub?token={token}").Build();
                } 

            }



            HubConnection?.On<MessageDto>("NewMessage", model =>
            {
                Messages.Add(model);
                StateHasChanged();
            });

            await HubConnection.StartAsync();
        }


        protected async Task SelectChat(Guid chatId, bool isSelected = false)
        {
            
            
            ChatId=chatId;


            ChatDto = Chats.Single(c => c.Id == chatId);

            Messages = ChatDto.Messages;
            if (chatId!=Guid.Empty)
            {
                isSelected = true;
            }
            
            IsSelected = isSelected;

            StateHasChanged();
        }


        private async Task DisconnectHub()
        {
            if (HubConnection is not null)
            {
                await HubConnection.StopAsync();
            }
        }

        protected async Task Pressed(KeyboardEventArgs e)
        {
            if (e.Key=="Enter")
            {
                await SendMessage();
            }
        }


    }
}
