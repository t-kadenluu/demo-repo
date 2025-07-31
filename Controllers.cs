using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.TestService.Controllers
{
    /// <summary>
    /// Controller for handling user operations
    /// </summary>
    public class UserController
    {
        public async ValueTask<string> GetUserAsync(int id, CancellationToken cancellationToken = default)
        {
            // Simulate async operation and observe cancellation
            await Task.Delay(10, cancellationToken).WaitAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            var user = $"User ID: {id}, Name: Test User";
            return user;
        }

        public async ValueTask<string> CreateUserAsync(object userData, CancellationToken cancellationToken = default)
        {
            // Simulate async user creation and observe cancellation
            await Task.Delay(10, cancellationToken).WaitAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return "User created successfully";
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