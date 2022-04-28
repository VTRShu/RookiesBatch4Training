
using System.ComponentModel.DataAnnotations;

namespace  ASP.Net_Core_API_Exer.Models
{
    public class TaskModel{
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}