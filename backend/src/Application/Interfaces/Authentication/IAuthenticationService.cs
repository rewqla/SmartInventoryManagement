using Application.DTO.Authentication;
using SharedKernel.ResultPattern;

namespace Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<Result<AuthenticationResponse>> SignInAsync(SignInRequest signInRequest);
    Task<Result<IdleUnit>> SignUpAsync(SignUpDTO signUpDTO);
    Task<Result<AuthenticationResponse>> RefreshTokenAsync(string refreshToken);
}

//todo: move to request/response layer