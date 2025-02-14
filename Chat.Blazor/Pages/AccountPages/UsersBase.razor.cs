using System.Net;
using Blazored.LocalStorage;
using Chat.Blazor.DTOs;
using Chat.Blazor.Repositories.Contracts;
using Microsoft.AspNetCore.Components;

namespace Chat.Blazor.Pages.AccountPages
{
    public class UsersBase:ComponentBase
    {
        [Inject]
        public IUserIntegration UserIntegration { get; set; }


        [Inject]
        public NavigationManager NavigationManager { get; set; }



        protected List<UserDto>? Users = new ();


        protected override async Task OnInitializedAsync()
        {
            await GetAllUsers();
        }

        protected async Task GetAllUsers()
        {
            var (statusCode, response) = await UserIntegration.GetAllUsers();

            if (statusCode==HttpStatusCode.OK)
            {
                
                Users=(List<UserDto>)response;
            }

            else if (statusCode==HttpStatusCode.BadRequest || statusCode==HttpStatusCode.Unauthorized)
            {
                var errorMessage=(string) response;

                NavigationManager.NavigateTo($"/error/{response}");
            }
        }
    }
}
