using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using LibraryManagementBE.Repositories.Requests;
using LibraryManagementBE.Services;
using LibraryManagementBE.Services.Implements;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace LibraryManagementUTest;
public class TestUserService
{
    private LibraryManagementDBContext? _libraryMDBInMemoryContext;
    private IMapper? _mapper;
    private ShareSetupTest? _shareSetupTest;
    private IUserService? _userService;
    private Mock<ILogger<UserService>>? _mockLogger;
    private List<AppUser>? _userTestList;
    private Mock<UserManager<AppUser>>? _userManagerMock;
    private List<AppUser>? _allTestUser;

    [SetUp]
    public void SetUp(){
        _mockLogger = new Mock<ILogger<UserService>>();
        _shareSetupTest= new ShareSetupTest();
        _mapper = _shareSetupTest.MappingForTesting();
        _libraryMDBInMemoryContext = _shareSetupTest.InMemoryDatabaseSetup();
        _userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        _userService = new UserService(_userManagerMock.Object,_libraryMDBInMemoryContext,_mockLogger.Object,_mapper);
        _userTestList = _libraryMDBInMemoryContext.AppUser.Where(x=>x.IsDisabled == false).ToList();
        _allTestUser = _libraryMDBInMemoryContext.AppUser.ToList();
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
    public async Task Test_UserService_GetUsers_ReturnListUser_WhoIsntDisable_WithPaging()
    {  
        var request = new PagingRequest(){
            PageSize = 3,
            PageIndex = 1,
        };

        //action
        var result = await _userService.GetUserListAsync(request);

        // Assert
        Assert.IsInstanceOf<PagingResult<AppUserDTO>>(result);
        Assert.AreEqual(2, result.Items.Count);
        Assert.AreEqual(request.PageSize,result.PageSize);
        Assert.AreEqual(request.PageIndex, result.PageIndex);
        Assert.AreEqual(_userTestList[0].Id, result.Items[0].Id);
        Assert.AreEqual(false,result.Items[0].IsDisabled);
    }
    [Test]
    public async Task Test_UserService_GetUser_ReturnUserDTO_WhenIDValid()
    {
        var userIdTest = _userTestList[0].Id;
        var nameTest = "Le Trung Nghia";
        var result = await _userService.GetUserDetailsAsync(userIdTest);

        Assert.IsInstanceOf<AppUserDTO>(result);
        Assert.AreEqual(nameTest,result.FullName);
    }

    [Test]
    public async Task Test_UserService_GetUser_ReturnNull_WhenIDInValid()
    {
        var userIdTest = Guid.NewGuid();

        var result = await _userService.GetUserDetailsAsync(userIdTest);

        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_UserService_DisableUser_ReturnTrue_AndChangeInDB_WhenDisableSuccess()
    {
        var userIdTest = _userTestList[1].Id;

        var result = await _userService.DisableUserAsync(userIdTest);
        var userIMDb = _libraryMDBInMemoryContext.AppUser.FirstOrDefault(x => x.Id == userIdTest);

        Assert.AreEqual(true,result);
        Assert.AreEqual(true,userIMDb.IsDisabled);
    }
    [Test]
    public async Task Test_UserService_DisableUser_ReturnFalse_WhenDisableFail_IdInvalid()
    {
        var userIdTest = Guid.NewGuid();

        var result = await _userService.DisableUserAsync(userIdTest);

        Assert.AreEqual(false,result);
    }
    [Test]
    public async Task Test_UserService_DisableUser_ReturnFalse_WhenDisable_UserAlreadyDisable()
    {
        var userIdTest = _allTestUser[1].Id;

        var result = await _userService.DisableUserAsync(userIdTest);

        Assert.AreEqual(false,result);
    }
    [Test]
    public async Task Test_UserService_CreatenewUser_ReturnASuccessUserDTO_AddToDB_WhenInputAllValid()
    {
        var newUserTest = new AppUserDTO(){
            Id = Guid.NewGuid(),
            FullName = "Hoang Mai Yen",
            Dob = new DateTime(2000,1,1),
            Gender = (Gender)1,
            Type = (Role)0,
            UserName = "yen20",
            Email = "yen@gmail.com",
            Password = "Admin123@"
        };
        var newUser = _mapper.Map<AppUser>(newUserTest);
        var role = "NormalUser";
        _userManagerMock.Setup(x=> x.CreateAsync(newUser,newUserTest.Password)).Returns(Task.FromResult(IdentityResult.Success));
        _userManagerMock.Setup(x=> x.AddToRoleAsync(newUser,role)).Returns(Task.FromResult(IdentityResult.Success));
        
        var result = await _userService.CreateUserAsync(newUserTest);
        var newUserInDB = _libraryMDBInMemoryContext.AppUser.FirstOrDefault(x=>x.Id == newUserTest.Id);

        Assert.AreEqual("Hoang Mai Yen", newUserInDB.FullName);
        // Assert.AreEqual(newUserTest.Id, _libraryMDBInMemoryContext.UserRoles.ToList()[3].UserId);
        Assert.IsInstanceOf<AppUserDTO>(result);
    }
    [Test]
    public async Task Test_UserService_CreatenewUser_ReturnNull_WhenInputExistEmail()
    {
        var newUserTest = new AppUserDTO(){
            Id = Guid.NewGuid(),
            FullName = "Hoang Mai Yen",
            Dob = new DateTime(2000,1,1),
            Gender = (Gender)1,
            Type = (Role)0,
            UserName = "yen20",
            Email = "nghia@gmail.com",
            Password = "Admin123@"
        };
        
        var result = await _userService.CreateUserAsync(newUserTest);
    
        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_UserService_CreatenewUser_ReturnNull_WhenInputExistUsername()
    {
        var newUserTest = new AppUserDTO(){
            Id = Guid.NewGuid(),
            FullName = "Hoang Mai Yen",
            Dob = new DateTime(2000,1,1),
            Gender = (Gender)1,
            Type = (Role)0,
            UserName = "nghialt",
            Email = "yen@gmail.com",
            Password = "Admin123@"
        };
        
        var result = await _userService.CreateUserAsync(newUserTest);
    
        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_UserService_CreatenewUser_ReturnNull_WhenFieldInputIsNull()
    {
        var newUserTest = new AppUserDTO(){
            Id = Guid.NewGuid(),
            FullName = "Hoang Mai Yen",
            Dob = new DateTime(2000,1,1),
            Gender = (Gender)1,
            Type = (Role)0,
            UserName = "nghialt",
            Email = "yen@gmail.com",
            Password = null
        };
        
        var result = await _userService.CreateUserAsync(newUserTest);
    
        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_UserService_UpdateExistUser_ReturnSuccessUserDTO_WhenInputAllValid()
    {
        var existUserIdTest = _userTestList[1].Id;
        var editRequest = new AppUserDTO(){
            FullName = "Hoang Tho Thang",
            Gender = (Gender)1,
            Dob = new DateTime(2000,1,1),
        };
        var role = "SuperUser";

        var result = await _userService.UpdateUserAsync(role, editRequest,existUserIdTest);

        Assert.IsInstanceOf<AppUserDTO>(result);
        Assert.AreEqual(editRequest.FullName,result.FullName);
        Assert.AreEqual(editRequest.Gender,result.Gender);
    }
    [Test]
    public async Task Test_UserService_UpdateExistUser_ReturnNull_WhenUserDoesntExist()
    {
        var existUserIdTest = Guid.NewGuid();
        var editRequest = new AppUserDTO(){
            FullName = "Hoang Tho Thang",
            Gender = (Gender)1,
            Dob = new DateTime(2000,1,1),
        };
        var role = "SuperUser";

        var result = await _userService.UpdateUserAsync(role, editRequest,existUserIdTest);

        Assert.IsNull(result);
    }
}