using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore_Ex.Models;

public class RookieModel 
{   
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get{return $"{FirstName} {LastName}";} }
    public string City { get; set; }
    public string State { get; set; }
    [RegularExpression(@"^([0-9]{9,})$", ErrorMessage = "Number digit only!, at least 9")]
    public string PhoneNumber { get; set; }
}