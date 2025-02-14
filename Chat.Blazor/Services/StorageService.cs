using Blazored.LocalStorage;

namespace Chat.Blazor.Services
{
    public class StorageService(ILocalStorageService localStorageService)
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;

        private const string Key = "token";

        public async Task SetToken(string token)
        {
            await  localStorageService.SetItemAsStringAsync(Key, token);
        }

        public async Task<string?> GetToken()
        {
            var token=await _localStorageService.GetItemAsync<string>(Key);


            return token;
        }

        public async Task DeleteToken()
        {
            await _localStorageService.RemoveItemAsync(Key);
        }
    }
}
