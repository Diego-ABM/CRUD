using CRUD.Models;

namespace CRUD.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseModel> LoginAsync(LoginModel login);
    }
}