using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibraryManagementBE.Repositories.Enum;

namespace LibraryManagementBE.Repositories.Entities;
public class BookBorrowingRequest{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid? RequestedById { get; set; }
    public AppUser? RequestedBy { get; set; }
    [DataType (DataType.Date)]
    public DateTime? RequestedAt { get; set; }
    public Status? Status { get; set; }
    public List<BookBorrowingRequestDetails>? BooksRequested{ get; set;}
    public Guid? ResponseById { get; set; }
    public AppUser? ResponseBy { get; set; }
    public DateTime? ResponseAt { get; set; }
}