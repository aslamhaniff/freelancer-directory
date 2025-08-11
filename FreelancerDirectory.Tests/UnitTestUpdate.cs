using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FreelancerDirectory.Application.DTOs;
using Xunit;
using Xunit.Abstractions;

namespace FreelancerDirectory.Tests
{
    public class UnitTestUpdate 
        : IClassFixture<CustomWebApplicationFactory<FreelancerDirectory.API.Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public UnitTestUpdate(CustomWebApplicationFactory<FreelancerDirectory.API.Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task UpdateFreelancer_ShouldReturnNoContent()
        {
            _output.WriteLine("=== UPDATE FREELANCER TEST ===");
            
            // First create the freelancer
            var dto = new FreelancerDto
            {
                Username = "jane_smith",
                Email = "jane@example.com",
                PhoneNumber = "9876543210",
                Skillsets = { "React", "Node.js" },
                Hobbies = { "Painting", "Cooking" }
            };

            _output.WriteLine($"Step 1: Creating freelancer {dto.Username}");
            var createResponse = await _client.PostAsJsonAsync("/api/freelancer", dto);
            _output.WriteLine($"Create Response Status: {createResponse.StatusCode} ({(int)createResponse.StatusCode})");
            
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await createResponse.Content.ReadFromJsonAsync<FreelancerDto>();
            created.Should().NotBeNull();
            _output.WriteLine($"Created freelancer with ID: {created!.Id}");

            // Modify and send back with correct Id
            created.Skillsets.Add("TypeScript");
            _output.WriteLine($"Step 2: Updating freelancer - Adding TypeScript skill");

            var updateResponse = await _client.PutAsJsonAsync("/api/freelancer", created);
            _output.WriteLine($"Update Response Status: {updateResponse.StatusCode} ({(int)updateResponse.StatusCode})");
            var updateContent = await updateResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Update Response Content: '{updateContent}' (should be empty for NoContent)");
            
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            
            _output.WriteLine("✅ UPDATE TEST PASSED - Freelancer updated successfully");
        }
    }
}