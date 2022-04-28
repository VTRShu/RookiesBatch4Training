using System.ComponentModel.DataAnnotations;

namespace LibraryManagementBE.Repositories.Models;

public class CategoryModel
{   
    [Required]
    public string? CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
}

