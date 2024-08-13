using System;
using DatingAppServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingAppServer.Data;

/*
The Normal Constructor way ->
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
        
    }
}
*/

//The Primary Constructor way
//This contains less boiler plate compared to default one
public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
}
