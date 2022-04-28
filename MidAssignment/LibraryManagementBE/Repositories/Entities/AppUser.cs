using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryManagementBE.Repositories.Enum;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementBE.Repositories.Entities;
public class AppUser : IdentityUser<Guid> {
    public string? FullName { get; set; }
    [DataType (DataType.Date)]
    public DateTime Dob { get; set; }
    public Gender Gender{ get; set;}
    public Role Type { get; set; }
    public bool IsDisabled { get; set; }
    public List<BookBorrowingRequest>? OwnRequested{ get; set;}

}