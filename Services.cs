using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json; // Use System.Text.Json for serialization
using Microsoft.Extensions.Configuration; // Modern configuration
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.TestService.Auth
{
    /// <summary>
    /// Legacy authentication helper
    /// </summary>
    public static class AuthHelper
    {
        private static IConfiguration? _configuration;

        // Modern configuration initialization (singleton pattern)
        private static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    // Build configuration from appsettings.json and environment variables
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();
                }
                return _configuration;
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            // Use modern configuration instead of ConfigurationManager
            var configValue = Configuration["AuthEnabled"];
            // TODO: Implement actual authentication logic
            return !string.IsNullOrEmpty(username);
        }

        public static string GetCurrentUser()
        {
            // In ASP.NET Core, use IHttpContextAccessor for user context
            // For demo, return a placeholder
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

        public DataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Modern async pattern with cancellation support
        public async Task<List<object>> GetDataAsync(CancellationToken cancellationToken = default)
        {
            var results = new List<object>();

            // Simulate async data access
            await Task.Run(() =>
            {
                // Use object initializer and anonymous type
                results.Add(new { Id = 1, Name = "Sample User" });
            }, cancellationToken);

            return results;
        }

        // Modern serialization using System.Text.Json
        public string SerializeData(object? data)
        {
            if (data is null)
                return string.Empty;

            // Use System.Text.Json for cross-platform serialization
            return JsonSerializer.Serialize(data);
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
        // Consider making async for future extensibility
        Task<string> GetLegacyDataAsync(string id, CancellationToken cancellationToken = default);
    }

    public class LegacyService : ILegacyService
    {
        public Task<string> GetLegacyDataAsync(string id, CancellationToken cancellationToken = default)
        {
            // Simulate async operation
            return Task.FromResult($"Legacy data for ID: {id}");
        }
    }
}
