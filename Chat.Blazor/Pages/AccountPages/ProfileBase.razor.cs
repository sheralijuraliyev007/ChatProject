
using System.Net;
using Chat.Blazor.Constants;
using Chat.Blazor.DTOs;
using Chat.Blazor.Repositories.Contracts;
using Microsoft.AspNetCore.Components;

namespace Chat.Blazor.Pages.AccountPages
{
    public class ProfileBase:ComponentBase
    {
        [Inject] 
        private IUserIntegration UserIntegration { get; set; }

        [Inject]

        private NavigationManager NavigationManager { get; set; }

        protected string ImgUrl { get; set; }

        protected byte Age { get; set; }

        protected string Bio { get; set; }


        protected UserDto? User { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var (statusCode, response) = await UserIntegration.GetProfile();


            if (statusCode==HttpStatusCode.OK)
            {
                User =(UserDto) response;

                if (User.PhotoData!=null)
                {
                    ImgUrl = $"data:image/jpeg;base64,{Convert.ToBase64String(User.PhotoData)}";
                }

                else
                {
                    if (User.Gender.ToLower()=="male")
                    {
                        ImgUrl = UrlConstants.DefaultManImageUrl;
                    }
                    else
                    {
                        ImgUrl=UrlConstants.DefaultWomanImageUrl;
                    }
                }
            }

            else
            {
                var error = (string)response;
            }
        }


        protected async Task UpdateAge()
        {
            var (statusCode, response) = await UserIntegration.UpdateAge(Age);

            NavigationManager.Refresh(forceReload:true);
        }

        protected async Task UpdateBio()
        {

            var (statusCode, response)=await UserIntegration.UpdateBio(Bio);

            NavigationManager.Refresh(forceReload:true);
        }
    }
}
