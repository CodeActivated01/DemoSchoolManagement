using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? ImagePath { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string? Sex { get; set; }

        public ICollection<Subject>? Subjects { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
