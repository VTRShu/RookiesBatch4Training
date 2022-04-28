using EFCore_Ex2.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore_Ex2.Repositories.EFContext;

public class ProductStoreDBContext : DbContext
{   
    public ProductStoreDBContext(){}
    public ProductStoreDBContext(DbContextOptions options) : base(options) { }
    public DbSet<CategoryEntity> CategoryEntity { get; set; }
    public DbSet<ProductEntity> ProductEntity { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        modelBuilder.Entity<CategoryEntity>().HasMany(x=>x.Products).WithOne(x=>x.Category).OnDelete(DeleteBehavior.SetNull);
        ProductStoreSeedData.Seed(modelBuilder);
    }
}