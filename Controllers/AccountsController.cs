using System;
using System.Security.Cryptography;
using System.Text;
using DatingAppServer.Data;
using DatingAppServer.DTOs;
using DatingAppServer.Entities;
using DatingAppServer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppServer.Controllers;
/// <summary>
/// AccountsController will deal with all the account related routings
/// Function based Dependency Injection instead of Constructor based. 
/// </summary>
public class AccountsController(DataContext context, ITokenService tokenService) : BaseApiController
{
    /// <summary>
    /// To Register the user by encrypting the password
    /// API Endpoint = https://localhost:5001/api/accounts/register
    /// POST method
    /// </summary>
    /// <param name="registerDTO"> Method Body will contain username and password which will be mapped to RegisterDTO object </param>
    /// <returns> Newly created user in the form of userDTo which contains only username and token </returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
    {
        if (await UserExists(registerDTO.Username))
        {
            return BadRequest("Username already taken!!");
        }

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDTO.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserDTO
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    /// <summary>
    /// To sign in the user
    /// </summary>
    /// <param name="loginDTO"> Method Body will contain username and password which will be mapped to LoginDTO object </param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        /// This will return user if found otherwise a null value
        var user = await context.Users
        .FirstOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());

        //when user is null
        if (user == null)
        {
            return Unauthorized("Invalid Username!!");
        }

        //check for the password hash with the help of key which was saved as password salt
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            //match each character of the computed hash with the already saved password hash of user 
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid Password");
            }
        }

        return new UserDTO
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };
    }

    /// <summary>
    /// Method to check if username already exists
    /// </summary>
    /// <param name="username"> Username of User </param>
    /// <returns> True or False </returns>
    private async Task<bool> UserExists(string username)
    {
        return await context.Users.AnyAsync(user => user.UserName.ToLower() == username.ToLower());
    }
}
