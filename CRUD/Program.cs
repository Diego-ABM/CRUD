using CRUD.Models.bdCrud;
using CRUD.Services;
using CRUD.Services.Interfaces;
using CRUD.Validations;
using Microsoft.EntityFrameworkCore;

namespace CRUD
{

    static class Program
    {

        static void Main()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            AddServices(builder.Services);
            AddDbContext(builder.Services, builder.Configuration.GetConnectionString("crud")!);

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }

        static void AddServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<UserService>();

            services.AddScoped<ClientValidation>();
            services.AddScoped<UserValidation>();
        }

        static void AddDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CrudContext>(optionsAction => optionsAction.UseSqlServer(connectionString));
        }

    }

}
