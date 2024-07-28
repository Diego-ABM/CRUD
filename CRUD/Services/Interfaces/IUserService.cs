using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Services
{
    public interface IUserService
    {
        Task<ResponseModel> CreateAsync(UserModel user);
        Task<ResponseModel> DeleteAsync(string email);
        Task<ResponseModel> ReadAsync(string email);
        Task<ResponseModel> UpdateAsync(UserModel user);
    }
}