using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json; // For serialization
using Microsoft.AspNetCore.Mvc; // For ASP.NET Core controller base

namespace Microsoft.TestService.Controllers
{
    /// <summary>
    /// Controller for handling user operations
    /// </summary>
    [ApiController] // Use ASP.NET Core attribute for automatic model validation and routing
    [Route("api/[controller]")]
    public class UserController : ControllerBase // Inherit from ControllerBase for ASP.NET Core
    {
        // GET: api/User/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<string>> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            // Simulate async operation for cross-platform scalability
            await Task.Yield(); // No real async work here, but pattern is modernized

            // Use interpolated string for user info
            var user = $"User ID: {id}, Name: Test User";
            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<string>> CreateUserAsync([FromBody] Models.UserRequest userData, CancellationToken cancellationToken)
        {
            // Simulate async user creation
            await Task.Yield();

            // Return a success message
            return Ok("User created successfully");
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
        public string Name { get; set; } = string.Empty; // Use nullable reference types for safety
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class UserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
