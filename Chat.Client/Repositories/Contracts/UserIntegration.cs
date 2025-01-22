using System.Net;
using System.Net.Http.Json;
using Chat.Client.DTOs;
using Chat.Client.Models.UserModels;

namespace Chat.Client.Repositories.Contracts
{
    public class UserIntegration(HttpClient httpClient) : IUserIntegration
    {
        public async Task<Tuple<HttpStatusCode, string>> Login(LoginUserModel loginUser)
        {
            string url = "/api/Users/login";

            var r = await httpClient.GetAsync("/api/profile");
            
           var baseUri = httpClient.BaseAddress;
            var result = await httpClient.PostAsJsonAsync(url, loginUser);
            
            Console.WriteLine( result.StatusCode);

            var statusCode = result.StatusCode;

            var response = await result.Content.ReadAsStringAsync();

            Console.WriteLine( response);
            return new(statusCode, response);
        }

        public async Task<Tuple<HttpStatusCode, string>> Register(CreateUserModel createUserModel)
        {
            string url = "/api/users/register";

            var result = await httpClient.PostAsJsonAsync(url, createUserModel);

            var statusCode = result.StatusCode;

            var response= await result.Content.ReadAsStringAsync();

            return new(statusCode, response);
        }

        public async Task<Tuple<HttpStatusCode, object>> GetAllUsers()
        {
            string url = "api/users";


            var result = await httpClient.GetAsync(url);


            var statusCode= result.StatusCode;

            if (statusCode==HttpStatusCode.OK)
            {
                var users = await result.Content.ReadFromJsonAsync<List<UserDto>>();

                return (new(statusCode, users!));
            }


            else if (statusCode == HttpStatusCode.BadRequest)
            {
                var response= await result.Content.ReadAsStringAsync();
                return new(statusCode, response);
            }

            else if (statusCode==HttpStatusCode.Unauthorized)
            {
                return new(statusCode, "Unauthorized");
            }

            return new(statusCode, "Something went wrong?!!?");


        }
    }
}
