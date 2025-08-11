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
    public class UnitTestCreate 
        : IClassFixture<CustomWebApplicationFactory<FreelancerDirectory.API.Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public UnitTestCreate(CustomWebApplicationFactory<FreelancerDirectory.API.Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task CreateFreelancer_ShouldReturnCreatedFreelancer()
        {
            _output.WriteLine("=== CREATE FREELANCER TEST ===");
            
            var dto = new FreelancerDto
            {
                Username = "john_doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890",
                Skillsets = { "C#", "ASP.NET Core" },
                Hobbies = { "Cycling", "Reading" }
            };

            _output.WriteLine($"Sending POST request to create freelancer: {dto.Username}");
            var response = await _client.PostAsJsonAsync("/api/freelancer", dto);
            
            _output.WriteLine($"Response Status: {response.StatusCode} ({(int)response.StatusCode})");
            var responseContent = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"Response Content: {responseContent}");

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<FreelancerDto>();
            created.Should().NotBeNull();
            created!.Username.Should().Be("john_doe");
            
            _output.WriteLine($"✅ CREATE TEST PASSED - Created freelancer with ID: {created.Id}");
        }
    }
}