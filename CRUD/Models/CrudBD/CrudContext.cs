using CRUD.Models.CrudBD;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Models.bdCrud
{
    public class CrudContext : DbContext
    {
        // Constructor el cual isntancia la cadena de BD inidcada en la Inyeccion de dependencias
        public CrudContext(DbContextOptions<CrudContext> options) : base(options) { }

        // Crea intansias de las tablas creadas para manejarse con linq
        public DbSet<ClientModel> Cliente { get; set; }
        public DbSet<UserModel> Usuario { get; set; }
    }
}
