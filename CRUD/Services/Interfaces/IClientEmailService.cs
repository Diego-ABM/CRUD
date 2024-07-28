using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Services
{
    public interface IClientEmailService
    {
        Task<ResponseModel> CreateAsync(ClientEmailModel email);
        Task<ResponseModel> DeleteAsync(int id);
        Task<ResponseModel> ReadAsync(int idClient, string email = "");
        Task<ResponseModel> UpdateAsync(ClientEmailModel email);
    }
}