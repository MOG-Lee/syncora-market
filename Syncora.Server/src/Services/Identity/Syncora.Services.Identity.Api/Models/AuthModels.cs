using System.ComponentModel.DataAnnotations;

namespace Syncora.Services.Identity.Api.Models
{
    public record LoginRequest(
        [Required] string Email,
        [Required] string Password);

    public record RegisterRequest(
        [Required][EmailAddress] string Email,
        [Required] string Password,
        [Required] string FirstName,
        [Required] string LastName);

    public record AuthResponse(
        string Email,
        string Token);
}
