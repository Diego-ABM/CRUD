using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Validations.Interfaces
{
    public interface IUserValidation
    {
        Task<ValidationModel> CreateAsync(UserModel user);
        Task<ValidationModel> ReadOrDeleteAsync(string email);
        Task<ValidationModel> UpdateAsync(UserModel user);
    }
}