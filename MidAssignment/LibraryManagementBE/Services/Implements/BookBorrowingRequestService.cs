using System.Globalization;
using System.Linq;
using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using LibraryManagementBE.Repositories.Models;
using LibraryManagementBE.Repositories.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBE.Services.Implements;
public class BookBorrowingRequestService : IBookBorrowingRequestService
{   
    private readonly ILogger<BookBorrowingRequestService> _logger;
    private readonly LibraryManagementDBContext _libraryManagementDBContext;
    private readonly IMapper _mapper;
    public BookBorrowingRequestService(ILogger<BookBorrowingRequestService> logger, LibraryManagementDBContext libraryManagementDBContext, IMapper mapper)
    {
        _logger = logger;
        _libraryManagementDBContext = libraryManagementDBContext;
        _mapper = mapper;
    }
    public AppUser GetUserById(Guid? id) => _libraryManagementDBContext.AppUser.FirstOrDefault(x=>x.Id == id && x.IsDisabled == false);
    public BookEntity GetBookById(Guid? id) => _libraryManagementDBContext.BookEntity.FirstOrDefault(x=>x.Id == id);

    public async Task<string> CreateBorrowRequestAsync(Guid requestedId,BookBorrowingRequestDTO borrowingRequest)
    {   
        using var transaction = _libraryManagementDBContext.Database.BeginTransaction();
        try{
            var existRequestedUser = GetUserById(requestedId);
            List<BookBorrowingRequestDetails> listBorrowDetail = new List<BookBorrowingRequestDetails>();
            if(existRequestedUser != null)
            {      
                // if(DateTime.Now.DayOfWeek == DayOfWeek.Sunday) return "No service in Sunday!";
                var newBorrowingRequest = new BookBorrowingRequest(){
                    RequestedAt = DateTime.Now,
                    ResponseById = null,
                    RequestedById = requestedId,
                    Status = (Status)2
                };

                var countRequestInMonth = _libraryManagementDBContext.BookBorrowingRequest.Where(x=> x.RequestedAt.Value.Month == DateTime.Now.Month 
                && x.RequestedById == requestedId
                && x.Status != (Status)1).ToList();
                if(countRequestInMonth.Count  >= 3)
                {
                    return "You can only make 3 request per month, pls wait for the next month!";
                }
                if(borrowingRequest.Books.Count > 5)
                {
                    return "You have react the limit of book request!(Max 5)";
                }
                foreach(var book in borrowingRequest.Books)
                {   
                    var newBorrowingRequestDetail = new BookBorrowingRequestDetails();
                    var getBook = GetBookById(book);
                    if(getBook != null)
                    {
                        newBorrowingRequestDetail = new BookBorrowingRequestDetails()
                        {
                            DetailOfRequestId = newBorrowingRequest.Id,
                            BookId = book,
                            BookName = getBook.Name,
                        };
                    }else{
                        return "One of your books you requested doesnot exist!";
                    }
                    await _libraryManagementDBContext.BookBorrowingRequestDetails.AddAsync(newBorrowingRequestDetail);
                    listBorrowDetail.Add(newBorrowingRequestDetail);
                }
                newBorrowingRequest.BooksRequested = listBorrowDetail;
                await _libraryManagementDBContext.BookBorrowingRequest.AddAsync(newBorrowingRequest);
                await _libraryManagementDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Create request successfully!";
            }
            return "Request User does not exist!";
        }catch(Exception e)
        {
            _logger.LogError("Something went wrong");
        }
        return "Something went wrong!";
    }

    public async Task<bool> SuperUserResponseRequestAsync(Guid responseId, Guid requestId, string respond)
    {
        using var transaction = _libraryManagementDBContext.Database.BeginTransaction();
        try{
            var existBorrowRequest = _libraryManagementDBContext.BookBorrowingRequest.FirstOrDefault(x=> x.Id == requestId);
            var responseUser = GetUserById(responseId);
            if(responseUser == null)
            {
                return false;
            }
            if(existBorrowRequest == null)
            {
                return false;
            }if(existBorrowRequest.Status == (Status)2 && respond == "Approve")
            {
                existBorrowRequest.Status = (Status)0;
                existBorrowRequest.ResponseById = responseId;
                existBorrowRequest.ResponseAt = DateTime.Now;
                _libraryManagementDBContext.Entry(existBorrowRequest).State = EntityState.Modified;
                await _libraryManagementDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            if(existBorrowRequest.Status == (Status)2 && respond == "Reject")
            {
                existBorrowRequest.Status = (Status)1;
                existBorrowRequest.ResponseById = responseId;
                existBorrowRequest.ResponseAt = DateTime.Now;
                _libraryManagementDBContext.Entry(existBorrowRequest).State = EntityState.Modified;
                await _libraryManagementDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            return false;
        }catch(Exception e)
        {
            _logger.LogError("Something went wrong!");
        }
        return false;
    }
    public async Task<PagingResult<BookBorrowingRequestModel>> GetBorrowRequestListAsync(string role,Guid userId,PagingRequest request)
    {   
        int total = 0;
        List<BookBorrowingRequestModel> data = new List<BookBorrowingRequestModel>();
        if(role == "SuperUser")
        {
            var requests = _libraryManagementDBContext.BookBorrowingRequest.Include(x=>x.BooksRequested);
            total = await requests.CountAsync();
            var listBookName = new List<string>();
            var requestList = requests.ToList();
            foreach(var rq in requestList)
            {
                foreach(var books in rq?.BooksRequested)
                {
                var book = GetBookById(books.BookId);
                listBookName.Add(book?.Name);
                }
            };
            data = await requests.Skip((request.PageIndex-1)* request.PageSize).Take(request.PageSize)
            .Select(x=> new BookBorrowingRequestModel(){
                Id = x.Id,
                RequestedAt = x.RequestedAt,
                RequestedName = x.RequestedBy.FullName,
                Status = x.Status,
                BooksRequested = new List<string>( x.BooksRequested.Select(x=>x.BookName).ToList()) ,
                ResponseByName = x.ResponseBy.FullName,
                ResponseAt = x.ResponseAt
            }).ToListAsync();
        }
        if(role == "NormalUser")
        {
            var requests = _libraryManagementDBContext.BookBorrowingRequest.Where(x=>x.RequestedById == userId).Include(x=>x.BooksRequested);
            total = await requests.CountAsync();
            var listBookName = new List<string>();
            var requestList = requests.ToList();
            foreach(var rq in requestList)
            {
                foreach(var books in rq?.BooksRequested)
                {
                var book = GetBookById(books.BookId);
                listBookName.Add(book?.Name);
                }
            };
            data = await requests.Skip((request.PageIndex-1)* request.PageSize).Take(request.PageSize)
            .Select(x=> new BookBorrowingRequestModel(){
                Id = x.Id,
                RequestedAt = x.RequestedAt,
                RequestedName = x.RequestedBy.FullName,
                Status = x.Status,
                BooksRequested = new List<string>( x.BooksRequested.Select(x=>x.BookName).ToList()) ,
                ResponseByName = x.ResponseBy.FullName,
                ResponseAt = x.ResponseAt
            }).ToListAsync();
        }
        var pageResult = new PagingResult<BookBorrowingRequestModel>()
        {
            Items = data,
            TotalRecords = total,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex
        };
        return pageResult;
    }
}