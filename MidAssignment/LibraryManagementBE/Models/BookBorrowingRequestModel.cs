using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;

namespace LibraryManagementBE.Repositories.Models;
public class BookBorrowingRequestModel{
    public Guid Id { get; set; }
    public string? RequestedName { get; set; }
    [DataType (DataType.Date)]
    public DateTime? RequestedAt { get; set; }
    public Status? Status { get; set; }
    public List<string>? BooksRequested{ get; set;}
    public string? ResponseByName { get; set; }
    [DataType (DataType.Date)]
    public DateTime? ResponseAt { get; set; }
}