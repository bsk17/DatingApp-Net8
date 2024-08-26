using System.Text;
using DatingAppServer.Data;
using DatingAppServer.Extensions;
using DatingAppServer.Interfaces;
using DatingAppServer.Middleware;
using DatingAppServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

/*Add services to the container.*/

//Adding services from ApplicationServiceExtension Class
builder.Services.AddAplicationServices(builder.Configuration);

//Adding services from IdentityServiceExtensions Class
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

/*Configure the HTTP request pipeline.*/

//Adding Exceptioon Middleware on the top of request pipeline
app.UseMiddleware<ExceptionMiddleware>();

//Cross Origin Response Service utilisation
app.UseCors(options => options
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "https://localhost:4200")
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region Seeding
//Seeding process
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during Migration/Seeding!!");
}
#endregion

app.Run();
