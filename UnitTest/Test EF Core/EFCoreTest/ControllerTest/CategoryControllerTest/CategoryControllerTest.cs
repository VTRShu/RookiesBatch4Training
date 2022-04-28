using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EFCore_Ex2.Controllers;
using EFCore_Ex2.Mappings;
using EFCore_Ex2.Models;
using EFCore_Ex2.Repositories.DTO;
using EFCore_Ex2.Repositories.EFContext;
using EFCore_Ex2.Repositories.Entities;
using EFCore_Ex2.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EFCoreTest;

public class CategoryControllerTest
{   
    private Mock<ICategoryService>? _categoryService;
    private IMapper? _mapper;
    private CategoryController? _categoryController;
    private ShareSetupTest? _shareSetupTest;
    static List<CategoryEntity>? _categoryList;
    private List<CategoryDTO>? _categoryListDTO = new List<CategoryDTO>(){
            new CategoryDTO(){CategoryId = Guid.NewGuid(),CategoryName="Laptop"},
            new CategoryDTO(){CategoryId = Guid.NewGuid(),CategoryName="Bike"}
        };

    [SetUp]
    public void SetUp()
    {   
        _shareSetupTest = new ShareSetupTest();
        _mapper = _shareSetupTest.MappingForTesting();
        _categoryService = new Mock<ICategoryService>();

        _categoryController = new CategoryController(_categoryService.Object,_mapper);
    }
    
    [Test]
    public async Task Test_CategoryController_GetCategoryList_ReturnAListCategory()
    {   
        _categoryService.Setup(x=>x.GetCategoryListAsync())
        .Returns(Task.FromResult(_categoryListDTO));
        
        var result = await _categoryController.Categories() as OkObjectResult;
        var resultObject = result.Value as List<CategoryModel>;

        Assert.IsNotEmpty(resultObject);
        Assert.AreEqual(2,resultObject.Count);
        Assert.AreEqual("Laptop",resultObject[0].CategoryName.ToString());
    }
    [Test]
    public async Task Test_CategoryController_GetCategoryById_ReturnOK_WhenInputValidId()
    {   
        Guid categoryId = _categoryListDTO[0].CategoryId;
        _categoryService.Setup(x=>x.GetCategoryDetailAsync(categoryId)).Returns(Task.FromResult(_categoryListDTO[0]));

        var result = await _categoryController.Category(categoryId) as OkObjectResult;
        var resultObject = result.Value as CategoryModel;

        Assert.IsNotNull(resultObject);
        Assert.AreEqual(resultObject.CategoryName,_categoryListDTO[0].CategoryName);
    }
    [Test]
    public async Task Test_CategoryController_GetCategoryById_ReturnBadRequest_WhenCategoryDoesntExist()
    {
        Guid categoryId = Guid.NewGuid();
        
        var result = await _categoryController.Category(categoryId) as BadRequestResult;

        Assert.AreEqual(400, result.StatusCode);
    }
    [Test]
    public async Task Test_CategoryController_DeleteCategory_ReturnOK_WhenInputExistCategoryId()
    {
        Guid categoryId = _categoryListDTO[0].CategoryId;
        _categoryService.Setup(x=>x.DeleteCategoryAsync(categoryId)).Returns(Task.FromResult(true));

        var result = await _categoryController.Delete(categoryId) as OkObjectResult;

        Assert.AreEqual(true,result.Value);
    }
    [Test]
    public async Task Test_CategoryController_DeleteCategory_ReturnOKValueFalse_WhenCategoryDoesntExist()
    {
        Guid categoryId = Guid.NewGuid();
        _categoryService.Setup(x=>x.DeleteCategoryAsync(categoryId)).Returns(Task.FromResult(false));

        var result = await _categoryController.Delete(categoryId) as OkObjectResult;

        Assert.AreEqual(false,result.Value);
    }
    [Test]
    public async Task Test_CategoryController_CreateCategory_ReturnSuccessCategory_And_SaveInDatabase_WhenInputAllValid()
    {
        var request = new CategoryModel(){
            CategoryName = "Car",
        };

        var requestDto =_mapper.Map<CategoryDTO>(request);        
        var response = new CategoryDTO(){
            CategoryName = "Car",
        };
        _categoryService.Setup(x=>x.CreateCategoryAsync(requestDto)).Returns(Task.FromResult(response));
       
        var result = await _categoryController.CreateNew(request) ;  

        Assert.IsInstanceOf<OkObjectResult>(result);
    }
    [Test]
    public async Task Test_CategoryController_EditCategory_ReturnSucessCategory_WhenInputValidIdAndFields()
    {
        var categoryIdTest = _categoryListDTO[0].CategoryId;
        var requestDto = new CategoryDTO(){
            CategoryId =categoryIdTest,
              CategoryName = "aaa"
        };
        var respone = new CategoryDTO(){
             CategoryId =categoryIdTest,
              CategoryName = "aaa"
        };
    
        _categoryService?.Setup(x=>x.EditCategoryAsync(categoryIdTest,requestDto)).Returns(Task.FromResult(respone));
        var requestModel = _mapper.Map<CategoryModel>(requestDto);
        var result = await _categoryController.Edit(categoryIdTest,requestModel);

        Assert.IsInstanceOf<OkObjectResult>(result);
    }
    [Test]
    public async Task Test_CategoryControllerTest_EditCategory_ReturnBadRequest_WhenCategoryDoesntExist()
    {
        var categoryIdTest = Guid.NewGuid();
        var requestDto = new CategoryDTO(){
            CategoryId =categoryIdTest,
              CategoryName = "aaa"
        };
        var respone = new CategoryDTO(){
             CategoryId =categoryIdTest,
              CategoryName = "aaa"
        };
         _categoryService?.Setup(x=>x.EditCategoryAsync(categoryIdTest,requestDto)).Returns(Task.FromResult(respone));
        var requestModel = _mapper.Map<CategoryModel>(requestDto);
        var result = await _categoryController.Edit(categoryIdTest,requestModel) as BadRequestResult;

        Assert.AreEqual(400,result.StatusCode);
    }
}