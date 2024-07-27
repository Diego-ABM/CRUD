using CRUD.Controllers.Interfaces;
using CRUD.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Client : ControllerBase, IClient
    {

        private readonly IConexionBD _conexionBD;

        public Client(IConexionBD conexionBD)
        {
            _conexionBD = conexionBD;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public void Create()
        {
            _conexionBD.Connect();
        }

    }
}
