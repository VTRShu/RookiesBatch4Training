using EFCore_Ex2.Repositories.Entities;

namespace EFCore_Ex2.Repositories.DTO;

public class CategoryDTO 
{   
    public Guid CategoryId { get; set; }
    public string CategoryName{get;set;}
}