using System;
using DatingAppServer.Data;
using DatingAppServer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppServer.Controllers;

/// <summary>
/// UsersController uses the properties of BaseApiController created manually.
/// The route to this controller will be /api/users which is defined in BaseApiController
/// </summary>

public class UsersController(DataContext context) : BaseApiController
{
    [AllowAnonymous] // By default allow anonymous is set, But in case at top top level if it is set as Authorize then we can specifically use allow anonymous to overrider that.
    [HttpGet] // /api/users
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return users; //similar to OK(users)
    }

    [Authorize]
    [Authorize]
    [HttpGet("{id:int}")] // /api/users/3
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }
}
