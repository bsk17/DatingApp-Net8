using System;
using System.Security.Claims;
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
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
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


    /// <summary>
    /// UpdateUser will help in persisting the changes made from angular side
    /// since we are sending the token along with the request from angular side
    /// therefore we can use the ClaimTypes.NameIdentifier to get the username 
    /// as it has been set in TokenService.cs (line 26 & 27)
    /// </summary>
    /// <param name="memberUpdateDTO">parameter is sent as json body from angular side</param>
    /// <returns></returns>
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (username == null)
        {
            return BadRequest("No username foudn in token!!");
        }

        var user = await userRepository.GetUserByNameAsync(username);
        if (user == null)
        {
            return BadRequest("Could not find user!!");
        }

        //make sure that the details from memberUpdateDTO is mapped into the user object that is just received from DB
        //NOTE:- This can also be done manually by individually assigning each values one by one
        mapper.Map(memberUpdateDTO, user);

        if (await userRepository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Failed to update the User!!!");
    }
}
