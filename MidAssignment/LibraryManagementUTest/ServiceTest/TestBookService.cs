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
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace LibraryManagementUTest;
public class TestBookService
{
    private LibraryManagementDBContext? _libraryMDBInMemoryContext;
    private IMapper? _mapper;
    private ShareSetupTest? _shareSetupTest;
    private IBookService? _bookService;
    private Mock<ILogger<BookService>>? _mockLogger;
    private List<BookEntity>? _bookTestList;
    private List<CategoryEntity>? _categoryTestList;

    [SetUp]
    public void SetUp(){
        _mockLogger = new Mock<ILogger<BookService>>();
        _shareSetupTest= new ShareSetupTest();
        _mapper = _shareSetupTest.MappingForTesting();
        _libraryMDBInMemoryContext = _shareSetupTest.InMemoryDatabaseSetup();
        _bookService = new BookService(_mockLogger.Object,_libraryMDBInMemoryContext,_mapper);
        _bookTestList = _libraryMDBInMemoryContext.BookEntity.ToList();
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
    public async Task Test_BookService_GetBooks_ReturnListBook_WithPaging()
    {  
        var request = new PagingRequest(){
            PageSize = 1,
            PageIndex = 1,
        };

        //action
        var result = await _bookService.GetBooksAsync(request);

        // Assert
        Assert.IsInstanceOf<PagingResult<BookDTO>>(result);
        Assert.IsInstanceOf<List<BookDTO>>(result.Items);
        Assert.AreEqual(request.PageSize, result.Items.Count);
        Assert.AreEqual(request.PageSize, result.PageIndex);
        Assert.AreEqual(_bookTestList[0].Id, result.Items[0].Id);
    }

    [Test]
    public async Task Test_BookService_CreateBook_ReturnASuccessBookDTO_And_SaveInDatabase_WhenInputAllValidFields()
    {
        var BookDto = new BookDTO(){
            Id = Guid.NewGuid(),
            Name = "Guilty Crown", 
            Description = "In the near future", 
            CategoryId =_categoryTestList[0].Id, 
            CoverSrc = "https://static.wikia.nocookie.net/guiltycrown/images/f/fb/Guilty_Crown_poster.jpg"
        };

        var result = await _bookService.CreateBookAsync(BookDto);
        var bookIMDatabase = _libraryMDBInMemoryContext.BookEntity.FirstOrDefault(x=>x.Id == BookDto.Id);

        Assert.IsInstanceOf<BookDTO>(result);
        Assert.AreEqual(BookDto.Id, result.Id);
        Assert.AreEqual(BookDto.Id,bookIMDatabase.Id);
        Assert.AreEqual(BookDto.Name,bookIMDatabase.Name);
        Assert.AreEqual(BookDto.Name,result.Name);
    }
    [Test]
    public async Task Test_BookService_CreateBook_ReturnNull_WhenCategoryDoestExist()
    {
        var BookDto = new BookDTO(){
            Id = Guid.NewGuid(),
            Name = "Guilty Crown", 
            Description = "In the near future", 
            CategoryId = Guid.NewGuid(), 
            CoverSrc = "https://static.wikia.nocookie.net/guiltycrown/images/f/fb/Guilty_Crown_poster.jpg"
        };
        var result = await _bookService.CreateBookAsync(BookDto);

        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_BookService_DeleteBook_ReturnTrue_And_SaveDatabase_WhenInputValidId()
    {
        var bookIdTest = _bookTestList[0].Id;

        var result = await _bookService.DeleteBookAsync(bookIdTest);

        Assert.IsInstanceOf<bool>(result);
        Assert.AreEqual(1,_libraryMDBInMemoryContext.BookEntity.ToList().Count);
        Assert.AreEqual(true,result);
    }
    [Test]
    public async Task Test_BookService_DeleteBook_ReturnFalse_When_BookDoesntExist()
    {
        var bookIdTest = Guid.NewGuid();

        var result = await _bookService.DeleteBookAsync(bookIdTest);

        Assert.AreEqual(2,_libraryMDBInMemoryContext.BookEntity.ToList().Count);
        Assert.AreEqual(false,result);
    }
    [Test]
    public async Task Test_BookService_GetBookById_ReturnSuccessBookDTO_WhenInputValidId()
    {
        var bookIdTest = _bookTestList[0].Id;

        var result = await _bookService.GetBookAsync(bookIdTest);

        Assert.IsInstanceOf<BookDTO>(result);
        Assert.AreEqual(bookIdTest, result.Id);
    }
    [Test]
    public async Task Test_BookService_GetBookById_ReturnNull_WhenBookDoesntExist()
    {
        var bookIdTest = Guid.NewGuid();

        var result = await _bookService.GetBookAsync(bookIdTest);

        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_BookService_EditBook_ReturnSuccessBook_And_SaveDatabase_WhenInputAllValid()
    {
        var bookIdTest = _bookTestList[0].Id;
        var request = new BookDTO(){
            Name = "Tsuki ga Michibiku Isekai Douchuu",
            CategoryId = _categoryTestList[0].Id,
            Description = "aaa",
            CoverSrc = "https://s199.imacdn.com/tt24/2021/04/09/cd3319db9a8fbc65_784a78034e63d855_3031741617979942845957.jpg"
        };

        var result = await _bookService.UpdateBookAsync(request,bookIdTest);
        
        Assert.IsInstanceOf<BookDTO>(result);
        Assert.AreEqual(bookIdTest, result.Id);
        Assert.AreEqual(request.Name, result.Name);
        Assert.AreEqual(request.Description, result.Description);
        Assert.AreEqual(request.CoverSrc,result.CoverSrc);
    }
    [Test]
    public async Task Test_BookService_EditBook_ReturnNull_WhenBookDoesntExist()
    {
        var bookIdTest = Guid.NewGuid();
        var request = new BookDTO(){
            Name = "Tsuki ga Michibiku Isekai Douchuu",
            CategoryId = _categoryTestList[0].Id,
            Description = "aaa",
            CoverSrc = "https://s199.imacdn.com/tt24/2021/04/09/cd3319db9a8fbc65_784a78034e63d855_3031741617979942845957.jpg"
        };
        
        var result = await _bookService.UpdateBookAsync(request,bookIdTest);

        VerifyLogger("Something went wrong!");
        Assert.IsNull(result);
    }
    [Test]
    public async Task Test_BookService_EditBook_ReturnNull_WhenCategoryDoesntExist()
    {
        var bookIdTest = _bookTestList[0].Id;
        var request = new BookDTO(){
            Name = "Tsuki ga Michibiku Isekai Douchuu",
            CategoryId = Guid.NewGuid(),
            Description = "aaa",
            CoverSrc = "https://s199.imacdn.com/tt24/2021/04/09/cd3319db9a8fbc65_784a78034e63d855_3031741617979942845957.jpg"
        };
        
        var result = await _bookService.UpdateBookAsync(request,bookIdTest);

        Assert.IsNull(result);
    }
}