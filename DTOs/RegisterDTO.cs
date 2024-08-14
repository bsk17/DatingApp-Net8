using System;
using System.ComponentModel.DataAnnotations;

namespace DatingAppServer.DTOs;

/// <summary>
/// RegisterDTO - To map the data being sent by user to the AppUser entity while registering.
/// </summary>
public class RegisterDTO
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}
