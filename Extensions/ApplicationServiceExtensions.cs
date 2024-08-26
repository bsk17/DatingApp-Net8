using System;
using DatingAppServer.Data;
using DatingAppServer.Interfaces;
using DatingAppServer.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingAppServer.Extensions;

/// <summary>
/// To extend the services from here instead of Program.cs
/// All the services.<MethodName> used here can be directly used in Program.cs but that would make things messier hence to keep it clean and seggregrated divide the services and extend them.
/// </summary>
public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        //Add EntityFramework DataContext as DB Context adn use the default connection string.
        //GetConnectionString will search for "ConnectionStrings" Key in appsettings.Json/appsettings.Development.json
        //inside ConnectionStrings then we need to add DefaultConnection as key for our connection string value
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        //Add CORS policy so that other ports can access this server
        services.AddCors();

        //Add manually created services
        services.AddScoped<ITokenService, TokenService>();

        //Add Repository services
        services.AddScoped<IUserRepository, UserRepository>();

        //Add AutoMapper Service
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
