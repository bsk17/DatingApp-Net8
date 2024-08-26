using System;
using AutoMapper;
using DatingAppServer.Data;
using DatingAppServer.DTOs;
using DatingAppServer.Entities;
using DatingAppServer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppServer.Controllers;

/// <summary>
/// UsersController uses the properties of BaseApiController created manually.
/// The route to this controller will be /api/users which is defined in BaseApiController
/// </summary>

[Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
{
    /*
    [AllowAnonymous] // By default allow anonymous is set, But in case at top top level if it is set as Authorize then we can specifically use allow anonymous to overrider that.
    */
    [HttpGet] // /api/users
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")] // /api/users/lisa
    public async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}
