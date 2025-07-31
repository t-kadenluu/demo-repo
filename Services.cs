using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

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
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        public static bool ValidateUser(string username, string password)
        {
            // Legacy authentication logic
            var configValue = _configuration["AuthEnabled"];
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
            // Use Microsoft.Extensions.Configuration for connection string retrieval
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<object>> GetDataAsync(CancellationToken cancellationToken = default)
        {
            var results = new List<object>();

            // Simulate data access
            await Task.Delay(10, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            results.Add(new { Id = 1, Name = "Sample User" });

            return results;
        }

        public string SerializeData(object data)
        {
            return data != null ? JsonSerializer.Serialize(data) : string.Empty;
        }

        // Example: Use thread-safe collection for cross-platform compatibility
        private readonly ConcurrentDictionary<int, object> _cache = new();

        public void AddToCache(int key, object value)
        {
            _cache[key] = value;
        }

        public bool TryGetFromCache(int key, out object value)
        {
            return _cache.TryGetValue(key, out value);
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