using CRUD.Models;
using CRUD.Models.bdCrud;

namespace CRUD.Services.Interfaces
{
    public interface IClientService
    {
        Task<ResponseModel> CreateAsync(ClientModel cliente);
        Task<ResponseModel> DeleteAsync(string numberIdentification);
        Task<ResponseModel> ReadAsync(string numberIdentification);
        Task<ResponseModel> UpdateAsync(ClientModel cliente);
    }
}