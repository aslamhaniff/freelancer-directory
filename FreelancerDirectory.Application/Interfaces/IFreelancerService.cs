using FreelancerDirectory.Application.DTOs;

namespace FreelancerDirectory.Application.Interfaces
{
    public interface IFreelancerService
    {
        Task<List<FreelancerDto>> GetAllAsync(bool? archived);
        Task<FreelancerDto?> GetByIdAsync(Guid id);
        Task<FreelancerDto> CreateAsync(FreelancerDto dto);
        Task<bool> UpdateAsync(FreelancerDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<FreelancerDto>> SearchAsync(string query);
        Task<bool> PatchAsync(Guid id, FreelancerDto patchedDto);

    }
}
