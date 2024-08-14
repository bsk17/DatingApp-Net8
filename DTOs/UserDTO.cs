using System;

namespace DatingAppServer.DTOs;

/// <summary>
/// UserDto will be returned when the user logs in successfully.
/// </summary>
public class UserDTO
{
    public required string Username { get; set; }
    public required string Token { get; set; }
}
