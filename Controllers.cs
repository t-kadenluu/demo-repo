using System;
using System.IO;
using System.Text;
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
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public async Task<string> GetUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = new Microsoft.TestService.Models.User
            {
                Id = id,
                Name = "Test User",
                Email = "testuser@example.com",
                CreatedDate = DateTime.UtcNow
            };

            await Task.Yield(); // Ensure async context

            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, user, _jsonOptions, cancellationToken);
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, Encoding.UTF8, false, leaveOpen: true);
            var json = await reader.ReadToEndAsync(cancellationToken);
            return json;
        }

        public async Task<string> CreateUserAsync(Microsoft.TestService.Models.UserRequest userData, CancellationToken cancellationToken = default)
        {
            var user = new Microsoft.TestService.Models.User
            {
                Id = Random.Shared.Next(1, 10000),
                Name = userData.Name,
                Email = userData.Email,
                CreatedDate = DateTime.UtcNow
            };

            await Task.Yield(); // Ensure async context

            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, user, _jsonOptions, cancellationToken);
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, Encoding.UTF8, false, leaveOpen: true);
            var json = await reader.ReadToEndAsync(cancellationToken);
            return json;
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