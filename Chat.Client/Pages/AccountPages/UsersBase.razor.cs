using System.Net;
using Chat.Client.DTOs;
using Chat.Client.Repositories.Contracts;
using Microsoft.AspNetCore.Components;

namespace Chat.Client.Pages.AccountPages
{
    public class UsersBase:ComponentBase
    {
        [Inject]
        public IUserIntegration UserIntegration { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected List<UserDto>? Users { get; set; }

        protected async Task GetAllUsers()
        {
            var (statusCode, response) = await UserIntegration.GetAllUsers();

            if (statusCode==HttpStatusCode.OK)
            {
                Users = (List<UserDto>)response;
            }

            else if (statusCode is HttpStatusCode.BadRequest or 
                     HttpStatusCode.Unauthorized)
            {
                var errorMessage = (string)response;
                NavigationManager.NavigateTo($" /error/{response}");
            }

        }
    }
}
