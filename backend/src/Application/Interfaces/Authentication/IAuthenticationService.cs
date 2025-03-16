using Application.Common;
using Application.DTO.Authentication;

namespace Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<Result<AuthenticationDTO>> SignInAsync(SignInDTO signInDTO);
    Task<Result<IdleUnit>> SignUpAsync(SignUpDTO signUpDTO);
    Task<Result<AuthenticationDTO>> RefreshTokenAsync(string refreshToken);
}

//todo: move to request/response layer