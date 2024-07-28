using CRUD.Models;

namespace CRUD.Validations.Interfaces
{
    public interface IAuthValidation
    {
        Task<ValidationModel> LoginAsync(LoginModel login);
    }
}