using Application.Common;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository; 
    private readonly IPasswordHasher _passwordHasher; 
    private readonly ITokenService _tokenService;

    public AuthenticationService(ITokenService tokenService, IPasswordHasher passwordHasher, IUserRepository userRepository)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<Result<AuthenticationDTO>> SignInAsync(SignInDTO signInDTO)
    {
        //todo: add user validation
        var user = await _userRepository.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone);

        if (user == null)
        {
            //todo: update Error code
            return Result<AuthenticationDTO>.Failure(new Error("User.NotFound", "User not found"));
        }

        if (!_passwordHasher.Verify(signInDTO.Password, user.PasswordHash))
        {
            //todo: update Error code
            return Result<AuthenticationDTO>.Failure(new Error("InvalidCredentials", "Incorrect password"));
        }

        var accessToken = _tokenService.GenerateJwtToken(user);
        
        //todo: finish adding refresh token
        var refreshToken = _tokenService.GenerateRefreshToken();

        //todo: store refresh tokens in the db
        //todo: add role check for endpoints

        var response = new AuthenticationDTO
        {
            AccessToken = accessToken,
            RefreshToken = null
        };

        return Result<AuthenticationDTO>.Success(response);
    }
}