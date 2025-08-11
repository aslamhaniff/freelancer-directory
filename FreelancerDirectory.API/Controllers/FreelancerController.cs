using FreelancerDirectory.Application.DTOs;
using FreelancerDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace FreelancerDirectory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FreelancerController : ControllerBase
    {
        private readonly IFreelancerRepository _freelancerRepository;

        public FreelancerController(IFreelancerRepository freelancerRepository)
        {
            _freelancerRepository = freelancerRepository;
        }

        // GET: api/freelancer
        [HttpGet]
        public async Task<ActionResult<List<FreelancerDto>>> GetAll()
        {
            var freelancers = await _freelancerRepository.GetAllAsync();
            return Ok(freelancers);
        }

        // GET: api/freelancer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FreelancerDto>> GetById(Guid id)
        {
            var freelancer = await _freelancerRepository.GetByIdAsync(id);
            if (freelancer == null)
                return NotFound();

            return Ok(freelancer);
        }

        // POST: api/freelancer
        [HttpPost]
        public async Task<ActionResult<FreelancerDto>> Create([FromBody] FreelancerDto dto)
        {
            var created = await _freelancerRepository.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/freelancer
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FreelancerDto dto)
        {
            var updated = await _freelancerRepository.UpdateAsync(dto);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/freelancer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _freelancerRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // PATCH: api/freelancer/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<FreelancerDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var freelancer = await _freelancerRepository.GetByIdAsync(id);

            if (freelancer == null)
                return NotFound();

            patchDoc.ApplyTo(freelancer, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _freelancerRepository.UpdateAsync(freelancer); // reuse existing method

            if (!updated)
                return StatusCode(500, "Error updating freelancer");

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query cannot be empty.");

            var results = await _freelancerRepository.SearchAsync(query);
            return Ok(results);
        }

    }
}