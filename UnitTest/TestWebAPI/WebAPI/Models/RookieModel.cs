using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebAPI.Models.Enums;

namespace WebAPI.Models
{   

    public class RookieModel
    {
        public int RookieId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName { get { return $"{FirstName} {LastName}"; }}
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid!")]
        public string Email { get; set; }
        // [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}",ApplyFormatInEditMode = true)]
        public DateTime DoB { get; set; }
        [Required]
        [RegularExpression(@"^([0-9]{9,})$", ErrorMessage = "Number digit only!, at least 9")]
        public string PhoneNumber { get; set; }
        public string Age
        {
            get
            {
                DateTime now = DateTime.Now;
                int age = now.Year - DoB.Year;
                if (DoB > now.AddYears(-age))
                {
                    age--;
                };
                return age.ToString();
            }
        }
        [Required]
        public string BirthPlace { get; set; }
        public Boolean Graduated { get; set; }
    }
}