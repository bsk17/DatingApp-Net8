using DatingAppServer.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*Add services to the container.*/

builder.Services.AddControllers();

//Add EntityFramework DataContext as DB Context adn use the default connection string.
//GetConnectionString will search for "ConnectionStrings" Key in appsettings.Json/appsettings.Development.json
//inside ConnectionStrings then we need to add DefaultConnection as key for our connection string value
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Add CORS policy so that other ports can access this server
builder.Services.AddCors();

var app = builder.Build();

/*Configure the HTTP request pipeline.*/
app.UseCors(options => options
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "https://localhost:4200")
);

app.MapControllers();

app.Run();
