using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.TestService.Controllers
{
    /// <summary>
    /// Controller for handling user operations
    /// </summary>
    public class UserController
    {
        public async Task<string> GetUserAsync(int id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = new Microsoft.TestService.Models.User
            {
                Id = id,
                Name = "Test User",
                Email = "testuser@example.com",
                CreatedDate = DateTime.UtcNow
            };
            // Use System.Text.Json for serialization in .NET 9.0
            await Task.Yield(); // Ensure async context
            return JsonSerializer.Serialize(user);
        }

        public async Task<string> CreateUserAsync(Microsoft.TestService.Models.UserRequest userData, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Simulate user creation
            var user = new Microsoft.TestService.Models.User
            {
                Id = Random.Shared.Next(1, 10000),
                Name = userData.Name,
                Email = userData.Email,
                CreatedDate = DateTime.UtcNow
            };
            // Use System.Text.Json for serialization in .NET 9.0
            await Task.Yield(); // Ensure async context
            return JsonSerializer.Serialize(user);
        }
    }
}

namespace Microsoft.TestService.Models
{
    /// <summary>
    /// Data models for the application
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class UserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}