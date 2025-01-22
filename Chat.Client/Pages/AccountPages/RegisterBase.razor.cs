using System.Net;
using Chat.Client.Models.UserModels;
using Chat.Client.Repositories.Contracts;
using Microsoft.AspNetCore.Components;

namespace Chat.Client.Pages.AccountPages
{
    public class RegisterBase:ComponentBase
    {

        [Inject]
        public IUserIntegration UserIntegration { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected CreateUserModel Model=new ();

        protected async Task RegisterClicked()
        {
            var (statusCode, response) =await UserIntegration.Register(Model);

            if (statusCode==HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo($"/login-successfully/{response}");
            }
            else if (statusCode==HttpStatusCode.BadRequest)
            {
                NavigationManager.NavigateTo($"/error/{response}");
            }


        }

    }
}
