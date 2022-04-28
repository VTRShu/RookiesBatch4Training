namespace EFCore_Ex2.Repositories.DTO;

public class ProductDTO 
{   
    public Guid ProductId{get;set;}
    public string ProductName{get;set;}
    public string Manufacture{get;set;}
    public Guid CategoryId{get;set;}
}