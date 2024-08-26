using System;

namespace DatingAppServer.Extensions;

/// <summary>
/// Provides an extension method for DateOnly class which can be used anywhere, 
/// hence static in nature.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// CalculateAge is an Extesnion Method of DateOnly class, hence static in nature.
    /// DateOnly.CalculateAge();
    /// </summary>
    /// <param name="dob">Attribute of AppUser Entity</param>
    /// <returns>The Age of the AppUser</returns>
    public static int CalculateAge(this DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var age = today.Year - dob.Year;
        if (dob > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }
}
