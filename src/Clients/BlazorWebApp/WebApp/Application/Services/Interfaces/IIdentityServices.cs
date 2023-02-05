using System.Threading.Tasks;

namespace WebApp.Application.Services.Interfaces
{
    public interface IIdentityServices
    {
        string GetUsername();
        string GetToken();

        bool IsAuthenticated();
        Task<bool> Login(string username, string password); 
        Task Logout();
        Task Register(string username, string password, string email); 
    }
}
