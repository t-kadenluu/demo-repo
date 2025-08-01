using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
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
            await Task.Delay(10, cancellationToken).ConfigureAwait(false);

            cancellationToken.ThrowIfCancellationRequested();

            results.Add(new { Id = 1, Name = "Sample User" });

            return results;
        }

        public async Task<string> SerializeDataAsync<T>(T data, CancellationToken cancellationToken = default)
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

#if NET9_0_OR_GREATER
            var context = JsonContext.Default;
            using var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, data, data?.GetType() ?? typeof(object), context, options, cancellationToken).ConfigureAwait(false);
            ms.Position = 0;
            using var reader = new StreamReader(ms);
            return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
#else
            using var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, data, options, cancellationToken).ConfigureAwait(false);
            ms.Position = 0;
            using var reader = new StreamReader(ms);
            return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
#endif
        }

        public async Task SerializeDataToStreamAsync<T>(T data, Stream stream, CancellationToken cancellationToken = default)
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

#if NET9_0_OR_GREATER
            var context = JsonContext.Default;
            await JsonSerializer.SerializeAsync(stream, data, data?.GetType() ?? typeof(object), context, options, cancellationToken).ConfigureAwait(false);
#else
            await JsonSerializer.SerializeAsync(stream, data, options, cancellationToken).ConfigureAwait(false);
#endif
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

#if NET9_0_OR_GREATER
            var context = JsonContext.Default;
            return await JsonSerializer.DeserializeAsync<T>(stream, context, options, cancellationToken).ConfigureAwait(false);
#else
            return await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken).ConfigureAwait(false);
#endif
        }

        // Example: Parse JSON using System.Text.Json.Nodes for cross-platform compatibility
        public async Task<JsonNode?> ParseJsonNodeAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            if (stream is null)
                return null;

            // System.Text.Json.Nodes is cross-platform in .NET 9.0
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            return JsonNode.Parse(json, documentOptions: null, cancellationToken: cancellationToken);
        }

        // Example: Replace legacy JObject/JToken manipulation with JsonNode/JsonObject/JsonArray
        public JsonObject CreateUserJsonObject(string id, string name)
        {
            var user = new JsonObject
            {
                ["id"] = JsonValue.Create(id),
                ["name"] = JsonValue.Create(name)
            };
            return user;
        }

        // Example: Mutate JsonNode using Set, Add, Remove methods
        public void UpdateUserName(JsonObject user, string newName)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
            user["name"] = JsonValue.Create(newName);
        }
    }

    // Source generation context for high-throughput scenarios
    [JsonSerializable(typeof(object))]
    [JsonSourceGenerationOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false
    )]
    internal partial class JsonContext : JsonSerializerContext
    {
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