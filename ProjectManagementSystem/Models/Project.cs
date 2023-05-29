using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class Project
    {
        public Guid Id { get; set; }

        [MaxLength(150, ErrorMessage = "Максимальная длина составляет 150 символов")]
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string GitUrl { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public DateTime DateEnd { get; set; }
        public int Attestation { get; set; }
        public int Status { get; set; }
        public Guid? ProjectOwnerId { get; set; }
        public string? ProjectOwnerName { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public User? User { get; set; }
    }
}
