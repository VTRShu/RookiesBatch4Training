using System.Text.Json.Serialization;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;

namespace LibraryManagementBE.Repositories.DTOs
{
    public class AppUserDTO
    {   
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public DateTime Dob { get; set; }
        public Gender Gender { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public Role Type { get; set; }
        public bool IsDisabled { get; set; }
        public string? Password { get; set; }
    }
}
