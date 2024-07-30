using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public int Age { get; set; }

        public string? ImagePath { get; set; }

        public string? Class { get; set; }

        [Required]
        public int RollNumber { get; set; }

        public ICollection<Subject>? Subjects { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
