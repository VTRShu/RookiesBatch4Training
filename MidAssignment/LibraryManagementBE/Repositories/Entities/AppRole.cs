using Microsoft.AspNetCore.Identity;


namespace LibraryManagementBE.Repositories.Entities;

public class AppRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
}
