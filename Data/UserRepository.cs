using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingAppServer.DTOs;
using DatingAppServer.Entities;
using DatingAppServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppServer.Data;

/// <summary>
/// Respository Class for user related functions
/// </summary>
/// <param name="context">Database Context</param>
public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    /// <summary>
    /// Instead of directly getting the User, We are going to get the MemberDTO.
    /// By Projecting
    /// </summary>
    /// <param name="username">user name</param>
    /// <returns></returns>
    public async Task<MemberDTO?> GetMemberAsync(string username)
    {
        return await context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    /// <summary>
    /// Instead of directly getting the User, We are going to get the MemberDTO.
    /// By Projecting 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
    {
        return await context.Users
            .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
            .ToListAsync();
    }

    /// <summary>
    /// Methos won't be used further
    /// </summary>
    /// <param name="id">The user id if we know that</param>
    /// <returns></returns>
    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByNameAsync(string username)
    {
        return await context.Users
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
            .Include(x => x.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
