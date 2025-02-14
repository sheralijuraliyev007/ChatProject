using Chat.Blazor.Repositories.Contracts;
using Microsoft.AspNetCore.Components;
using System.Net;
using Chat.Blazor.Models.UserModels;

namespace Chat.Blazor.Pages.AccountPages
{
    public class RegisterBase:ComponentBase
    {

        [Inject]
        public IUserIntegration UserIntegration { get; set; }

        [Inject]
        public NavigationManager NavigationManager{ get; set; }


        protected CreateUserModel CreateUserModel { get; set; }


        protected override void OnInitialized()
        {
            CreateUserModel=new CreateUserModel();
        }


        protected async Task RegisterClicked()
        {
            var (statusCode, response) = await UserIntegration.Register(CreateUserModel);

            if (statusCode == HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/account/login");
            }
            else if (statusCode == HttpStatusCode.BadRequest)
            {
                NavigationManager.NavigateTo($"/error/{response}");
            }

        }

    }
}
