using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Microsoft.TestService.Auth
{
    /// <summary>
    /// Legacy authentication helper
    /// </summary>
    public static class AuthHelper
    {
        private static IConfiguration _configuration;

        static AuthHelper()
        {
            // Build configuration from appsettings.json or environment variables
            var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(configPath, optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static bool ValidateUser(string username, string password)
        {
            // Updated configuration access using strongly typed GetValue
            var configValue = _configuration.GetValue<bool>("AuthEnabled");
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
    /// Data access layer using legacy ADO.NET
    /// </summary>
    public class DataRepository
    {
        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration)
        {
            // Use Microsoft.Extensions.Configuration for connection strings
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<object> GetData(CancellationToken cancellationToken = default)
        {
            var results = new List<object>();

            // Simulate data access
            cancellationToken.ThrowIfCancellationRequested();

            results.Add(new { Id = 1, Name = "Sample User" });

            return results;
        }

        public async Task<List<object>> GetDataAsync(CancellationToken cancellationToken = default)
        {
            var results = new List<object>();

            // Simulate async data access
            await Task.Delay(10, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            results.Add(new { Id = 1, Name = "Sample User" });

            return results;
        }

        public string SerializeData<T>(T data)
        {
            // Use System.Text.Json for serialization in .NET 9.0
            if (data is null)
                return string.Empty;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Explicitly specify encoder for cross-platform unicode handling
            };

            return JsonSerializer.Serialize<T>(data, options);
        }

        public async Task SerializeDataAsync<T>(T data, Stream stream, CancellationToken cancellationToken = default)
        {
            if (data is null || stream is null)
                return;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = false,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            await JsonSerializer.SerializeAsync<T>(stream, data, options, cancellationToken).ConfigureAwait(false);
        }

        public async Task<T?> DeserializeDataAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream is null)
                return default;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            return await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken).ConfigureAwait(false);
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