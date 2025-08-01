using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.TestService.Controllers
{
    /// <summary>
    /// Controller for handling user operations
    /// </summary>
    public class UserController
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Explicitly specify encoder for cross-platform unicode handling
        };

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
            // Serialize using System.Text.Json for .NET 9.0 with recommended options
            return JsonSerializer.Serialize<Models.User>(user, JsonOptions);
        }

        public async Task<string> CreateUserAsync(Models.UserRequest userData, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Simulate user creation
            // Serialize the request data for logging or processing using recommended options
            var serializedData = JsonSerializer.Serialize<Models.UserRequest>(userData, JsonOptions);
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