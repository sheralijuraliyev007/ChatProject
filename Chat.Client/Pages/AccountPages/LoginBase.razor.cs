using Chat.Client.Repositories.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net;
using Chat.Client.Models.UserModels;

namespace Chat.Client.Pages.AccountPages
{
    public class LoginBase() : ComponentBase
    {
        [Inject]
        public IUserIntegration UserIntegration { get; set; }

        [Inject]
        public NavigationManager NavManager { get; set; }

        protected LoginUserModel Model = new();


        protected string? Token { get; set; }

        protected async Task LoginClicked()
        {
            var (statusCode, response) = await UserIntegration.Login(Model);


            bool isOk = statusCode == HttpStatusCode.OK;
            bool isBadRequest = statusCode == HttpStatusCode.BadRequest;


            if (isOk)
            {
                NavManager.NavigateTo($"/login-successfully/{response}");
            }

            else if (isBadRequest)
            {
                NavManager.NavigateTo($"/error/{response}");
            }

        }
    }
}
