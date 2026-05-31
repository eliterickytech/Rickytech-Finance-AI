namespace FinanceControl.Application.Features.Auth.Dtos;

public sealed record LoginDto(string Email, string Password);

public sealed record TokenResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTimeOffset ExpiresAt,
    string UserName);
