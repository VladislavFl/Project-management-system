namespace ProjectManagementSystem.Models
{
    public class Tasks
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Priorety { get; set; }
        public string Description { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
