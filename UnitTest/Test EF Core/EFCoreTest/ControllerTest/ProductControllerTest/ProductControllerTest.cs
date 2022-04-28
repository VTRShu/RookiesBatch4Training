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

public class ProductControllerTest
{   
    private Mock<IProductService>? _productService;
    private IMapper? _mapper;
    private ProductController? _productController;
    private ShareSetupTest? _shareSetupTest;
    static List<ProductEntity>? _productList;
    private List<ProductDTO>? _productListDto = new List<ProductDTO>(){
            new ProductDTO(){ProductId = Guid.NewGuid(),ProductName="X17",CategoryId=Guid.NewGuid()},
            new ProductDTO(){ProductId = Guid.NewGuid(),ProductName="Omen",Manufacture ="Hp",CategoryId=Guid.NewGuid()}
        };

    [SetUp]
    public void SetUp()
    {   
        _shareSetupTest = new ShareSetupTest();
        _mapper = _shareSetupTest.MappingForTesting();
        _productService = new Mock<IProductService>();

        _productController = new ProductController(_productService.Object,_mapper);
    }
    
    [Test]
    public async Task Test_ProductController_GetProductList_ReturnAListProduct()
    {   
        _productService.Setup(x=>x.GetListProductAsync())
        .Returns(Task.FromResult(_productListDto));
        
        var result = await _productController.Products() as OkObjectResult;
        var resultObject = result.Value as List<ProductModel>;

        Assert.IsNotEmpty(resultObject);
        Assert.AreEqual(2,resultObject.Count);
        Assert.AreEqual("X17",resultObject[0].ProductName);
    }
    [Test]
    public async Task Test_ProductController_GetProductById_ReturnASuccessProductDTO_WhenInputValidId()
    {   
        Guid productIdTest = _productListDto[0].ProductId;
        _productService.Setup(x=>x.GetProductDetailAsync(productIdTest)).Returns(Task.FromResult(_productListDto[0]));

        var result = await _productController.Product(productIdTest) as OkObjectResult;
        var resultObject = result.Value as ProductModel;

        Assert.AreEqual(resultObject.ProductName,_productListDto[0].ProductName);
    }
    [Test]
    public async Task Test_ProductController_GetProductById_ReturnBadRequest_WhenProductDoesntExist()
    {
        Guid productIdTest = Guid.NewGuid();
        
        var result = await _productController.Product(productIdTest) as BadRequestResult;

        Assert.AreEqual(400, result.StatusCode);
    }
    [Test]
    public async Task Test_ProductController_DeleteProduct_ReturnOKIsTrue_WhenInputExistProductId()
    {
        Guid productIdTest = _productListDto[0].ProductId;
        _productService.Setup(x=>x.DeleteProductAsync(productIdTest)).Returns(Task.FromResult(true));

        var result = await _productController.Delete(productIdTest) as OkObjectResult;

        Assert.AreEqual(true,result.Value);
        Assert.AreEqual(200,result.StatusCode);
    }
    [Test]
    public async Task Test_ProductController_DeleteProduct_ReturnOKValueFalse_WhenProductDoesntExist()
    {
        Guid productIdTest = Guid.NewGuid();
        _productService.Setup(x=>x.DeleteProductAsync(productIdTest)).Returns(Task.FromResult(false));

        var result = await _productController.Delete(productIdTest) as OkObjectResult;

        Assert.AreEqual(false,result.Value);
        Assert.AreEqual(200,result.StatusCode);
    }
    [Test]
    public async Task Test_ProductController_CreateProduct_ReturnSuccessProduct_And_SaveInDatabase_WhenInputAllValid()
    {
        var request = new ProductModel(){
            ProductName = "Kawasaki H2",
            Manufacture = "Kawasaki",
            CategoryId = Guid.NewGuid()
        };

        var requestDto =_mapper.Map<ProductDTO>(request);        
        var response = new ProductDTO(){
             ProductName = "Kawasaki H2",
            Manufacture = "Kawasaki",
            CategoryId = request.CategoryId
        };
        _productService.Setup(x=>x.CreateProductAsync(requestDto)).Returns(Task.FromResult(response));
       
        var result = await _productController.Create(request) ;  

        Assert.IsInstanceOf<OkObjectResult>(result);
    }
    [Test]
    public async Task Test_ProductController_EditProduct_ReturnSucessProduct_WhenInputValidIdAndFields()
    {
        var ProductIdTest = _productListDto[0].ProductId;
        var request = new ProductModel(){
            ProductName = "Kawasaki H2",
            Manufacture = "Kawasaki",
            CategoryId = Guid.NewGuid()
        };
        var requestDto = _mapper.Map<ProductDTO>(request);
        var responseDto = _mapper.Map<ProductDTO>(request);
    
        _productService.Setup(x=>x.EditProductAsync(ProductIdTest,requestDto)).Returns(Task.FromResult(responseDto));
        var result = await _productController.Edit(ProductIdTest,request);

        Assert.IsInstanceOf<OkObjectResult>(result);
    }
    [Test]
    public async Task Test_CategoryControllerTest_EditCategory_ReturnBadRequest_WhenCategoryDoesntExist()
    {
        var ProductIdTest = Guid.NewGuid();
        var request = new ProductModel(){
            ProductName = "Kawasaki H2",
            Manufacture = "Kawasaki",
            CategoryId = Guid.NewGuid()
        };
        var requestDto = _mapper.Map<ProductDTO>(request);
        var responseDto = _mapper.Map<ProductDTO>(request);

         _productService.Setup(x=>x.EditProductAsync(ProductIdTest,requestDto)).Returns(Task.FromResult(responseDto));

        var result = await _productController.Edit(ProductIdTest,request) as BadRequestResult;

        Assert.AreEqual(400,result.StatusCode);
    }
}