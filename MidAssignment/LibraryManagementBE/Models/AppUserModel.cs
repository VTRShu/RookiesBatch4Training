using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using LibraryManagementBE.Repositories.Enum;

namespace LibraryManagementBE.Repositories.Models
{
    public class AppUserModel
    {   
        [Required]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        public Gender Gender { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid!")]
        public string Email { get; set; }
        public Role Type { get; set; }
        public bool IsDisabled { get; set; }
        public string? Password { get; set; }
    }
}
