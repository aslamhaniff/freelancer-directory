namespace FreelancerDirectory.Domain.Entities;

public class Skillset
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public Guid FreelancerId { get; set; }
    public Freelancer Freelancer { get; set; } = null!;
}

