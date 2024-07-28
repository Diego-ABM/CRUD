using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Validations.Interfaces
{
    public interface IClientContactValidation
    {
        Task<ValidationModel> CreateAsync(ClientContactModel contact);
        Task<ValidationModel> ReadOrDeleteAsync(int idClient);
        Task<ValidationModel> UpdateAsync(ClientContactModel contact);
    }
}