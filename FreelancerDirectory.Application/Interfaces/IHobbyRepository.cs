using FreelancerDirectory.Application.DTOs;

namespace FreelancerDirectory.Application.Interfaces
{
    public interface IHobbyRepository
    {
        Task<List<HobbyDto>> GetAllAsync();
    }
}
