using CRUD.Services.Interfaces;

namespace CRUD.Services
{
    public class ConexionBD : IConexionBD
    {
        private readonly IConfiguration _configuration;

        public ConexionBD(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Connect()
        {
            string connectionString = _configuration.GetConnectionString("crud");

        }

    }
}
