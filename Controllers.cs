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

            var user = new Models.User
            {
                Id = id,
                Name = "Test User",
                Email = "testuser@example.com",
                CreatedDate = DateTime.UtcNow
            };
            // Serialize using System.Text.Json for .NET 9.0
            return await Task.Run(() => JsonSerializer.Serialize(user), cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> CreateUserAsync(Models.UserRequest userData, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Simulate user creation
            // Serialize the request data for logging or processing
            var serializedData = await Task.Run(() => JsonSerializer.Serialize(userData), cancellationToken).ConfigureAwait(false);
            return "User created successfully";
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