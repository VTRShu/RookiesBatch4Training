using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Models;
using LibraryManagementBE.Repositories.Requests;

namespace LibraryManagementBE.Services;
public interface IBookBorrowingRequestService{
    Task<string> CreateBorrowRequestAsync(Guid requestedId,BookBorrowingRequestDTO borrowingRequest);
    Task<bool> SuperUserResponseRequestAsync(Guid responseId, Guid requestId, string respond);
    Task<PagingResult<BookBorrowingRequestModel>> GetBorrowRequestListAsync(string role,Guid userId,PagingRequest request);
}