using CRUD.Models.CrudBD;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Models.bdCrud
{
    public class CrudContext : DbContext
    {
        // Constructor el cual isntancia la cadena de BD inidcada en la Inyeccion de dependencias
        public CrudContext(DbContextOptions<CrudContext> options) : base(options) { }

        // Crea intansias de las tablas creadas para manejarse con linq
        public DbSet<UserModel> Usuario { get; set; }
        public DbSet<ClientModel> Cliente { get; set; }
        public DbSet<ClientAddressModel> ClienteDireccion { get; set; }
        public DbSet<ClientContactModel> ClienteContacto { get; set; }
        public DbSet<ClientEmailModel> ClienteCorreoElectronico { get; set; }

    }
}
