using System;

namespace DatingAppServer.DTOs;

/// <summary>
/// LoginDTO - To map the data being sent by user to the AppUser entity while login.
/// </summary>
public class LoginDTO
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
