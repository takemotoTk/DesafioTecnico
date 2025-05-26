namespace VaccinationCardManagement.Application.Models;

public record AuthenticationModel
{
    public string UserIdentifier { get; init; }
    public string UserId { get; init; }
    public DateTime Created { get; init; }
    public DateTime Expiration { get; init; }
    public string TokenType { get; init; } = "Bearer";
    public string AccessToken { get; init; }
}