using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBE.Repositories.Models
{
    public class BookModel
    {   
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CoverSrc { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}
