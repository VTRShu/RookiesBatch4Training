

using LibraryManagementBE.Repositories.Enum;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementBE.Repositories.Requests
{
    public class FilterRequest
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
        [FromQuery(Name = "email")]
        public string? Email { get; set; }
        [FromQuery(Name = "gender")]
        public Gender? Gender { get; set; }
        [FromQuery(Name = "role")]
        public Role? Role { get; set; }
    }
}