using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibraryManagementBE.Repositories.Enum;

namespace LibraryManagementBE.Repositories.DTOs;
public class BookBorrowingRequestDTO{
    [DataType (DataType.Date)]
    public DateTime? RequestedAt { get; set; }
    public Status? Status { get; set; }
    public List<Guid>? Books{ get; set;}
}