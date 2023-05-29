using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.ViewModels.Project
{
    public class ProjectTeamViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [MaxLength(150, ErrorMessage = "Максимальная длина составляет 150 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string GitUrl { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public DateTime DateEnd { get; set; }
        public int Attestation { get; set; }
        public int Status { get; set; }

        [Required(ErrorMessage = "Поле не может быть пустым")]
        public Guid ProjectOwnerId { get; set; }
        public string? ProjectOwnerName { get; set; }
    }
}
