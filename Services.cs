using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static bool ValidateUser(string username, string password)
        {
            // Updated configuration access
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
            if (cancellationToken.IsCancellationRequested)
                return results;

            results.Add(new { Id = 1, Name = "Sample User" });

            return results;
        }

        public async Task<List<object>> GetDataAsync(CancellationToken cancellationToken = default)
        {
            var results = new List<object>();

            // Simulate async data access
            await Task.Delay(10, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return results;

            results.Add(new { Id = 1, Name = "Sample User" });

            return results;
        }

        public string SerializeData(object data)
        {
            // Use System.Text.Json for serialization in .NET 9.0
            return data is null ? string.Empty : JsonSerializer.Serialize(data);
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