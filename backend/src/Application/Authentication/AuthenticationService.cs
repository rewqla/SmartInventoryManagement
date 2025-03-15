using Application.Common;
using Application.DTO.Authentication;
using Application.Exceptions;
using Application.Interfaces.Authentication;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthenticationService(ITokenService tokenService, IPasswordHasher passwordHasher,
        IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<AuthenticationDTO>> SignInAsync(SignInDTO signInDTO)
    {
        var user = await _userRepository.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone);

        if (user == null)
        {
            return Result<AuthenticationDTO>.Failure(CommonErrors.NotFound("User"));
        }

        if (!_passwordHasher.Verify(signInDTO.Password, user.PasswordHash))
        {
            return Result<AuthenticationDTO>.Failure(AuthenticationErrors.InvalidCredentials());
        }

        string accessToken;
        try
        {
            accessToken = _tokenService.GenerateJwtToken(user);
        }
        catch (Exception ex)
        {
            return Result<AuthenticationDTO>.Failure(TokenErrors.TokenGenerationError("access", ex.Message));
        }

        //todo: write unit tests for timespan 

        RefreshToken refreshToken;
        try
        {
            refreshToken = _tokenService.GenerateRefreshToken(user);
        }
        catch (Exception ex)
        {
            return Result<AuthenticationDTO>.Failure(TokenErrors.TokenGenerationError("refresh", ex.Message));
        }

        await _refreshTokenRepository.DeleteByUserIdAsync(user.Id);
        await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken);

        //todo: add unit tests
        //todo: add role check for endpoints

        var response = new AuthenticationDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };

        return Result<AuthenticationDTO>.Success(response);
    }

    public async Task<Result<AuthenticationDTO>> RefreshTokenAsync(string refreshToken)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (existingRefreshToken == null || existingRefreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            return Result<AuthenticationDTO>.Failure(TokenErrors.InvalidOrExpiredRefreshToken());
        }

        var user = await _userRepository.GetByIdWithRoles(existingRefreshToken.UserId);
        if (user == null)
        {
            return Result<AuthenticationDTO>.Failure(CommonErrors.NotFound("User"));
        }

        string accessToken;
        try
        {
            accessToken = _tokenService.GenerateJwtToken(user);
        }
        catch (Exception ex)
        {
            return Result<AuthenticationDTO>.Failure(TokenErrors.TokenGenerationError("access", ex.Message));
        }

        RefreshToken newRefreshToken;
        try
        {
            newRefreshToken = _tokenService.GenerateRefreshToken(user);
        }
        catch (Exception ex)
        {
            return Result<AuthenticationDTO>.Failure(TokenErrors.TokenGenerationError("refresh", ex.Message));
        }

        await _refreshTokenRepository.DeleteByUserIdAsync(user.Id);
        await _refreshTokenRepository.SaveRefreshTokenAsync(newRefreshToken);

        var response = new AuthenticationDTO
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token
        };

        return Result<AuthenticationDTO>.Success(response);
    }
}