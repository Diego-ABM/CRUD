using CRUD.Models.bdCrud;
using CRUD.Services;
using CRUD.Services.Interfaces;
using CRUD.Validations;
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

            AddServices(builder.Services);
            AddDbContext(builder.Services, builder.Configuration.GetConnectionString("crud")!);

            // Configurar la autenticación JWT
            AddAutorizationBearerToken(builder);


            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

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

            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IClientContactService, ClientContactService>();
            services.AddScoped<IClientAddressService, ClientAddressService>();
            services.AddScoped<IClientEmailService, ClientEmailService>();


            services.AddScoped<ClientValidation>();
            services.AddScoped<ClientContactValidation>();
            services.AddScoped<ClientAddressValidation>();
            services.AddScoped<ClientEmailValidation>();
            services.AddScoped<UserValidation>();
            services.AddScoped<AuthValidation>();
        }
        static void AddDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<CrudContext>(optionsAction => optionsAction.UseSqlServer(connectionString));
        }

        static void AddAutorizationBearerToken(WebApplicationBuilder builder)
        {
            // Configurar la autenticación JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });

            builder.Services.AddAuthorization();

        }

    }

}
