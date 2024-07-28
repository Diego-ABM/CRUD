using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Validations.Interfaces
{
    public interface IClientEmailValidation
    {
        Task<ValidationModel> CreateAsync(ClientEmailModel clientEmail);
        Task<ValidationModel> ReadOrDeleteAsync(int idClient, string email = "");
        Task<ValidationModel> UpdateAsync(ClientEmailModel clientEmail);
    }
}