using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore_Ex2.Repositories.Entities;

public class CategoryEntity 
{   
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CategoryId{get;set;}
    public string CategoryName{get;set;}
    public List<ProductEntity>? Products{get;set;}
}