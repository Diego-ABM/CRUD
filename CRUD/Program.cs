using CRUD.Models.bdCrud;
using CRUD.Services;
using CRUD.Services.Interfaces;
using CRUD.Validations;
using CRUD.Validations.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CRUD
{
    static class Program
    {
        static void Main()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();

            // Agrega los servicios requeridos
            AddServices(builder.Services);
            // Agrega las conexiones 
            AddDbContext(builder.Services, builder.Configuration.GetConnectionString("crud")!);

            // Agrega la autenticación JWT
            AddAutorizationBearerToken(builder);

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Necesario para utilizar JWT
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }

        static void AddServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Servicios
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IClientContactService, ClientContactService>();
            services.AddScoped<IClientAddressService, ClientAddressService>();
            services.AddScoped<IClientEmailService, ClientEmailService>();

            // Validaciones
            services.AddScoped<IAuthValidation, AuthValidation>();
            services.AddScoped<IClientValidation, ClientValidation>();
            services.AddScoped<IClientContactValidation, ClientContactValidation>();
            services.AddScoped<IClientAddressValidation, ClientAddressValidation>();
            services.AddScoped<IClientEmailValidation, ClientEmailValidation>();
            services.AddScoped<IUserValidation, UserValidation>();
        }
        static void AddDbContext(IServiceCollection services, string connectionString)
        {
            // Conexion BD
            services.AddDbContext<CrudContext>(optionsAction => optionsAction.UseSqlServer(connectionString));
        }

        static void AddAutorizationBearerToken(WebApplicationBuilder builder)
        {
            // Configurar la autenticación JWT
            builder.Services.AddAuthentication(options =>
            {
                // Establecer el esquema de autenticación predeterminado como JwtBearer
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Configurar opciones específicas para JWT Bearer
                options.RequireHttpsMetadata = false; // No requiere HTTPS (usar solo en desarrollo)
                options.SaveToken = true; // Guarda el token en el contexto de autenticación

                // Configurar los parámetros de validación del token
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Validar el emisor del token
                    ValidateAudience = true, // Validar la audiencia del token
                    ValidateLifetime = true, // Validar que el token no haya expirado
                    ValidateIssuerSigningKey = true, // Validar la clave de firma del emisor

                    // Especificar el emisor válido del token
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],

                    // Especificar la audiencia válida del token
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    // Configurar la clave de firma para validar la firma del token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

            // Agregar servicios de autorización a la aplicación
            builder.Services.AddAuthorization();
        }
    }
}
