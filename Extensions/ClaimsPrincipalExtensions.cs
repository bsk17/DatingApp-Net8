using System.Security.Claims;

namespace DatingAppServer.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Creating an extension method of ClaimsPrincipal class that's why "this" is used
    /// it can be used as "User.GetUsername()" where User is of type ClaimsPrincipal
    /// </summary>
    /// <param name="user">Object of ClaimsPrincipal</param>
    /// <returns>username found in claims</returns>
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Cannot get username from token");
        return username;
    }
}