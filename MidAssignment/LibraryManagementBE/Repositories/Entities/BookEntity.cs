using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementBE.Repositories.Entities;
public class BookEntity {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    [DataType (DataType.Date)]
    public DateTime? PublishedAt { get; set; }
    public string? CoverSrc { get; set; }

}