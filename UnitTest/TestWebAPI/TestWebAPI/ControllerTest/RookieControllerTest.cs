using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Enums;
using WebAPI.Services;
using WebAPI.Services.Implement;
using NUnit.Framework;
using Moq;
using WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebAPI.Request;

namespace TestWebAPI;

public class RookieControllerTest
{   
    private Mock<IRookieService> _mockRookieService;
    private RookieController _rookieController;
    private static List<RookieModel> _rookieList ;
    private IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
    [SetUp]
    public void Setup()
    {
        _mockRookieService = new Mock<IRookieService>();
        _rookieController = new RookieController(_mockRookieService.Object);
        _rookieList = new List<RookieModel>()
        {
            new RookieModel(){RookieId = 1, FirstName = "Long" ,  LastName = "Bao",   Email = "LongBNash@gmail.com",  Gender =Gender.Male,  DoB = new DateTime(1995,2,2), BirthPlace ="HaNoi",PhoneNumber="012345678", Graduated = true},
            new RookieModel(){RookieId = 2, FirstName = "Ky" ,    LastName = "Nguyen",Email = "KyNNash@gmail.com",    Gender =Gender.Male,  DoB = new DateTime(1996,2,2), BirthPlace ="Nam Dinh", PhoneNumber="012345678", Graduated = true},
            new RookieModel(){RookieId = 3, FirstName = "Hung" ,  LastName = "Hoang", Email = "HungHNash@gmail.com",  Gender =Gender.Male,  DoB = new DateTime(1991,2,2), BirthPlace ="HaNoi" ,   PhoneNumber="012345678", Graduated = true},
        };
        _mockRookieService.Setup(x=>x.GetRookieListAsync()).Returns(Task.FromResult(_rookieList));
    }

    [Test]
    public async Task Test_RookieController_ReturnListRookie()
    {   
        //Act
        var result = await _rookieController.Rookies() as OkObjectResult;
        var resultObject = result.Value as List<RookieModel>;
        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual(3,resultObject.Count);
    }

    [Test]
    public async Task Test_RookieController_ReturnRookie_WhenIDExist()
    {   
        int testId = 1;
        _mockRookieService.Setup(x=>x.GetRookieDetailsAsync(testId)).Returns(Task.FromResult(_rookieList.Find(x=>x.RookieId == testId)));
        //Act
        
        var result = await _rookieController.Rookie(testId) as OkObjectResult;

        //Assert
        Assert.IsAssignableFrom<RookieModel>(result.Value);
        Assert.AreEqual(testId, (result.Value as RookieModel).RookieId);
    }
    [Test]
    public async Task Test_RookieController_ReturnNotFound_WhenIDDoesntExist()
    {   
        int testId = 4;
        _mockRookieService.Setup(x=>x.GetRookieDetailsAsync(testId)).Returns(Task.FromResult(_rookieList.Find(x=>x.RookieId == testId)));
        //Act
        
        var result = await _rookieController.Rookie(testId) as NotFoundResult;

        //Assert
        Assert.AreEqual(404, result.StatusCode);
    }
    [Test]
    public async Task Test_RookieController_CreateRookie_InputAllValid_ReturnANewRookie()
    {
        var request = new RookieModel()
        {
            FirstName = "Trang" ,  LastName = "Nguyen",   Email = "TrangNguyen@gmail.com",  Gender =Gender.Female,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345678", Graduated = true
        };
        var response = new RookieModel()
        {
            FirstName = "Trang" ,  LastName = "Nguyen",   Email = "TrangNguyen@gmail.com",  Gender =Gender.Female,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345678", Graduated = true
        };
        _mockRookieService.Setup(x=>x.CreateNewRookieAsync(request)).Returns(Task.FromResult(response));

        var result = await _rookieController.Create(request);
        var resultObject = (result as ObjectResult).Value as RookieModel;
        Assert.AreEqual(200, (result as OkObjectResult).StatusCode);
        Assert.AreEqual(request.FirstName, resultObject.FirstName);
    }

