using CRUD.Controllers;
using CRUD.Controllers.Interfaces;
using CRUD.Services;
using CRUD.Services.Interfaces;

namespace CRUD
{

    static class Program
    {

        static void Main()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            AddServices(builder.Services);

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

            services.AddScoped<IConexionBD, ConexionBD>();
            services.AddScoped<IClient, Client>();

        }

    }

}
