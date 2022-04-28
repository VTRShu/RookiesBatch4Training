using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;

namespace LibraryManagementBE.Repositories.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
