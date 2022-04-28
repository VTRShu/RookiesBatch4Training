using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.Requests;

namespace LibraryManagementBE.Services;
public interface IBookService{
    Task<PagingResult<BookDTO>> GetBooksAsync(PagingRequest request);
    Task<BookDTO> CreateBookAsync(BookDTO book);
    Task<BookDTO> UpdateBookAsync(BookDTO book, Guid id);
    Task<bool> DeleteBookAsync(Guid id);
    Task<BookDTO> GetBookAsync(Guid id);
    Task<List<BookDTO>> GetAllBookAsync();
}