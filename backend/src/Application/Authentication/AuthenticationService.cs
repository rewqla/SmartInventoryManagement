using Application.Common;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using SharedKernel;

namespace Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository; 
    private readonly IPasswordHasher _passwordHasher; 
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public AuthenticationService(ITokenService tokenService, IPasswordHasher passwordHasher, IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
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
        var refreshToken = _tokenService.GenerateRefreshToken(user);
        
        //todo: finish adding refresh token
        await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken);
        
        //todo: store refresh tokens in the db
        //todo: add role check for endpoints
        //todo: delete old revoke tokens from the db

        var response = new AuthenticationDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };

        return Result<AuthenticationDTO>.Success(response);
    }
}