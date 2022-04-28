using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using LibraryManagementBE.Repositories.Requests;
using LibraryManagementBE.Services;
using LibraryManagementBE.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;


namespace LibraryManagementUTest;
public class TestAuthenticationService
{
    private IAuthenticationService? _authenticationService;
    private LibraryManagementDBContext? _libraryMDBInMemoryContext;
    private ShareSetupTest? _shareSetupTest;
    private Mock<IUserStore<AppUser>>? _userStore;
    private Mock<UserManager<AppUser>>? _userManagerMock;
    private Mock<SignInManager<AppUser>>? _signInManagerMock;
    private Mock<RoleManager<AppRole>>? _roleManagerMock;
    private IConfiguration _config;
    private Mock<ILogger<AuthenticationService>>? _mockLogger;
    
    [SetUp]
    public void SetUp()
    {      
        _shareSetupTest = new ShareSetupTest();
        _libraryMDBInMemoryContext = _shareSetupTest.InMemoryDatabaseSetup();
        // _userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        // _signInManagerMock = new Mock<SignInManager<AppUser>>(
        //     _userManagerMock.Object,Mock.Of<IHttpContextAccessor>(),
        //     Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(), 
        //     null,null,null,null);
        // _roleManagerMock = new Mock<RoleManager<AppRole>>(Mock.Of<IRoleStore<AppRole>>(), null, null, null, null, null, null);
        // _authenticationService = new AuthenticationService(
        //     _userManagerMock.Object,
        //     _signInManagerMock.Object,
        //     _roleManagerMock.Object,
        //     _config,
        //     _libraryMDBInMemoryContext,
        //     _mockLogger.Object);
    }
    // [Test]
    // public async Task Test_AuthenticationService_Login_Success_Return_Token()
    // {
    //     var request = new LoginRequest(){
    //         UserName = "nghialt",
    //         Password = "Admin123@", 
    //         RememberMe = true
    //     };

    //     var result = _authenticationService.Authenticate(request);

    //     Assert.IsNotNull(result);
    // }
}