using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using LibraryManagementBE.Repositories.Models;
using LibraryManagementBE.Repositories.Requests;
using LibraryManagementBE.Services;
using LibraryManagementBE.Services.Implements;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace LibraryManagementUTest;
public class TestBookBorrowService
{
    private LibraryManagementDBContext? _libraryMDBInMemoryContext;
    private IMapper? _mapper;
    private ShareSetupTest? _shareSetupTest;
    private IBookBorrowingRequestService? _bookBorrowService;
    private Mock<ILogger<BookBorrowingRequestService>>? _mockLogger;
    private List<AppUser>? _userTestList;
    private List<BookEntity>? _bookTestList;
    private List<BookBorrowingRequest>? _bookBorrowTestList;

    [SetUp]
    public void SetUp(){
        _mockLogger = new Mock<ILogger<BookBorrowingRequestService>>();
        _shareSetupTest= new ShareSetupTest();
        _mapper = _shareSetupTest.MappingForTesting();
        _libraryMDBInMemoryContext = _shareSetupTest.InMemoryDatabaseSetup();
        _bookBorrowService = new BookBorrowingRequestService(_mockLogger.Object,_libraryMDBInMemoryContext,_mapper);
        _bookTestList = _libraryMDBInMemoryContext.BookEntity.ToList();
        _userTestList = _libraryMDBInMemoryContext.AppUser.Where(x=>x.IsDisabled == false).ToList();
        _bookBorrowTestList = _libraryMDBInMemoryContext.BookBorrowingRequest.ToList();
    }

    [Test]
    public async Task Test_BookBorrowService_Create_Success_BorrowRequest_ReturnSuccessStringMessage()
    {
        var requestedUserId = _userTestList[1].Id;
        var borrowRequest = new BookBorrowingRequestDTO(){
            Status = (Status)2,
            RequestedAt= DateTime.Now, 
            Books = new List<Guid>(){
                _bookTestList[0].Id,
                _bookTestList[1].Id,
            }
        };

        var result = await _bookBorrowService.CreateBorrowRequestAsync(requestedUserId, borrowRequest);

        Assert.IsInstanceOf<string>(result);
        Assert.AreEqual("Create request successfully!",result);
    }
    [Test]
    public async Task Test_BookBorrowService_Create_Over5Books_BorrowRequest_ReturnStringMessage()
    {
        var requestedUserId = _userTestList[1].Id;
        var borrowRequest = new BookBorrowingRequestDTO(){
            Status = (Status)2,
            RequestedAt= DateTime.Now, 
            Books = new List<Guid>(){
                _bookTestList[0].Id,
                _bookTestList[1].Id,
                _bookTestList[1].Id,
                _bookTestList[1].Id,
                _bookTestList[1].Id,
                _bookTestList[1].Id
            }
        };

        var result = await _bookBorrowService.CreateBorrowRequestAsync(requestedUserId, borrowRequest);

        Assert.IsInstanceOf<string>(result);
        Assert.AreEqual("You have react the limit of book request!(Max 5)",result);
    }
    [Test]
    public async Task Test_BookBorrowService_Create_Over3RequestPerMonth_BorrowRequest_ReturnStringMessage()
    {
        var requestedUserId = _userTestList[1].Id;
        var borrowRequest = new BookBorrowingRequestDTO(){
            Status = (Status)2,
            RequestedAt= DateTime.Now, 
            Books = new List<Guid>(){
                _bookTestList[0].Id,
                _bookTestList[1].Id,
            }
        };

        var result1 = await _bookBorrowService.CreateBorrowRequestAsync(requestedUserId, borrowRequest);
        var result2 = await _bookBorrowService.CreateBorrowRequestAsync(requestedUserId, borrowRequest);
        var result3 = await _bookBorrowService.CreateBorrowRequestAsync(requestedUserId, borrowRequest);
        var result4 = await _bookBorrowService.CreateBorrowRequestAsync(requestedUserId, borrowRequest);
        Assert.IsInstanceOf<string>(result4);
        Assert.AreEqual("You can only make 3 request per month, pls wait for the next month!",result4);
    }
    [Test]
    public async Task Test_BookBorrowService_Create_ButBookInListBookDoestExist_BorrowRequest_ReturnStringMessage()
    {
        var requestedUserId = _userTestList[1].Id;
        var borrowRequest = new BookBorrowingRequestDTO(){
            Status = (Status)2,
            RequestedAt= DateTime.Now, 
            Books = new List<Guid>(){
               Guid.NewGuid(),
                _bookTestList[1].Id,
            }
        };

        var result = await _bookBorrowService.CreateBorrowRequestAsync(requestedUserId, borrowRequest);

        Assert.IsInstanceOf<string>(result);
        Assert.AreEqual("One of your books you requested doesnot exist!",result);
    }
    [Test]
    public async Task Test_BookBorrowService_Create_ButRequestedUserDoestExist_BorrowRequest_ReturnStringMessage()
    {
        var requestedUserId = Guid.NewGuid();
        var borrowRequest = new BookBorrowingRequestDTO(){
            Status = (Status)2,
            RequestedAt= DateTime.Now, 
            Books = new List<Guid>(){
                _bookTestList[1].Id,
                _bookTestList[1].Id,
            }
        };

        var result = await _bookBorrowService.CreateBorrowRequestAsync(requestedUserId, borrowRequest);

        Assert.IsInstanceOf<string>(result);
        Assert.AreEqual("Request User does not exist!",result);
    }

