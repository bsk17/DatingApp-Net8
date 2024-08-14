using System;
using DatingAppServer.Entities;

namespace DatingAppServer.Interfaces;

/// <summary>
/// Interface for TokenService
/// </summary>
public interface ITokenService
{
    string CreateToken(AppUser user);
}
