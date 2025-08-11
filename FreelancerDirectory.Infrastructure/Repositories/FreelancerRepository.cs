using FreelancerDirectory.Application.DTOs;
using FreelancerDirectory.Application.Interfaces;
using FreelancerDirectory.Domain.Entities;
using FreelancerDirectory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FreelancerDirectory.Infrastructure.Repositories
{
    public class FreelancerRepository : IFreelancerRepository
    {
        private readonly FreelancerDbContext _context;

        public FreelancerRepository(FreelancerDbContext context)
        {
            _context = context;
        }

        public async Task<List<FreelancerDto>> GetAllAsync()
        {
            return await _context.Freelancers
                .Include(f => f.Skillsets)
                .Include(f => f.Hobbies)
                .Where(f => !f.IsArchived)
                .Select(f => new FreelancerDto
                {
                    Id = f.Id,
                    Username = f.Username,
                    Email = f.Email,
                    PhoneNumber = f.PhoneNumber,
                    IsArchived = f.IsArchived,
                    Skillsets = f.Skillsets.Select(s => s.Name).ToList(),
                    Hobbies = f.Hobbies.Select(h => h.Name).ToList()
                })
                .ToListAsync();
        }


        public async Task<FreelancerDto?> GetByIdAsync(Guid id)
        {
            var f = await _context.Freelancers
                .Include(f => f.Skillsets)
                .Include(f => f.Hobbies)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (f == null) return null;

            return new FreelancerDto
            {
                Id = f.Id,
                Username = f.Username,
                Email = f.Email,
                PhoneNumber = f.PhoneNumber,
                IsArchived = f.IsArchived,
                Skillsets = f.Skillsets.Select(s => s.Name).ToList(),
                Hobbies = f.Hobbies.Select(h => h.Name).ToList()
            };
        }

        public async Task<FreelancerDto> CreateAsync(FreelancerDto dto)
        {
            var freelancer = new Freelancer
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Skillsets = dto.Skillsets.Select(s => new Skillset { Name = s }).ToList(),
                Hobbies = dto.Hobbies.Select(h => new Hobby { Name = h }).ToList()
            };

            _context.Freelancers.Add(freelancer);
            await _context.SaveChangesAsync();

            dto.Id = freelancer.Id;
            return dto;
        }

        public async Task<bool> UpdateAsync(FreelancerDto dto)
        {
            var freelancer = await _context.Freelancers
                .Include(f => f.Skillsets)
                .Include(f => f.Hobbies)
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (freelancer == null) return false;

            freelancer.Username = dto.Username;
            freelancer.Email = dto.Email;
            freelancer.PhoneNumber = dto.PhoneNumber;

            // ✅ Fix: Ensure IsArchived is updated
            freelancer.IsArchived = dto.IsArchived;

            // Remove old skills/hobbies
            _context.Skillsets.RemoveRange(freelancer.Skillsets);
            _context.Hobbies.RemoveRange(freelancer.Hobbies);

            // Add updated skills/hobbies
            freelancer.Skillsets = dto.Skillsets.Select(s => new Skillset { Name = s, FreelancerId = freelancer.Id }).ToList();
            freelancer.Hobbies = dto.Hobbies.Select(h => new Hobby { Name = h, FreelancerId = freelancer.Id }).ToList();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var freelancer = await _context.Freelancers.FindAsync(id);
            if (freelancer == null) return false;

            freelancer.IsArchived = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FreelancerDto>> SearchAsync(string query)
        {
            return await _context.Freelancers
                .Include(f => f.Skillsets)
                .Include(f => f.Hobbies)
                .Where(f => !f.IsArchived &&
                            (f.Username.Contains(query) || f.Email.Contains(query)))
                .Select(f => new FreelancerDto
                {
                    Id = f.Id,
                    Username = f.Username,
                    Email = f.Email,
                    PhoneNumber = f.PhoneNumber,
                    IsArchived = f.IsArchived,
                    Skillsets = f.Skillsets.Select(s => s.Name).ToList(),
                    Hobbies = f.Hobbies.Select(h => h.Name).ToList()
                })
                .ToListAsync();
        }
        
        
    }
}