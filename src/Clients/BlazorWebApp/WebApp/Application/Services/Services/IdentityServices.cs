using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Application.Services.Interfaces;
using WebApp.Extensions;

namespace WebApp.Application.Services.Services
{
    public class IdentityServices : IIdentityServices
    {
        private readonly HttpClient httpClient;
        private readonly ISyncLocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public IdentityServices(HttpClient httpClient, ISyncLocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
        }


        public bool IsLoggedIn => !string.IsNullOrEmpty(GetToken());
        public string GetToken()
        {
            return localStorage.GetToken();  
        }

        public string GetUsername()
        {
            return localStorage.GetUsername();
        }

        public bool IsAuthenticated()
        {
            return IsLoggedIn;
        }

        public Task<bool> Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        //public Task Logout()
        //{
        //    //httpClient.PostAsync
        //}

        public Task Register(string username, string password, string email)
        {
            throw new System.NotImplementedException();
        }
    }
}
