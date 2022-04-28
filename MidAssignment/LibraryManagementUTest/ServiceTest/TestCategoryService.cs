using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Requests;
using LibraryManagementBE.Services;
using LibraryManagementBE.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace LibraryManagementUTest;
public class TestCategoryService
{
    private LibraryManagementDBContext? _libraryMDBInMemoryContext;
    private IMapper? _mapper;
    private ShareSetupTest? _shareSetupTest;
    private ICategoryService? _categoryService;
    private Mock<ILogger<CategoryService>>? _mockLogger;
    private List<CategoryEntity>? _categoryTestList;

    [SetUp]
    public void SetUp(){
        _mockLogger = new Mock<ILogger<CategoryService>>();
        _shareSetupTest= new ShareSetupTest();
        _mapper = _shareSetupTest.MappingForTesting();
        _libraryMDBInMemoryContext = _shareSetupTest.InMemoryDatabaseSetup();
        _categoryService = new CategoryService(_mockLogger.Object,_libraryMDBInMemoryContext,_mapper);
        _categoryTestList = _libraryMDBInMemoryContext.CategoryEntity.ToList();
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
    public async Task Test_CategoryService_GetCategories_ReturnListCategory_WithPaging()
    {  
        var request = new PagingRequest(){
            PageSize = 1,
            PageIndex = 1,
        };

        //action
        var result = await _categoryService.GetCategoriesAsync(request);

        // Assert
        Assert.IsInstanceOf<PagingResult<CategoryDTO>>(result);
        Assert.IsInstanceOf<List<CategoryDTO>>(result.Items);
        Assert.AreEqual(request.PageSize, result.Items.Count);
        Assert.AreEqual(request.PageSize, result.PageIndex);
        Assert.AreEqual(_categoryTestList[0].Id, result.Items[0].Id);
    }

    [Test]
    public async Task Test_CategoryService_CreateCategory_ReturnASuccessCategoryDTO_And_SaveInDatabase_WhenInputAllValidFields()
    {
        var categoryDto = new CategoryDTO(){
            Id = Guid.NewGuid(),
            CategoryName = "Romance"
        };

        var result = await _categoryService.CreateCategoryAsync(categoryDto);
        var categoryInDatabase = _libraryMDBInMemoryContext.CategoryEntity.FirstOrDefault(x=>x.Id == categoryDto.Id);

        Assert.AreEqual(categoryDto.Id, result.Id);
        Assert.AreEqual(categoryDto.Id,categoryInDatabase.Id);
         Assert.AreEqual(categoryDto.CategoryName,categoryInDatabase.CategoryName);
    }
    [Test]
    public async Task Test_CategoryService_CreateCategory_ReturnNull_WhenMissingRequiredField()
    {
        var categoryDto = new CategoryDTO(){
            Id = Guid.NewGuid(),
            CategoryName = null
        };
        var result = await _categoryService.CreateCategoryAsync(categoryDto);
        VerifyLogger("Something went wrong!");
        Assert.IsNull(result);

    }

    [Test]
    public async Task Test_CategoryService_DeleteCategory_ReturnTrue_And_SaveDatabase_WhenInputValidId()
    {
        var categoryIdTest = _categoryTestList[0].Id;

        var result = await _categoryService.DeleteCategoryAsync(categoryIdTest);

        Assert.AreEqual(1,_libraryMDBInMemoryContext.CategoryEntity.ToList().Count);
        Assert.AreEqual(true,result);
    }
    [Test]
    public async Task Test_CategoryService_DeleteCategory_ReturnFalse_When_CategoryDoesntExist()
    {
        var categoryIdTest = Guid.NewGuid();

        var result = await _categoryService.DeleteCategoryAsync(categoryIdTest);

        Assert.AreEqual(2,_libraryMDBInMemoryContext.CategoryEntity.ToList().Count);
        Assert.AreEqual(false,result);
    }
    [Test]
    public async Task Test_CategoryService_GetCategoryById_ReturnSuccessCategoryDTO_WhenInputValidId()
    {
        var categoryIdTest = _categoryTestList[0].Id;

        var result = await _categoryService.GetCategoryAsync(categoryIdTest);
        Assert.IsInstanceOf<CategoryDTO>(result);
        Assert.AreEqual(categoryIdTest, result.Id);
    }
    [Test]
    public async Task Test_CategoryService_GetCategoryById_ReturnNull_WhenCategoryDoesntExist()
    {
        var categoryIdTest = Guid.NewGuid();

        var result = await _categoryService.GetCategoryAsync(categoryIdTest);

        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_CategoryServiceTest_EditCategory_ReturnSuccessCategory_And_SaveDatabase_WhenInputAllValid()
    {
        var categoryIdTest = _categoryTestList[0].Id;
        var request = new CategoryDTO(){
            CategoryName = "sss"
        };

        var result = await _categoryService.UpdateCategoryAsync(request,categoryIdTest);

        Assert.AreEqual(categoryIdTest, result.Id);
        Assert.AreEqual(request.CategoryName, result.CategoryName);
    }
    [Test]
    public async Task Test_CategoryService_EditCategory_ReturnNull_WhenCategoryDoesntExist()
    {
        var categoryIdTest = Guid.NewGuid();
        var request = new CategoryDTO(){
            CategoryName = "sss"
        };
        
        var result = await _categoryService.UpdateCategoryAsync(request,categoryIdTest);

        Assert.IsNull(result);
    }
}