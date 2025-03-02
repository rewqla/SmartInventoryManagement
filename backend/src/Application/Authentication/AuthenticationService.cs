using Application.Common;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Repositories;
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

        await _refreshTokenRepository.DeleteByUserIdAsync(user.Id);
        //todo: finish adding refresh token
        await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken);
        
        //todo: add unit tests
        //todo: add role check for endpoints
        //todo: delete old revoke tokens from the db

        var response = new AuthenticationDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };

        return Result<AuthenticationDTO>.Success(response);
    }

    public async Task<Result<AuthenticationDTO>> RefreshTokenAsync(string refreshToken)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(refreshToken);
        
        if (existingRefreshToken == null || existingRefreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            //todo: update Error code
            return Result<AuthenticationDTO>.Failure(new Error("InvalidToken", "Refresh token is invalid or expired."));
        }

        var user = await _userRepository.GetByIdWithRoles(existingRefreshToken.UserId);
        if (user == null)
        {
            //todo: update Error code
            return Result<AuthenticationDTO>.Failure(new Error("User.NotFound", "User not found."));
        }

        var accessToken = _tokenService.GenerateJwtToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken(user);

        await _refreshTokenRepository.DeleteByUserIdAsync(user.Id);
        await _refreshTokenRepository.SaveRefreshTokenAsync(newRefreshToken);

        //todo: remove expired refresh tokens
        
        var response = new AuthenticationDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };

        return Result<AuthenticationDTO>.Success(response);
    }
}