
using System.ComponentModel.DataAnnotations;
using ASP.Net_Core_API_Exer.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_Core_API_Exer.Request
{
    public class FilterRequest
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
        [FromQuery(Name = "birthplace")]
        public string? BirthPlace { get; set; }
        [FromQuery(Name = "gender")]
        public Gender? Gender { get; set; }
    }
}