using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            var authEnabled = _configuration.GetValue<bool>("AuthEnabled");
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
    /// Data access layer using modern configuration
    /// </summary>
    public class DataRepository
    {
        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration)
        {
            // Use configuration.GetConnectionString for connection strings
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
            await Task.Yield();

            cancellationToken.ThrowIfCancellationRequested();

            results.Add(new { Id = 1, Name = "Sample User" });

            return results;
        }

        public string SerializeData(object data)
        {
            // Use System.Text.Json for serialization
            // In .NET 9, System.Text.Json is preferred and supports more options.
            // For anonymous types, specify options if needed.
            return data != null ? JsonSerializer.Serialize(data, data.GetType(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }) : string.Empty;
        }

        public async Task SerializeDataAsync(Stream stream, object data, CancellationToken cancellationToken = default)
        {
            if (data == null) return;
            await JsonSerializer.SerializeAsync(stream, data, data.GetType(), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }, cancellationToken);
        }

        public async Task<T?> DeserializeDataAsync<T>(Stream stream, CancellationToken cancellationToken = default)
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }, cancellationToken);
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