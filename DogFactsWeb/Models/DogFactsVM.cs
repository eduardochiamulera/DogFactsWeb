using System.ComponentModel.DataAnnotations;

namespace DogFactsWeb.Models
{
    public class DogFactsVM
    {
        public string? Url { get; set; }
        
        [Required]
        public IFormFile File { get; set; }
    }
}
