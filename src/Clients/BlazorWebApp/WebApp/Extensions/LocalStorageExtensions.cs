using Blazored.LocalStorage;
using System.Threading.Tasks;
using WebApp.Extensions.Encryption;

namespace WebApp.Extensions
{
    public static class LocalStorageExtensions
    {
        public static string GetUsername(this ISyncLocalStorageService localStorage)
        {
            return localStorage.GetItem<string>("username");
        }
        public static async Task<string> GetUsernameAsync(this ILocalStorageService localStorage)
        {
            return await localStorage.GetItemAsync<string>("username");
        }

        public static void SetUsername(this ISyncLocalStorageService localStorage, string username)
        {
            localStorage.SetItem("username", username);
        }

        public static async Task SetUsernameAsync(this ILocalStorageService localStorage, string username)
        {
            await localStorage.SetItemAsync("username", username);
        }

        public static void RemoveUsername(this ISyncLocalStorageService localStorage)
        {
            localStorage.RemoveItem("username");
        }

        public static async Task RemoveUsernameAsync(this ILocalStorageService localStorage)
        {
            await localStorage.RemoveItemAsync("username");
        }

        public static string GetToken(this ISyncLocalStorageService localStorage)
        {
            var token = localStorage.GetItem<string>("token");
            return Cryption.GetTextFromBase64(token);
        }

        public static async Task<string> GetTokenAsync(this ILocalStorageService localStorage)
        {
            var token = await localStorage.GetItemAsync<string>("token");
            return Cryption.GetTextFromBase64(token);
        }

        public static async Task SetTokenAsync(ILocalStorageService localStorage,string token)
        {
            var crypted = Cryption.GetBase64String(token);
            await localStorage.SetItemAsync<string>("token", crypted);
        }

        public static void SetToken(ISyncLocalStorageService localStorage, string token)
        {
            var crypted = Cryption.GetBase64String(token);
            localStorage.SetItem<string>("token", crypted);
        }

        public static void RemoveToken(this ISyncLocalStorageService localStorage)
        {
            localStorage.RemoveItem("token");
        }

        public static async Task RemoveTokenAsync(this ILocalStorageService localStorage)
        {
            await localStorage.RemoveItemAsync("token");
        } 
        public static void Clear(this ISyncLocalStorageService localStorage)
        {
            localStorage.Clear();
        }
        
        

    }
}
