using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;

namespace LibraryManagementBE.Repositories.DTOs
{
    public class BookDTO
    {   
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? CoverSrc { get; set; }
    }
}
