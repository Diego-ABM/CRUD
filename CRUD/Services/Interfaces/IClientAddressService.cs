using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Services
{
    public interface IClientAddressService
    {
        Task<ResponseModel> CreateAsync(ClientAddressModel address);
        Task<ResponseModel> DeleteAsync(int id);
        Task<ResponseModel> ReadAsync(int idClient);
        Task<ResponseModel> UpdateAsync(ClientAddressModel clientAddress);
    }
}