    [Test]
    public async Task Test_BookBorrowService_Respond_Approve_BorrowRequest_Return_True_And_StatusChangeToApprove_WhenRespondSuccess()
    {
        var borrowRequestTestId = _bookBorrowTestList[0].Id;
        var respondTestId = _userTestList[0].Id;
        var respond = "Approve";

        var result = await _bookBorrowService.SuperUserResponseRequestAsync(respondTestId,borrowRequestTestId,respond);
        var requestAfterRespond = _libraryMDBInMemoryContext.BookBorrowingRequest.FirstOrDefault(x=>x.Id == borrowRequestTestId);
        
        Assert.IsInstanceOf<bool>(result);
        Assert.AreEqual(true,result);
        Assert.AreEqual(_userTestList[0].Id,requestAfterRespond.ResponseById);
        Assert.AreEqual((Status)0,requestAfterRespond.Status);
    }

    [Test]
    public async Task Test_BookBorrowService_Respond_Reject_BorrowRequest_Return_True_And_StatusChangeToReject_WhenRespondSuccess()
    {
        var borrowRequestTestId = _bookBorrowTestList[0].Id;
        var respondTestId = _userTestList[0].Id;
        var respond = "Reject";

        var result = await _bookBorrowService.SuperUserResponseRequestAsync(respondTestId,borrowRequestTestId,respond);
        var requestAfterRespond = _libraryMDBInMemoryContext.BookBorrowingRequest.FirstOrDefault(x=>x.Id == borrowRequestTestId);
        
        Assert.IsInstanceOf<bool>(result);
        Assert.AreEqual(true,result);
        Assert.AreEqual(_userTestList[0].Id,requestAfterRespond.ResponseById);
        Assert.AreEqual((Status)1,requestAfterRespond.Status);
    }
    [Test]
    public async Task Test_BookBorrowService_Respond_BorrowRequest_Return_False_WhenRespondForRequestDoesntExist()
    {
        var borrowRequestTestId = Guid.NewGuid();
        var respondTestId = _userTestList[0].Id;
        var respond = "Approve";

        var result = await _bookBorrowService.SuperUserResponseRequestAsync(respondTestId,borrowRequestTestId,respond);
        
        Assert.IsInstanceOf<bool>(result);
        Assert.AreEqual(false,result);
    }
    [Test]
    public async Task Test_BookBorrowService_Respond_BorrowRequest_Return_False_WhenRespondIsInvalid()
    {
        var borrowRequestTestId = _bookBorrowTestList[0].Id;
        var respondTestId = _userTestList[0].Id;
        var respond = "aa";

        var result = await _bookBorrowService.SuperUserResponseRequestAsync(respondTestId,borrowRequestTestId,respond);
        
        Assert.IsInstanceOf<bool>(result);
        Assert.AreEqual(false,result);
    }

    [Test]
    public async Task Test_BookBorrowService_Respond_BorrowRequest_Return_False_WhenRespondIdIsInvalid()
    {
        var borrowRequestTestId = _bookBorrowTestList[0].Id;
        var respondTestId = Guid.NewGuid();
        var respond = "Approve";

        var result = await _bookBorrowService.SuperUserResponseRequestAsync(respondTestId,borrowRequestTestId,respond);
        
        Assert.IsInstanceOf<bool>(result);
        Assert.AreEqual(false,result);
    }
    [Test]
    public async Task Test_BookBorrowService_NormalUser_GetOwnRequests_ReturnPagingResult_WithOwnBorrowBooks()
    {
        var request = new PagingRequest(){
            PageSize = 3,
            PageIndex = 1,
        };
        var role = "NormalUser";
        var userTestId = _userTestList[1].Id;

        var result = await _bookBorrowService.GetBorrowRequestListAsync(role,userTestId,request);

        Assert.IsInstanceOf<List<BookBorrowingRequestModel>>(result.Items);
        Assert.AreEqual(2,result.Items.Count);
        Assert.AreEqual("Pham Ngoc Thang",result.Items[0].RequestedName);
        Assert.AreEqual(2, result.Items[0].BooksRequested.Count);
    }
    [Test]
    public async Task Test_BookBorrowService_SuperUser_GetRequests_ReturnPagingResult_OfAllUser_Include_TheirBorrowBooks()
    {
        var request = new PagingRequest(){
            PageSize = 3,
            PageIndex = 1,
        };
        var role = "SuperUser";
        var userTestId = _userTestList[1].Id;

        var result = await _bookBorrowService.GetBorrowRequestListAsync(role,userTestId,request);

        Assert.IsInstanceOf<List<BookBorrowingRequestModel>>(result.Items);
        Assert.AreEqual(3,result.Items.Count);
        Assert.AreEqual("Pham Ngoc Thang",result.Items[0].RequestedName);
        Assert.AreEqual("Pham Ngoc Dai",result.Items[2].RequestedName);
        Assert.AreEqual(2, result.Items[0].BooksRequested.Count);
    }
}