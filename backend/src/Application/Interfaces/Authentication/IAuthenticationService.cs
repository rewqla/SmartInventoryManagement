using Application.Common;
using Application.DTO.Authentication;

namespace Application.Interfaces.Authentication;

public interface  IAuthenticationService
{
    Task<Result<AuthenticationDTO>> SignInAsync(SignInDTO signInDTO);
}

//todo: move to request/response layer