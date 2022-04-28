
using System.ComponentModel.DataAnnotations;
using WebAPI.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Request
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