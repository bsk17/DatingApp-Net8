using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingAppServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppServer.Data;

/// <summary>
/// Seed.cs will help in generating Dummy users if not already present in DB.
/// This will only run the first time when program is build or executed.
/// Needs to be called from Program.cs
/// </summary>
public class Seed
{
    /// <summary>
    /// SeedUsers will take JSON from UserSeedData.json and convert them to List of AppUser.
    /// POpulate the smae in DB.
    /// </summary>
    /// <param name="context">DbContext throughout the app.</param>
    /// <returns>Null if users are already present in DB.</returns>
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
    }
}
