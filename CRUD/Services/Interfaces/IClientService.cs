using CRUD.Models;
using CRUD.Models.bdCrud;

namespace CRUD.Services.Interfaces
{
    public interface IClientService
    {
        ResponseControllerModel Create(ClientModel client);
        ResponseControllerModel Read(string numberIdentification);
        ResponseControllerModel Update(ClientModel cliente);
        ResponseControllerModel Delete(string numberIdentification);
    }
}