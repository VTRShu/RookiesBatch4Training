using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementBE.Repositories.Entities;
public class CategoryEntity {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string? CategoryName { get; set; }
    public List<BookEntity>? Books { get; set; }
    [DataType (DataType.Date)]
    public DateTime CreatedAt { get; set; }
}