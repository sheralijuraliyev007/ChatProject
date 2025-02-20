using System.Net;
using Chat.Blazor.DTOs;
using Chat.Blazor.Repositories.Contracts;
using Chat.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Blazor.Pages.ChatPages
{
    public abstract class SeeChatBase:ComponentBase
    {
        [Inject]
        IChatIntegration ChatIntegration { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        StorageService StorageService { get; set; }


        protected List<MessageDto> Messages = new();

        protected ChatDto ChatDto { get; set; }



        protected string Text { get; set; } 

        [Parameter] public Guid ToUserId { get; set; }


        private HubConnection? HubConnection { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await DisconnectHub();
            await ConnectToHub();

            var (statusCode, response) = await ChatIntegration.GetUserChat(toUserId:ToUserId);

            if (statusCode==HttpStatusCode.OK)
            {
                ChatDto = (ChatDto)response;
            }

            var (statusCode2, response2) = await ChatIntegration.GetChatMessages(ChatDto.Id);

            if (statusCode2 == HttpStatusCode.OK)
            {
                Messages= (List<MessageDto>)response2;
            }


        }


        protected async Task SendMessage()
        {
            var (statusCode, response) = await ChatIntegration.SendTextMessage(ChatDto.Id, Text);

            if (statusCode == HttpStatusCode.OK)
            {
                Text = string.Empty;

                //NavigationManager.Refresh(true);
            }
        }

        private async Task ConnectToHub()
        {
            var token = await StorageService.GetToken();

            if (!string.IsNullOrEmpty(token))
            {

                if (HubConnection==null)
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


        private async Task DisconnectHub()
        {
            if (HubConnection is not null)
            {
                await HubConnection.StopAsync();
            }
        }
    }
}
