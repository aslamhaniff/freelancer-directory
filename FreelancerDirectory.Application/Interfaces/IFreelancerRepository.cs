using FreelancerDirectory.Application.DTOs;

namespace FreelancerDirectory.Application.Interfaces
{
    public interface IFreelancerRepository
    {
        Task<List<FreelancerDto>> GetAllAsync();
        Task<FreelancerDto?> GetByIdAsync(Guid id); // ← Changed from int to Guid
        Task<FreelancerDto> CreateAsync(FreelancerDto freelancerDto);
        Task<bool> UpdateAsync(FreelancerDto freelancerDto);
        Task<bool> DeleteAsync(Guid id); // ← Changed from int to Guid
        Task<List<FreelancerDto>> SearchAsync(string query);

    }
}
