namespace ProjectManagementSystem.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public string Description { get; set; }
        public string GitUrl { get; set; }
        public DateTime DateEnd { get; set; }
        public int Attestation { get; set; }
    }
}
