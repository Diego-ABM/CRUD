using CRUD.Models;
using CRUD.Models.bdCrud;

namespace CRUD.Services.Interfaces
{
    public interface IClientService
    {
        ResponseModel Create(ClientModel client);
        ResponseModel Read(string numberIdentification);
        ResponseModel Update(ClientModel cliente);
        ResponseModel Delete(string numberIdentification);
    }
}