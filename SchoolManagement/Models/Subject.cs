using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Class { get; set; }

        public string? Language { get; set; }

        public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public ICollection<Student> Students { get; set; } = new List<Student>();

    }

}
