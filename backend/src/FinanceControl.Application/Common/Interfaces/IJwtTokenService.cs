using FinanceControl.Application.Features.Auth.Dtos;

namespace FinanceControl.Application.Common.Interfaces;

public interface IJwtTokenService
{
    TokenResponseDto Generate(Guid userId, string userName, string email, IEnumerable<string> roles);
}
