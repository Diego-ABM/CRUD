using Microsoft.EntityFrameworkCore;

namespace CRUD.Models.bdCrud
{
    public class CrudContext : DbContext
    {
        // Constructor el cual isntancia la cadena de BD inidcada en la Inyeccion de dependencias
        public CrudContext(DbContextOptions<CrudContext> options) : base(options) { }
        public DbSet<ClientModel> Cliente { get; set; }
    }
}
