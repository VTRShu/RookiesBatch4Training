using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementBE.Repositories.DTOs;
public class BookBorrowingRequestDetailsDTO{
    public Guid? DetailOfRequestId { get; set; }
    public Guid? BookId { get; set; }
}