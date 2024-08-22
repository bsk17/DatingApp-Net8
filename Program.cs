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

app.Run();
