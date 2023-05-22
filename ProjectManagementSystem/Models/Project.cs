using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Project
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string GitUrl { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public DateTime DateEnd { get; set; }
        public int Attestation { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
