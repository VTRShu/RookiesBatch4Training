using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EFCore_Ex2.Mappings;
using EFCore_Ex2.Repositories.EFContext;
using EFCore_Ex2.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace EFCoreTest;
public class ShareSetupTest 
{
    private ProductStoreDBContext? _productStoreDbContextIM;
    private DbContextOptions<ProductStoreDBContext>? _dbContextOptions;
    private IMapper? _mapper;
    private List<CategoryEntity>? _categoryListTest;
    private List<ProductEntity> _productListTest;

    public ProductStoreDBContext InMemoryDatabaseSetupTest()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ProductStoreDBContext>()
        .UseInMemoryDatabase("ProductStore")
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        _productStoreDbContextIM = new ProductStoreDBContext(_dbContextOptions);
        _productStoreDbContextIM.Database.EnsureCreated();
        _productStoreDbContextIM.Database.EnsureDeleted();
        SeedInMemoryDatabase();

        return _productStoreDbContextIM;
    }
    private void SeedInMemoryDatabase()
    {
        _categoryListTest = new List<CategoryEntity>(){
            new CategoryEntity(){CategoryId = Guid.NewGuid() , CategoryName = "Laptop"},
            new CategoryEntity(){CategoryId = Guid.NewGuid() , CategoryName = "Bike"}
        };
        _productStoreDbContextIM.CategoryEntity.AddRange(_categoryListTest);
        _productListTest = new List<ProductEntity>(){
            new ProductEntity(){ProductId = Guid.NewGuid(),ProductName = "Dell Alienware X17",Manufacture="Dell",CategoryId =_categoryListTest[0].CategoryId},
            new ProductEntity(){ProductId = Guid.NewGuid(),ProductName = "CB150R",Manufacture="Honda",CategoryId =_categoryListTest[1].CategoryId},
            new ProductEntity(){ProductId = Guid.NewGuid(),ProductName = "R15",Manufacture="Yamaha",CategoryId =_categoryListTest[1].CategoryId},
        };
        _productStoreDbContextIM.ProductEntity.AddRange(_productListTest);
        _productStoreDbContextIM.SaveChanges();
    } 
    public IMapper MappingForTesting()
    {
         var mapperConfiguration = new MapperConfiguration(
            cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = mapperConfiguration.CreateMapper();

        return _mapper;
    }                                                                                   
    private IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

}