using EFCore_Ex.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore_Ex.Repositories.EFContext;

public class RookieDBContext : DbContext
{  
    public RookieDBContext(DbContextOptions options):base(options){}
    public DbSet<RookieEntity> Rookies {get;set;}
}