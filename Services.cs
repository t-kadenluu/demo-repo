using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Microsoft.TestService.Auth
{
    /// <summary>
    /// Legacy authentication helper
    /// </summary>
    public class AuthOptions
    {
        public bool AuthEnabled { get; set; }
    }

    public static class AuthHelper
    {
        private static IConfiguration _configuration;
        private static IOptions<AuthOptions> _authOptions;

        static AuthHelper()
        {
            // In .NET 9, use dependency injection or build configuration as needed.
            // For demonstration, build a configuration from appsettings.json using cross-platform path.
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: true)
                .AddEnvironmentVariables();

            _configuration = configBuilder.Build();
            _authOptions = Options.Create(_configuration.Get<AuthOptions>());
        }

        public static bool ValidateUser(string username, string password)
        {
            // Use strongly-typed configuration binding via IOptions<AuthOptions>
            var authEnabled = _authOptions.Value.AuthEnabled;
            return !string.IsNullOrEmpty(username);
        }

        public static string GetCurrentUser()
        {
            // Simplified for demo - would normally check HttpContext
            return "DemoUser";
        }
    }
}

namespace Microsoft.TestService.Data
{
    /// <summary>
    /// Data access layer using modern .NET APIs
    /// </summary>
    public class DataRepository
    {
        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration)
        {
            // In .NET 9, get connection string from IConfiguration
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<object> GetData(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var results = new List<object>
            {
                new { Id = 1, Name = "Sample User" }
            };

            return results;
        }

        public async Task<List<object>> GetDataAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(10, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var results = new List<object>
            {
                new { Id = 1, Name = "Sample User" }
            };

            return results;
        }

        public async Task<string> SerializeDataAsync(object data, CancellationToken cancellationToken = default)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            if (data is null)
                return string.Empty;

            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, data, data.GetType(), options, cancellationToken).ConfigureAwait(false);
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }
    }
}

namespace Microsoft.TestService.Services
{
    /// <summary>
    /// Service for legacy operations
    /// </summary>
    public interface ILegacyService
    {
        string GetLegacyData(string id);
    }

    public class LegacyService : ILegacyService
    {
        public string GetLegacyData(string id)
        {
            return $"Legacy data for ID: {id}";
        }
    }
}