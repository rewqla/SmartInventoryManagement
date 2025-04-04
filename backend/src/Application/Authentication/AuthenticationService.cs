﻿using Application.Common;
using Application.DTO.Authentication;
using Application.Exceptions;
using Application.Interfaces.Authentication;
using Application.Validation.Authentication;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;

namespace Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthenticationService(ITokenService tokenService, IPasswordHasher passwordHasher,
        IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IRoleRepository roleRepository)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<AuthenticationDTO>> SignInAsync(SignInDTO signInDTO)
    {
        var user = await _userRepository.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone);

        if (user == null)
        {
            return Result<AuthenticationDTO>.Failure(CommonErrors.NotFound("User"));
        }

        //todo: add custom lock ut for users
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

        //todo: add role check for endpoints

        var response = new AuthenticationDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        };

        return Result<AuthenticationDTO>.Success(response);
    }

    public async Task<Result<IdleUnit>> SignUpAsync(SignUpDTO signUpDTO)
    {
        var validator = new SignUpDTOValidator();
        var validationResult = await validator.ValidateAsync(signUpDTO);

        if (!validationResult.IsValid)
        {
            //todo: write unit tests for validation
            var errorDetails = validationResult.Errors.Select(error => new ErrorDetail
            {
                PropertyName = error.PropertyName,
                ErrorMessage = error.ErrorMessage
            }).ToList();

            return Result<IdleUnit>.Failure(CommonErrors.ValidationError("SignUpDTO", errorDetails));
        }

        var existingUser = await _userRepository.GetByEmailOrPhoneAsync(signUpDTO.Email);
        if (existingUser != null)
        {
            return Result<IdleUnit>.Failure(AuthenticationErrors.EmailAlreadyExists());
        }

        var existingPhoneUser = await _userRepository.GetByEmailOrPhoneAsync(signUpDTO.PhoneNumber);
        if (existingPhoneUser != null)
        {
            return Result<IdleUnit>.Failure(AuthenticationErrors.PhoneAlreadyExists());
        }

        string passwordHash = _passwordHasher.Hash(signUpDTO.Password);

        var defaultRole = await _roleRepository.GetByNameAsync("Worker");
        if (defaultRole == null)
        {
            return Result<IdleUnit>.Failure(CommonErrors.NotFound("role"));
        }

        var newUser = new User
        {
            Email = signUpDTO.Email,
            Phone = signUpDTO.PhoneNumber,
            Name = signUpDTO.FullName,
            PasswordHash = passwordHash,
            Role = defaultRole
        };

        await _userRepository.AddAsync(newUser);

        return Result<IdleUnit>.Success(IdleUnit.Value);
    }

    //todo: write some integration tests for refresh tokens
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