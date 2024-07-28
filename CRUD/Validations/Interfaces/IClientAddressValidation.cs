using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Validations.Interfaces
{
    public interface IClientAddressValidation
    {
        Task<ValidationModel> CreateAsync(ClientAddressModel clientAddress);
        Task<ValidationModel> ReadOrDeleteAsync(int id);
        Task<ValidationModel> UpdateAsync(ClientAddressModel clientAddress);
    }
}