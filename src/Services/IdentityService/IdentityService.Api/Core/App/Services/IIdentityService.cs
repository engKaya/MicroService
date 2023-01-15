using IdentityService.Api.Core.App.Models;
using System.Threading.Tasks;

namespace IdentityService.Api.Core.App.Services
{
    public interface IIdentityService
    {
        Task<LoginResponseModel> Login(LoginRequestModels login);
    }
}
