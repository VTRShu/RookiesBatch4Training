using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore_Ex.Repositories.Entities;

public class RookieDTO
{   
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get{return $"{FirstName} {LastName}";} }
    public string City { get; set; }
    public string State { get; set; }
    public string PhoneNumber { get; set; }
}