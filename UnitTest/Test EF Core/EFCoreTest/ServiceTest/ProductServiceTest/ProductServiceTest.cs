using EFCore_Ex2.Services;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using EFCore_Ex2.Repositories.EFContext;
using AutoMapper;
using System.Collections.Generic;
using EFCore_Ex2.Repositories.DTO;
using EFCore_Ex2.Services.Implements;
using Microsoft.Extensions.Logging;
using EFCore_Ex2.Repositories.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;
using EFCore_Ex2.Mappings;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCoreTest;

public class ProductServiceTest
{   
    private ProductStoreDBContext? _productStoreDbContextIM;
    private DbContextOptions<ProductStoreDBContext>? _dbContextOptions;
    private IMapper? _mapper;
    private ShareSetupTest _shareSetupTest;
    private IProductService? _productService;
    private Mock<ILogger<ProductService>> _mockLogger;
    private List<ProductEntity>? _productListTest;
     private List<CategoryEntity>? _categoryListTest;
    [SetUp]
    public void SetUp()
    {   
        _mockLogger = new Mock<ILogger<ProductService>>();
        _shareSetupTest= new ShareSetupTest();
        _mapper = _shareSetupTest.MappingForTesting();
        _productStoreDbContextIM = _shareSetupTest.InMemoryDatabaseSetupTest();
        _productService = new ProductService(_productStoreDbContextIM,_mockLogger.Object,_mapper);
        _productListTest = _productStoreDbContextIM.ProductEntity.ToList();
        _categoryListTest = _productStoreDbContextIM.CategoryEntity.ToList();
    }

    public void VerifyLogger(string message)
    {
        _mockLogger.Verify(
        m => m.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString().Contains(message)),
            null,
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        Times.Once);
    }
    [Test]
    public async Task Test_ProductService_GetProductList_ReturnProductDTOList()
    {
        //Arrange
        var _productListDto = _productStoreDbContextIM.ProductEntity.Select(x=>_mapper.Map<ProductDTO>(x)).ToList();
        //Act
        var result = await _productService.GetListProductAsync();
   
        Assert.AreEqual(3, result.Count());
        Assert.AreEqual(_productListDto[0].ProductId, result[0].ProductId);
    }
    [Test]
    public async Task Test_ProductService_CreateProduct_ReturnASuccessProductDTO_And_SaveInDatabase_WhenInputAllValid()
    {
        var productDto = new ProductDTO(){
            ProductId = Guid.NewGuid(),
            ProductName = "Civic",
            Manufacture = "Honda",
            CategoryId = _categoryListTest[0].CategoryId,
        };

        var result = await _productService.CreateProductAsync(productDto);
        var productInDatabase = _productStoreDbContextIM.ProductEntity.FirstOrDefault(x=>x.ProductId == productDto.ProductId);

        Assert.AreEqual(productDto.ProductId, result.ProductId);
        Assert.AreEqual(productDto.ProductId,productInDatabase.ProductId);
        Assert.AreEqual(productDto.ProductName,productInDatabase.ProductName);
    }
    [Test]
    public async Task Test_ProductService_Createproduct_ReturnNull_WhenMissingRequiredField()
    {
        var productDto = new ProductDTO(){
            ProductId = Guid.NewGuid(),
            ProductName = null,
            Manufacture = "Honda",
            CategoryId = _categoryListTest[0].CategoryId,
        };
        var result = await _productService.CreateProductAsync(productDto);
        VerifyLogger("Something went wrong!");
        Assert.IsNull(result);

    }

    [Test]
    public async Task Test_ProductService_DeleteProduct_ReturnTrue_And_SaveDatabase_WhenInputValidId()
    {
        var productIdTest = _productListTest[0].ProductId;

        var result = await _productService.DeleteProductAsync(productIdTest);

        Assert.AreEqual(2,_productStoreDbContextIM.ProductEntity.ToList().Count);
        Assert.AreEqual(true,result);
    }
    [Test]
    public async Task Test_ProductService_Deleteproduct_ReturnFalse_When_ProductDoesntExist()
    {
        var productIdTest = Guid.NewGuid();

        var result = await _productService.DeleteProductAsync(productIdTest);

        Assert.AreEqual(3,_productStoreDbContextIM.ProductEntity.ToList().Count);
        Assert.AreEqual(false,result);
    }
    [Test]
    public async Task Test_ProductService_GetproductById_ReturnSuccessProductDTO_WhenInputValidId()
    {
        var productIdTest = _productListTest[0].ProductId;

        var result = await _productService.GetProductDetailAsync(productIdTest);
        
        Assert.AreEqual(productIdTest, result.ProductId);
    }
    [Test]
    public async Task Test_ProductService_GetProductById_ReturnNull_WhenProductDoesntExist()
    {
        var productIdTest = Guid.NewGuid();

        var result = await _productService.GetProductDetailAsync(productIdTest);
        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_ProductService_EditProduct_ReturnSuccessProduct_And_SaveDatabase_WhenInputAllValid()
    {
        var productIdTest = _productListTest[0].ProductId;
        var request = new ProductDTO(){
            ProductName = "Accord",
            Manufacture = "Honda",
            CategoryId = _categoryListTest[1].CategoryId,
        };

        var result = await _productService.EditProductAsync((Guid)productIdTest,request);

        Assert.AreEqual(productIdTest, result.ProductId);
        Assert.AreEqual(request.ProductName, result.ProductName);
        Assert.AreEqual(request.CategoryId, result.CategoryId);
    }
    [Test]
    public async Task Test_ProductService_EditProduct_ReturnNull_WhenProductDoesntExist()
    {
        var productIdTest = Guid.NewGuid();
        var request = new ProductDTO(){
            ProductId = Guid.NewGuid(),
            ProductName = "Accord",
            Manufacture = "Honda",
            CategoryId = _categoryListTest[1].CategoryId,
        };
        
        var result = await _productService.EditProductAsync(productIdTest,request);

        Assert.IsNull(result);
    }
    
}