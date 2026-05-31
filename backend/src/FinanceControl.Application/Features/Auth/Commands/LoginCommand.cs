using FinanceControl.Application.Common.Exceptions;
using FinanceControl.Application.Common.Interfaces;
using FinanceControl.Application.Features.Auth.Dtos;
using FluentValidation;
using MediatR;

namespace FinanceControl.Application.Features.Auth.Commands;

/// <summary>
/// Login simplificado para MVP: usuários estáticos vindos de configuração.
/// Em produção, substituir pelo ASP.NET Core Identity + tabela Users.
/// </summary>
public sealed record LoginCommand(string Email, string Password) : IRequest<TokenResponseDto>;

public sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public sealed class LoginHandler : IRequestHandler<LoginCommand, TokenResponseDto>
{
    private readonly IJwtTokenService _jwt;

    public LoginHandler(IJwtTokenService jwt) => _jwt = jwt;

    public Task<TokenResponseDto> Handle(LoginCommand r, CancellationToken ct)
    {
        // MVP: usuário fixo (substituir por persistência real)
        if (r.Email.Equals("rickyteck@hotmail.com", StringComparison.OrdinalIgnoreCase)
            && r.Password == "FinanceControl@2026")
        {
            var token = _jwt.Generate(
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Ricardo Perdigao", r.Email, new[] { "Admin", "User" });
            return Task.FromResult(token);
        }

        throw new UnauthorizedException("Email ou senha inválidos.");
    }
}
