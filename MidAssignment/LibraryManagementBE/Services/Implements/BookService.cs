using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using LibraryManagementBE.Repositories.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBE.Services.Implements;
public class BookService : IBookService
{   
    private readonly ILogger<BookService> _logger;
    private readonly LibraryManagementDBContext _libraryManagementDBContext;
    private readonly IMapper _mapper;
    public BookService(ILogger<BookService> logger, LibraryManagementDBContext libraryManagementDBContext, IMapper mapper)
    {
        _logger = logger;
        _libraryManagementDBContext = libraryManagementDBContext;
        _mapper = mapper;
    }
    public BookEntity GetBookById(Guid id) => _libraryManagementDBContext.BookEntity.FirstOrDefault(x => x.Id == id);
    public async Task<BookDTO> CreateBookAsync(BookDTO book)
    {
        BookDTO result = null;
        using var transaction = _libraryManagementDBContext.Database.BeginTransaction();
        try{
            var existedCategory = _libraryManagementDBContext.CategoryEntity.FirstOrDefault(x=>x.Id == book.CategoryId);
            if(existedCategory != null)
            {
                var duplicateBook = _libraryManagementDBContext.BookEntity.FirstOrDefault(x=>x.Name == book.Name);
                if(duplicateBook == null)
                {
                    book.PublishedAt = DateTime.Now;
                    var newBook = _mapper.Map<BookEntity>(book);
                    await _libraryManagementDBContext.BookEntity.AddAsync(newBook);
                    await _libraryManagementDBContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    result = _mapper.Map<BookDTO>(newBook);
                    return result;
                }
                return result;
            }
            return result;
        }catch(Exception e)
        {
            _logger.LogError("Something went wrong!");
        }
        return result;
    }

    public async Task<bool> DeleteBookAsync(Guid id)
    {
        var existedBook = GetBookById(id);
        if(existedBook != null)
        {
            _libraryManagementDBContext.BookEntity.Remove(existedBook);
            await _libraryManagementDBContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<BookDTO> GetBookAsync(Guid id)
    {
         var existedBook = GetBookById(id);
        if(existedBook != null)
        {
            return await Task.FromResult(_mapper.Map<BookDTO>(existedBook));
        }
        return null;
    }

    public async Task<PagingResult<BookDTO>> GetBooksAsync(PagingRequest request)
    {
        var books = _libraryManagementDBContext.BookEntity.Select(x=>_mapper.Map<BookDTO>(x));
        int total = await books.CountAsync();
        var data = await books.Skip((request.PageIndex-1)* request.PageSize).Take(request.PageSize)
        .Select(x=> _mapper.Map<BookDTO>(x)).ToListAsync();
        var pageResult = new PagingResult<BookDTO>()
        {
            Items = data,
            TotalRecords = total,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex
        };
        return pageResult;
    }   

    public async Task<BookDTO> UpdateBookAsync(BookDTO book, Guid id)
    {
        BookDTO result = null;
        using var transaction = _libraryManagementDBContext.Database.BeginTransaction();
        try{
            var existedBook = GetBookById(id);
            var existedCategory = _libraryManagementDBContext.CategoryEntity.FirstOrDefault(x=>x.Id == book.CategoryId);
            if(existedCategory != null)
            {
                existedBook.CoverSrc = book.CoverSrc;
                existedBook.Name = book.Name;
                existedBook.CategoryId = book.CategoryId;
                existedBook.Description = book.Description;
                _libraryManagementDBContext.Entry(existedBook).State = EntityState.Modified;
                await _libraryManagementDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                result = _mapper.Map<BookDTO>(existedBook);
                return result;
            }
            return result;
        }catch(Exception e)
        {
            _logger.LogError("Something went wrong!");
        }
        return result;
    }

    public async Task<List<BookDTO>> GetAllBookAsync()
    {
        return await _libraryManagementDBContext.BookEntity.Select(x=> _mapper.Map<BookDTO>(x)).ToListAsync();
    }
 
}