using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementBE.Repositories.Entities;
public class BookBorrowingRequestDetails{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid? DetailOfRequestId { get; set; }
    public BookBorrowingRequest? DetailOfRequest { get; set; }
    public string? BookName { get; set; }
    public Guid? BookId { get; set; }
    public BookEntity? Book { get; set; }
}