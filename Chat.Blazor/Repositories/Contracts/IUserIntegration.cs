
using System.Net;
using Chat.Blazor.Models.UserModels;

namespace Chat.Blazor.Repositories.Contracts
{
    public interface IUserIntegration
    {
        Task<Tuple<HttpStatusCode, string>> Login(LoginUserModel loginUser);


        Task<Tuple<HttpStatusCode,string>> Register(CreateUserModel createUserModel);


        Task<Tuple<HttpStatusCode, object>> GetAllUsers();


        Task<Tuple<HttpStatusCode, object>> GetProfile();

        Task<Tuple<HttpStatusCode, object?>> UpdateAge(byte age);

        Task<Tuple<HttpStatusCode,object?>> UpdateBio(string  bio);
    }
}
