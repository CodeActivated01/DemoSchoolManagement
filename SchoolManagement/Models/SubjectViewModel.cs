using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class SubjectViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Class { get; set; }

        public string? Language { get; set; }

        // List of selected Teacher IDs
        public IList<int> TeacherIds { get; set; } = new List<int>();

        // List of selected Student IDs
        public IList<int> StudentIds { get; set; } = new List<int>();

        // For use in views to display teacher and student names (optional)
        public IList<Teacher> Teachers { get; set; } = new List<Teacher>();
        public IList<Student> Students { get; set; } = new List<Student>();
    }
}