    [Test]
    public async Task Test_RookieController_CreateRookie_TestInValidPhoneNumber_ReturnErrorMessage()
    {
        var request = new RookieModel()
        {
            FirstName = "Trang" ,  LastName = "Nguyen",   Email = "TrangNguyen@gmail.com",  Gender =Gender.Female,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345", Graduated = true
        };
        var errorList = ValidateModel(request);
        Assert.IsTrue(errorList.Where(x=>x.ErrorMessage.Contains("Number digit only!, at least 9")).Count()>0);
    }
    [Test]
    public async Task Test_RookieController_CreateRookie_TestInvalidEmail_ReturnErrorMessage()
    {
        var request = new RookieModel()
        {
            FirstName = "Trang" ,  LastName = "Nguyen",   Email = "TrangNguyen",  Gender =Gender.Female,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345678", Graduated = true
        };
        var errorList = ValidateModel(request);
        Assert.IsTrue(errorList.Where(x=>x.ErrorMessage.Contains("Email is not valid!")).Count()>0);
    }
    [Test]
    public async Task Test_RookieController_CreateRookie_TestEmptyRequireField_ReturnErrorMessage()
    {
        var request = new RookieModel()
        {
            FirstName = "" ,  LastName = "",   Email = "TrangNguyen@gmail.com",  Gender =Gender.Female,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345678", Graduated = true
        };
         var errorList = ValidateModel(request);
        Assert.IsTrue(errorList.Where(x=>x.ErrorMessage.Contains("is required")).Count()>0);
    }
    [Test]
    public async Task Test_RookieController_CreateRookie_TestAllInvalidCase_ReturnAllErrorMessage()
    {
        var request = new RookieModel()
        {
            FirstName = "" ,  LastName = "",   Email = "TrangNguyen@",  Gender =Gender.Female,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345", Graduated = true
        };
         var errorList = ValidateModel(request);
        Assert.IsTrue(errorList.Where(x=>x.ErrorMessage.Contains("is required")).Count()>0);
        Assert.IsTrue(errorList.Where(x=>x.ErrorMessage.Contains("Email is not valid!")).Count()>0);
        Assert.IsTrue(errorList.Where(x=>x.ErrorMessage.Contains("Number digit only!, at least 9")).Count()>0);
    }
    [Test]
    public async Task Test_RookieController_UpdateRookie_ReturnRookie_WhenAllValid()
    {
        var request = new RookieModel()
        {
            FirstName = "hung" ,  LastName = "",   Email = "HungHNash@gmail.com",  Gender =Gender.Male,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345678", Graduated = true
        };
        int testId = 3;
        _mockRookieService.Setup(x=>x.EditRookieAsync(testId,request)).Returns(Task.FromResult((RookieModel)request));
        
        var result = await _rookieController.Edit(testId,request);
        var resultObject = (result as OkObjectResult).Value as RookieModel;
        
        Assert.IsInstanceOf<OkObjectResult>(result as OkObjectResult);
        Assert.AreEqual(request.LastName,resultObject.LastName);
    }   
    [Test]
    public async Task Test_RookieController_UpdateRookie_ReturnABadRequest_WhenRookieDoesntExist()
    {
        var request = new RookieModel()
        {
            FirstName = "hung" ,  LastName = "v",   Email = "HungHNash@gmail.com",  Gender =Gender.Male,  DoB = new DateTime(1995,2,2), BirthPlace ="QuangNinh",PhoneNumber="012345678", Graduated = true
        };
        int testId = 10;
        _mockRookieService.Setup(x=>x.EditRookieAsync(testId,request)).Returns(Task.FromResult<RookieModel>(null));

        var result = await _rookieController.Edit(testId,request);
        var resultObject = result as BadRequestObjectResult;

        Assert.AreEqual(400, resultObject.StatusCode);
    }
    [Test]
    public async Task Test_RookieController_DeleteRookie_ReturnTrue_WhenDeleteSuccess()
    {
        int testId = 1;
        _mockRookieService.Setup(x=>x.DeleteRookieAsync(testId)).Returns(Task.FromResult(true));

        var result = await _rookieController.Delete(testId) as OkObjectResult;
        var resultObject = result.Value;

        Assert.AreEqual(200, result.StatusCode);
        Assert.AreEqual(true,resultObject);
    }

    [Test]
    public async Task Test_RookieController_DeleteRookie_ReturnBadRequest_WhenInputRookieDoesntExist()
    {
        int testId = 10;
        _mockRookieService.Setup(x=>x.DeleteRookieAsync(testId)).Returns(Task.FromResult(false));

        var result = await _rookieController.Delete(testId) as BadRequestResult;

        Assert.AreEqual(400, result.StatusCode);
    }

    [Test]
    public async Task Test_RookieController_FilterRookie_ByName_BirthPlace_Gender_ReturnList()
    {

    }
    [Test]
    public async Task Test_RookieController_FilterRookie_JustBy_Name_ReturnList()
    {
        var requestFilter = new FilterRequest()
        {
            Name = "Hung",
            BirthPlace ="",
            Gender = null,
        };

        _mockRookieService.Setup(x=>x.FilterRookie(requestFilter))
        .Returns(Task.FromResult(_rookieList.FindAll(x=>x.LastName.Equals(requestFilter.Name)
                                                    ||x.FirstName.Equals(requestFilter.Name))));

        var result = await _rookieController.Filter(requestFilter);
        var resultObject = (result as OkObjectResult).Value as List<RookieModel>;

        Assert.AreEqual(2,resultObject.Count);

    }
    [Test]
    public async Task Test_RookieController_FilterRookie_JustBy_BirthPlace_ReturnListRookie_ButEmpty()
    {
         var requestFilter = new FilterRequest()
        {
            Name = "",
            BirthPlace ="HHHH",
            Gender = null,
        };
         _mockRookieService.Setup(x=>x.FilterRookie(requestFilter))
        .Returns(Task.FromResult(_rookieList.FindAll(x=>x.BirthPlace.Equals(requestFilter.BirthPlace))));
        
        var result = await _rookieController.Filter(requestFilter);
        var resultObject = (result as OkObjectResult).Value as List<RookieModel>;

        Assert.IsNotEmpty(resultObject);
    }
    [Test]
    public async Task Test_RookieController_FilterRookie_ByAllFilterRequest_ReturnRookie()
    {
         var requestFilter = new FilterRequest()
        {
            Name = "Hung",
            BirthPlace ="hanoi",
            Gender = (Gender?)1,
        };
         _mockRookieService.Setup(x=>x.FilterRookie(requestFilter))
        .Returns(Task.FromResult(_rookieList.FindAll(x=>x.BirthPlace.Equals(requestFilter.BirthPlace) 
        && x.FullName.Contains(requestFilter.Name)
        && x.Gender == requestFilter.Gender)));
        
        var result = await _rookieController.Filter(requestFilter);
        var resultObject = (result as OkObjectResult).Value as List<RookieModel>;

        Assert.IsNotEmpty(resultObject);
    }
}