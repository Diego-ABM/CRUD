using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Services.Interfaces
{
    public interface IClientContactService
    {
        Task<ResponseModel> CreateAsync(ClientContactModel contact);
        Task<ResponseModel> DeleteAsync(int idClient);
        Task<ResponseModel> ReadAsync(int idClient);
        Task<ResponseModel> UpdateAsync(ClientContactModel contact);
    }
}