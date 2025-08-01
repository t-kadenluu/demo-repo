using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Explicitly specify encoder for cross-platform unicode handling
            Converters = { new JsonStringEnumConverter() } // Ensure enums are serialized as strings
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
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, user, JsonOptions, cancellationToken);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(cancellationToken);
        }

        public async Task<string> CreateUserAsync(Models.UserRequest userData, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Simulate user creation
            // Serialize the request data for logging or processing using recommended options
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, userData, JsonOptions, cancellationToken);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            var serializedData = await reader.ReadToEndAsync(cancellationToken);
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
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }
    }

    public class UserRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}