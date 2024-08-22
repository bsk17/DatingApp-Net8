using System;
using System.ComponentModel.DataAnnotations;

namespace DatingAppServer.DTOs;

/// <summary>
/// RegisterDTO - To map the data being sent by user to the AppUser entity while registering.
/// </summary>
public class RegisterDTO
{
    /// <summary>
    /// The "= string.Empty" has been added to avoid warning. 
    /// And show proper messages when some error happens
    /// "public required string Username { get; set; }" can be alo be used.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; set; } = string.Empty;
}
