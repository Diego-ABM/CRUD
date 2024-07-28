using CRUD.Models;
using CRUD.Models.bdCrud;

namespace CRUD.Validations.Interfaces
{
    public interface IClientValidation
    {
        Task<ValidationModel> CreateAsync(ClientModel client);
        Task<ValidationModel> ReadOrDeleteAsync(string identificationNumber);
        Task<ValidationModel> UpdateAsync(ClientModel client);
    }
}