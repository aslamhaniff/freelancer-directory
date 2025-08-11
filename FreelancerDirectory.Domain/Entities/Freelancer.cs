namespace FreelancerDirectory.Domain.Entities;

public class Freelancer
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsArchived { get; set; }

    public ICollection<Skillset> Skillsets { get; set; } = new List<Skillset>();
    public ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();
}