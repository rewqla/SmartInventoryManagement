using Application.Configuration;
using Application.DTO.Authentication;
using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Validation;
using Application.Validation.Authentication;
using Infrastructure.Entities;
using Infrastructure.Interfaces.Repositories;
using SharedKernel.Exceptions;
using SharedKernel.Interfaces;
using SharedKernel.ResultPattern;

namespace Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly LockoutConfig _lockoutConfig;
    private readonly IEmailService _emailService;

    public AuthenticationService(ITokenService tokenService, IPasswordHasher passwordHasher,
        IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IRoleRepository roleRepository,
        IDateTimeProvider dateTimeProvider, LockoutConfig lockoutConfig, IEmailService emailService)
    {
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _roleRepository = roleRepository;
        _dateTimeProvider = dateTimeProvider;
        _lockoutConfig = lockoutConfig;
        _emailService = emailService;
    }

    public async Task<Result<AuthenticationResponse>> SignInAsync(SignInRequest signInRequest)
    {
        var user = await _userRepository.GetByEmailOrPhoneAsync(signInRequest.EmailOrPhone);

        if (user == null)
        {
            return Result<AuthenticationResponse>.Failure(CommonErrors.NotFound("User"));
        }

        if (IsAccountLocked(user))
        {
            return Result<AuthenticationResponse>.Failure(AuthenticationErrors.AccountLockedOut());
        }

        if (!IsPasswordValid(signInRequest.Password, user.PasswordHash))
        {
            HandleFailedLoginAsync(user);

            return Result<AuthenticationResponse>.Failure(AuthenticationErrors.InvalidCredentials());
        }

        ResetLockoutAsync(user);

        return await GenerateTokensAsync(user);
    }

    public async Task<Result<IdleUnit>> SignUpAsync(SignUpDTO signUpDTO)
    {
        var validator = new SignUpDTOValidator();
        var validationResult = await validator.ValidateAsync(signUpDTO);

        if (!validationResult.IsValid)
        {
            var errorDetails = validationResult.ToErrorDetails();

            return Result<IdleUnit>.Failure(CommonErrors.ValidationError("SignUpDTO", errorDetails));
        }

        if (await UserExistsAsync(signUpDTO.Email))
            return Result<IdleUnit>.Failure(AuthenticationErrors.EmailAlreadyExists());

        if (await UserExistsAsync(signUpDTO.PhoneNumber))
            return Result<IdleUnit>.Failure(AuthenticationErrors.PhoneAlreadyExists());

        var defaultRole = await _roleRepository.GetByNameAsync("Worker");
        if (defaultRole == null)
        {
            return Result<IdleUnit>.Failure(CommonErrors.NotFound("role"));
        }

        var newUser = CreateUser(signUpDTO, defaultRole);
        await _userRepository.AddAsync(newUser);
        await _userRepository.CompleteAsync();

        //todo: unit test
        _ = Task.Run(async () =>
        {
            var subject = "Welcome to Smart Inventory Management!";
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Application", "Email",
                "Templates", "Register.cshtml");

            await _emailService.SendEmailWithTemplateAsync(signUpDTO.Email, subject, templatePath, signUpDTO);
        });

        return Result<IdleUnit>.Success(IdleUnit.Value);
    }

    public async Task<Result<AuthenticationResponse>> RefreshTokenAsync(string refreshToken)
    {
        var existingRefreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (existingRefreshToken == null || existingRefreshToken.ExpiresOnUtc < DateTime.UtcNow)
        {
            return Result<AuthenticationResponse>.Failure(TokenErrors.InvalidOrExpiredRefreshToken());
        }

        var user = await _userRepository.GetByIdWithRoles(existingRefreshToken.UserId);
        if (user == null)
        {
            return Result<AuthenticationResponse>.Failure(CommonErrors.NotFound("User"));
        }

        return await GenerateTokensAsync(user);
    }

    private async Task<Result<AuthenticationResponse>> GenerateTokensAsync(User user)
    {
        // TODO: write unit tests for timespan
        string accessToken;
        try
        {
            accessToken = _tokenService.GenerateJwtToken(user);
        }
        catch (Exception ex)
        {
            return Result<AuthenticationResponse>.Failure(TokenErrors.TokenGenerationError("access", ex.Message));
        }

        RefreshToken refreshToken;
        try
        {
            refreshToken = _tokenService.GenerateRefreshToken(user);
        }
        catch (Exception ex)
        {
            return Result<AuthenticationResponse>.Failure(TokenErrors.TokenGenerationError("refresh", ex.Message));
        }

        await _refreshTokenRepository.DeleteByUserIdAsync(user.Id);
        await _refreshTokenRepository.SaveRefreshTokenAsync(refreshToken);

        return Result<AuthenticationResponse>.Success(new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token
        });
    }

    private User CreateUser(SignUpDTO dto, Role role)
    {
        var passwordHash = _passwordHasher.Hash(dto.Password);

        return new User
        {
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            FullName = dto.FullName,
            PasswordHash = passwordHash,
            Role = role
        };
    }

    private async Task<bool> UserExistsAsync(string value)
    {
        return await _userRepository.GetByEmailOrPhoneAsync(value) is not null;
    }

    private bool IsAccountLocked(User user)
    {
        return user.LockoutEnd.HasValue && user.LockoutEnd > _dateTimeProvider.UtcNow;
    }

    private bool IsPasswordValid(string inputPassword, string passwordHash)
    {
        return _passwordHasher.Verify(inputPassword, passwordHash);
    }

    private void HandleFailedLoginAsync(User user)
    {
        user.FailedLoginAttempts++;

        if (user.FailedLoginAttempts >= _lockoutConfig.MaxFailedAttempts)
        {
            user.LockoutEnd = _dateTimeProvider.UtcNow.AddMinutes(_lockoutConfig.LockoutDurationMinutes);
            user.FailedLoginAttempts = 0;
        }

        _userRepository.Update(user);
        _userRepository.CompleteAsync();
    }

    private void ResetLockoutAsync(User user)
    {
        user.FailedLoginAttempts = 0;
        user.LockoutEnd = null;

        _userRepository.Update(user);
    }
}