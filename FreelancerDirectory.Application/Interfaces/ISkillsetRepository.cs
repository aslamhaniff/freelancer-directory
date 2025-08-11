using FreelancerDirectory.Application.DTOs;

namespace FreelancerDirectory.Application.Interfaces
{
    public interface ISkillsetRepository
    {
        Task<List<SkillsetDto>> GetAllAsync();
    }
}
