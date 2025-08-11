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
    public class UnitTestSearch 
        : IClassFixture<CustomWebApplicationFactory<FreelancerDirectory.API.Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public UnitTestSearch(CustomWebApplicationFactory<FreelancerDirectory.API.Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task SearchFreelancers_ShouldReturnMatchingResults()
        {
            _output.WriteLine("=== SEARCH FREELANCERS TEST ===");
            
            // Create freelancers
            var dto1 = new FreelancerDto
            {
                Username = "alice_java_dev",  // Put "java" in username so search can find it
                Email = "alice@example.com",
                PhoneNumber = "5551112222",
                Skillsets = { "Java", "Spring Boot" },
                Hobbies = { "Hiking" }
            };

            var dto2 = new FreelancerDto
            {
                Username = "bob_python_dev",
                Email = "bob@example.com", 
                PhoneNumber = "5553334444",
                Skillsets = { "Python", "Django" },
                Hobbies = { "Gaming" }
            };

            _output.WriteLine("Step 1: Creating two freelancers");
            var resp1 = await _client.PostAsJsonAsync("/api/freelancer", dto1);
            _output.WriteLine($"Create Alice Response: {resp1.StatusCode} ({(int)resp1.StatusCode})");
            resp1.StatusCode.Should().Be(HttpStatusCode.Created);

            var resp2 = await _client.PostAsJsonAsync("/api/freelancer", dto2);
            _output.WriteLine($"Create Bob Response: {resp2.StatusCode} ({(int)resp2.StatusCode})");
            resp2.StatusCode.Should().Be(HttpStatusCode.Created);

            // Test searching by username (should find alice_java_dev)
            _output.WriteLine("Step 2: Searching for 'java'");
            var searchResponse = await _client.GetAsync("/api/freelancer/search?query=java");
            _output.WriteLine($"Search Response Status: {searchResponse.StatusCode} ({(int)searchResponse.StatusCode})");
            
            var searchContent = await searchResponse.Content.ReadAsStringAsync();
            _output.WriteLine($"Search Response Content: {searchContent}");
            
            searchResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var results = await searchResponse.Content.ReadFromJsonAsync<FreelancerDto[]>();
            results.Should().NotBeNull();
            results!.Should().ContainSingle(f => f.Username.Contains("java"));
            
            _output.WriteLine($"✅ SEARCH TEST PASSED - Found {results.Length} result(s)");
            if (results.Length > 0)
            {
                _output.WriteLine($"Found freelancer: {results[0].Username}");
            }
        }
    }
}