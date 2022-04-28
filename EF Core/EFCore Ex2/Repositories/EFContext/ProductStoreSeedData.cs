using EFCore_Ex2.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore_Ex2.Repositories.EFContext;
public static class ProductStoreSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>().HasData(
            new ProductEntity()
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Alienware X17",
                Manufacture = "Dell",
                CategoryId =  new Guid("df44bc42-6785-459b-8185-4d7f7c16b544"),
            },
            new ProductEntity()
            {
                ProductId = Guid.NewGuid(),
                ProductName = "Alienware 15R6",
                Manufacture = "Dell",
                CategoryId =  new Guid("df44bc42-6785-459b-8185-4d7f7c16b544"),
            },
            new ProductEntity()
            {
                ProductId = Guid.NewGuid(),
                ProductName = "MSI modern 14 B11",
                Manufacture = "MSI",
                CategoryId =  new Guid("df44bc42-6785-459b-8185-4d7f7c16b544"),
            }
        );
        modelBuilder.Entity<CategoryEntity>().HasData(
            new CategoryEntity
            {
                CategoryId = new Guid("df44bc42-6785-459b-8185-4d7f7c16b544"),
                CategoryName = "Laptop",
            });
    }
}