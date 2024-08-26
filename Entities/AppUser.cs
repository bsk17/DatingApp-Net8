using System;
using DatingAppServer.Extensions;

namespace DatingAppServer.Entities;

/// <summary>
/// AppUser class represents User Entity
/// </summary>
public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public DateOnly DateofBirth { get; set; }
    public required string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }

    //Navigation Property
    public List<Photo> Photos { get; set; } = [];

    /// <summary>
    /// DateOfBirth is an object of DateOnly.
    /// We have created an extension method of DateOnly in DateTimeExtensions.cs.
    /// Hence CalculateAge can be used directly as normal method anywhere.
    /// It is named as "GetAge" to match with the property "Age" in "MemebrDTO" - Automapper will assign the return value from this function to the Age property of MemberDTO
    /// </summary>
    /// <returns>integer Age</returns>

    /*
    public int GetAge()
    {
        return DateofBirth.CalculateAge(); // REMOVED TO AUTOMAPPER PROFILE
    }
    */
}
