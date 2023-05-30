using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Tasks
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public int Priorety { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public Guid? UserId { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
