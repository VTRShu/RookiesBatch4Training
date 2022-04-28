using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore_Ex2.Repositories.Entities;

public class ProductEntity 
{   
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProductId{get;set;}
    public string ProductName{get;set;}
    public string Manufacture{get;set;}
    public Guid? CategoryId{get;set;}
    public CategoryEntity? Category{get;set;}
}