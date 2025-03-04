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
        public required List<ChatDto>? Chats { get; set; }

        protected List<UserDto>? Users { get; set; } =new();

        [Inject] IChatIntegration ChatIntegration { get; set; }

        [Inject] IUserIntegration UserIntegration { get; set; }

        [Inject] public AuthenticationStateProvider StateProvider { get; set; }

        [Inject] StorageService StorageService { get; set; }

        private HubConnection? HubConnection { get; set; }

        protected UserDto User { get; set; }

        protected string Text { get; set; }
        

        protected bool IsSelected { get; set; }

        protected string? Username { get; set; }

        protected Guid? ChatId { get; set; }


        protected List<MessageDto>? Messages { get; set; } 

        protected ChatDto? ChatDto { get; set; }

        protected override async Task OnInitializedAsync()
        {


            var (statusCodeForChats, responseForChats) = await ChatIntegration.GetUserChats();

            if (statusCodeForChats == HttpStatusCode.OK)
            {
                Chats = (List<ChatDto>)responseForChats;
            }


            var (statusCode, response) = await UserIntegration.GetProfile();

            if (statusCode == HttpStatusCode.OK)
            {
                User = (UserDto)response;
                
            }

            GetChatNames();


            var (statusCode1, response1) = await UserIntegration.GetAllUsers();

            if (statusCode1 == HttpStatusCode.OK)
            {
                Users = (List<UserDto>)response1;
            }



            Username = await GetUserInfo();
 

            await SortContacts();



            //await DisconnectHub();
            //await ConnectToHub();


            if (ChatId is not null)
            {
                ChatDto = Chats.Single(c => c.Id == ChatId);
                Messages = ChatDto.Messages!;

            }

        }


        private async Task<string> GetUserInfo()
        {
            var stateProvider = (CustomAuthHandler)StateProvider;

            var state = await stateProvider.GetAuthenticationStateAsync();

            var user = state.User;

            var username = user.Claims.First(c => c.Type == ClaimTypes.Name).Value;

            return  username;
        }


        protected async Task SendMessage()
        {
            //#
            var (statusCode, response) = await ChatIntegration.SendTextMessage(ChatDto.Id, Text);

            if (statusCode == HttpStatusCode.OK)
            {
                
                Text = string.Empty;
            }
        }



        private void  GetChatNames()
        {
            var currentFullName = GetFullName(User.FirstName, User.LastName);
            foreach (var chat in Chats)
            {
                //chat.ChatName = GetChatName(chat.ChatNames!);

                chat.ChatName = chat.ChatNames?.First(c => c != currentFullName);
            }
            StateHasChanged();
        }

        private string GetChatName(List<string> chatNames)
        {
            var currentFullName = GetFullName(User.FirstName, User.LastName);

            var chatName = chatNames?.First(c => c != currentFullName);

            return chatName!;

        }

        private async Task ConnectToHub()
        {
            var token = await StorageService.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                HubConnection ??= new HubConnectionBuilder().WithUrl($"https://localhost:7156/chat-hub?token={token}").Build();
            }



            HubConnection?.On<MessageDto>("NewMessage", model =>
            {
                Messages = Messages ?? new();
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



        protected async Task Pressed(KeyboardEventArgs e)
        {
            if (e.Key=="Enter")
            {
                await SendMessage();
            }
        }


        protected static string GetFullName(string firstname, string? lastname)
        {
            if (string.IsNullOrEmpty(lastname))
            {
                return $"{firstname}";
            }

            return $"{firstname} {lastname}";

        }



        private async Task SortContacts()
        {

            var currentUser = Users?.SingleOrDefault(u => u.Username == Username);
            if (currentUser is not null)
                Users?.Remove(currentUser);

            foreach (var chat in Chats)
            {
                if (chat.UserChats is not null)
                {
                    var userChat = chat.UserChats[0];

                    //var userId1 = userChat.UserId;
                    
                    
                    //var userId2 = userChat.ToUserId;

                    //var user1 = Users?.SingleOrDefault(u => u.Id == userId1);

                    //if (user1 is not null)
                    //{
                    //    Users?.Remove(user1);
                    //}

                    //var user2 = Users?.SingleOrDefault(u => u.Id == userId2);

                    //if (user2 is not null)
                    //{
                    //    Users?.Remove(user2);
                    //}

                    var toUserId = userChat.ToUserId;
                    var toUser = Users?.SingleOrDefault(u => u.Id == toUserId);

                    if (toUser is not null)
                    {
                        Users?.Remove(toUser);
                    }

                }
            }
        }



        protected async Task CreateChat(Guid toUserId)
        {
            var (statusCode, response) = await ChatIntegration.GetUserChat(toUserId);

            if (statusCode == HttpStatusCode.OK)
            {
                ChatDto = (ChatDto)response;
                ChatDto.ChatName = GetChatName(ChatDto.ChatNames!);
                Messages = ChatDto.Messages!;

                var toUser = Users?.SingleOrDefault(u => u.Id == toUserId);

                if (toUser is not null)
                {
                    Users?.Remove(toUser);
                }
                Chats.Add(ChatDto);


                //await DisconnectHub();
                //await ConnectToHub();
            }
        }


    }
}
