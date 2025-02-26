using System.Net;
using Blazored.LocalStorage;
using Chat.Blazor.Models.UserModels;
using Chat.Blazor.Repositories.Contracts;
using Microsoft.AspNetCore.Components;

namespace Chat.Blazor.Pages.AccountPages
{
    public class LoginBase:ComponentBase
    {
        [Inject]
        public IUserIntegration UserIntegration { get; set; }


        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject] 
        ILocalStorageService StorageService { get; set; }




        protected LoginUserModel LoginUserModel = new();


        protected async Task LoginClicked()
        {

            var (statusCode, response) =await UserIntegration.Login(LoginUserModel);


            if (statusCode==HttpStatusCode.OK)
            {

                await StorageService.SetItemAsStringAsync("token", response);

                NavigationManager.Refresh(true);

                
                NavigationManager.NavigateTo("/user-chats");
            }

            else if (statusCode==HttpStatusCode.BadRequest)
            {
                NavigationManager.NavigateTo($"/error/{response}");
            }
        }

    }
}
