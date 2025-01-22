using Chat.Client.Models.UserModels;
using System.Net;

namespace Chat.Client.Repositories.Contracts
{
    public interface IUserIntegration
    {
        Task<Tuple<HttpStatusCode, string>> Login(LoginUserModel loginUser);


        Task<Tuple<HttpStatusCode,string>> Register(CreateUserModel createUserModel);


        Task<Tuple<HttpStatusCode, object>> GetAllUsers();
    }
}
