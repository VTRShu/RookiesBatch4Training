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

public class CategoryServiceTest
{   
    private ProductStoreDBContext? _productStoreDbContextIM;
    private DbContextOptions<ProductStoreDBContext>? _dbContextOptions;
    private IMapper? _mapper;
    private ShareSetupTest _shareSetupTest;
    private ICategoryService? _categoryService;
    private Mock<ILogger<CategoryService>> _mockLogger;
    private List<CategoryEntity>? _categoryListTest;
    [SetUp]
    public void SetUp()
    {   
        _mockLogger = new Mock<ILogger<CategoryService>>();
        _shareSetupTest= new ShareSetupTest();
        _mapper = _shareSetupTest.MappingForTesting();
        _productStoreDbContextIM = _shareSetupTest.InMemoryDatabaseSetupTest();
        _categoryService = new CategoryService(_productStoreDbContextIM,_mockLogger.Object,_mapper);
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
    public async Task Test_CategoryService_GetCategoryList_ReturnCategoryDTOList()
    {
        //Arrange
        var _categoryListDto = _productStoreDbContextIM.CategoryEntity.Select(x=>_mapper.Map<CategoryDTO>(x)).ToList();
        //Act
        var result = await _categoryService.GetCategoryListAsync();
   
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual(_categoryListDto[0].CategoryId, result[0].CategoryId);
    }
    [Test]
    public async Task Test_CategoryService_CreateCategory_ReturnASuccessCategoryDTO_And_SaveInDatabase_WhenInputAllValidFields()
    {
        var categoryDto = new CategoryDTO(){
            CategoryId = Guid.NewGuid(),
            CategoryName = "Car"
        };

        var result = await _categoryService.CreateCategoryAsync(categoryDto);
        var categoryInDatabase = _productStoreDbContextIM.CategoryEntity.FirstOrDefault(x=>x.CategoryId == categoryDto.CategoryId);

        Assert.AreEqual(categoryDto.CategoryId, result.CategoryId);
        Assert.AreEqual(categoryDto.CategoryId,categoryInDatabase.CategoryId);
    }
    [Test]
    public async Task Test_CategoryService_CreateCategory_ReturnNull_WhenMissingRequiredField()
    {
        var categoryDto = new CategoryDTO(){
            CategoryId = Guid.NewGuid(),
            CategoryName = null
        };
        var result = await _categoryService.CreateCategoryAsync(categoryDto);
        VerifyLogger("Something went wrong!");
        Assert.IsNull(result);

    }

    [Test]
    public async Task Test_CategoryService_DeleteCategory_ReturnTrue_And_SaveDatabase_WhenInputValidId()
    {
        var categoryIdTest = _categoryListTest[0].CategoryId;

        var result = await _categoryService.DeleteCategoryAsync(categoryIdTest);

        Assert.AreEqual(1,_productStoreDbContextIM.CategoryEntity.ToList().Count);
        Assert.AreEqual(true,result);
    }
    [Test]
    public async Task Test_CategoryService_DeleteCategory_ReturnFalse_When_CategoryDoesntExist()
    {
        var categoryIdTest = Guid.NewGuid();

        var result = await _categoryService.DeleteCategoryAsync(categoryIdTest);

        Assert.AreEqual(2,_productStoreDbContextIM.CategoryEntity.ToList().Count);
        Assert.AreEqual(false,result);
    }
    [Test]
    public async Task Test_CategoryService_GetCategoryById_ReturnSuccessCategoryDTO_WhenInputValidId()
    {
        var categoryIdTest = _categoryListTest[0].CategoryId;

        var result = await _categoryService.GetCategoryDetailAsync(categoryIdTest);
        Assert.IsInstanceOf<CategoryDTO>(result);
        Assert.AreEqual(categoryIdTest, result.CategoryId);
    }
    [Test]
    public async Task Test_CategoryService_GetCategoryById_ReturnNull_WhenCategoryDoesntExist()
    {
        var categoryIdTest = Guid.NewGuid();

        var result = await _categoryService.GetCategoryDetailAsync(categoryIdTest);

        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_CategoryServiceTest_EditCategory_ReturnSuccessCategory_And_SaveDatabase_WhenInputAllValid()
    {
        var categoryIdTest = _categoryListTest[0].CategoryId;
        var request = new CategoryDTO(){
            CategoryName = "sss"
        };

        var result = await _categoryService.EditCategoryAsync(categoryIdTest,request);

        Assert.AreEqual(categoryIdTest, result.CategoryId);
        Assert.AreEqual(request.CategoryName, result.CategoryName);
    }
    [Test]
    public async Task Test_CategoryService_EditCategory_ReturnNull_WhenCategoryDoesntExist()
    {
        var categoryIdTest = Guid.NewGuid();
        var request = new CategoryDTO(){
            CategoryName = "sss"
        };
        
        var result = await _categoryService.EditCategoryAsync(categoryIdTest,request);

        Assert.IsNull(result);
    }
    
}