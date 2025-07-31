using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.TestService.Auth
{
    /// <summary>
    /// Legacy authentication helper
    /// </summary>
    public static class AuthHelper
    {
        public static bool ValidateUser(string username, string password, IConfiguration configuration)
        {
            // Modern configuration logic using Microsoft.Extensions.Configuration
            // var configValue = configuration["AuthEnabled"];
            var configValue = Environment.GetEnvironmentVariable("AuthEnabled");
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
            // Use Microsoft.Extensions.Configuration for connection string
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<object> GetData()
        {
            var results = new List<object>();

            // Simulate data access
            results.Add(new { Id = 1, Name = "Sample User" });

            // Example of using CollectionsMarshal for high-performance access if needed
            // var span = CollectionsMarshal.AsSpan(results);

            return results;
        }

        public async Task<string> SerializeDataAsync(object data, CancellationToken cancellationToken = default)
        {
            // Use System.Text.Json for serialization
            if (data == null)
                return string.Empty;

            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, data, data.GetType(), cancellationToken: cancellationToken);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync(cancellationToken);
        }

        public async IAsyncEnumerable<object> GetDataAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var results = new List<object>
            {
                new { Id = 1, Name = "Sample User" }
            };

            foreach (var item in results)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
                await Task.Yield();
            }
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