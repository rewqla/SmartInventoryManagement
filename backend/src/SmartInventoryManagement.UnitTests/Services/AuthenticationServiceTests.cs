using Application.Authentication;
using Application.Configuration;
using Application.DTO.Authentication;
using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Validation.Authentication;

namespace SmartInventoryManagement.Tests.Services;

public class AuthenticationServiceTests
{
    private readonly AuthenticationService _authenticationService;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IRoleRepository> _roleRepository;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<LockoutConfig> _lockoutConfig;
    private readonly Mock<IEmailService> _emailService;
    private readonly Mock<IPasswordHasher> _passwordHasher;
    private readonly Mock<IDateTimeProvider> _dateTimeProvider;

    //todo: update test with lockout and email
    //todo: write lockout tests
    public AuthenticationServiceTests()
    {
        _passwordHasher = new Mock<IPasswordHasher>();
        _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _userRepository = new Mock<IUserRepository>();
        _tokenService = new Mock<ITokenService>();
        _roleRepository = new Mock<IRoleRepository>();
        _lockoutConfig = new Mock<LockoutConfig>();
        _emailService = new Mock<IEmailService>();
        _dateTimeProvider = new Mock<IDateTimeProvider>();

        _authenticationService =
            new AuthenticationService(_tokenService.Object, _passwordHasher.Object, _userRepository.Object,
                _refreshTokenRepository.Object, _roleRepository.Object, _dateTimeProvider.Object, _lockoutConfig.Object,
                _emailService.Object);
    }

    [Fact]
    public async Task SignInAsync_UserNotFound_ReturnsFailure()
    {
        // Arrange
        var signInDTO = new SignInDTO { EmailOrPhone = "nonexistent@example.com", Password = "password" };
        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone))
            .ReturnsAsync((User)null);

        // Act
        var result = await _authenticationService.SignInAsync(signInDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("User.NotFound");
    }

    [Fact]
    public async Task SignInAsync_AccountIsLocked_ReturnsLockedOutFailure()
    {
        // Arrange
        var user = new UserFaker().Generate();
        user.LockoutEnd = DateTime.UtcNow.AddHours(1);

        var signInDTO = new SignInDTO
        {
            EmailOrPhone = user.Email,
            Password = "password"
        };

        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(user.Email))
            .ReturnsAsync(user);

        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        // Act
        var result = await _authenticationService.SignInAsync(signInDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.AccountLockedOut");

        _passwordHasher.Verify(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SignInAsync_PasswordVerificationFails_ReturnsInvalidCredentialsError()
    {
        // Arrange
        var user = new UserFaker().Generate();
        var signInDTO = new SignInDTO
        {
            EmailOrPhone = user.Email,
            Password = "wrongPassword"
        };

        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone))
            .ReturnsAsync(user);

        _passwordHasher.Setup(x => x.Verify(signInDTO.Password, user.PasswordHash))
            .Returns(false);

        // Act
        var result = await _authenticationService.SignInAsync(signInDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.InvalidCredentials");
        result.Error.Description.Should().Be("Incorrect password");

        _userRepository.Verify(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone), Times.Once);
        _passwordHasher.Verify(x => x.Verify(signInDTO.Password, user.PasswordHash), Times.Once);

        _userRepository.Verify(x => x.Update(It.Is<User>(u =>
            u.FailedLoginAttempts == 1 &&
            u.LockoutEnd == null
        )), Times.Once);

        _tokenService.Verify(x => x.GenerateJwtToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task SignInAsync_JwtTokenGenerationFails_ReturnsTokenGenerationError()
    {
        // Arrange
        var user = new UserFaker().Generate();
        var signInDTO = new SignInDTO { EmailOrPhone = user.Email, Password = "correctPassword" };

        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone)).ReturnsAsync(user);
        _passwordHasher.Setup(x => x.Verify(signInDTO.Password, user.PasswordHash)).Returns(true);

        _tokenService.Setup(x => x.GenerateJwtToken(user))
            .Throws(new InvalidOperationException("Secret key must be at least 32 bytes long."));

        // Act
        var result = await _authenticationService.SignInAsync(signInDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.TokenGenerationError");
        result.Error.Description.Should().Contain("Secret key must be at least 32 bytes long.");

        _userRepository.Verify(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone), Times.Once);
        _passwordHasher.Verify(x => x.Verify(signInDTO.Password, user.PasswordHash), Times.Once);
        _tokenService.Verify(x => x.GenerateJwtToken(It.IsAny<User>()), Times.Once);
        _tokenService.Verify(x => x.GenerateRefreshToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task SignInAsync_RefreshTokenGenerationFails_ReturnsTokenGenerationError()
    {
        // Arrange
        var user = new UserFaker().Generate();
        var signInDTO = new SignInDTO { EmailOrPhone = user.Email, Password = "correctPassword" };

        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone)).ReturnsAsync(user);
        _passwordHasher.Setup(x => x.Verify(signInDTO.Password, user.PasswordHash)).Returns(true);
        _tokenService.Setup(x => x.GenerateJwtToken(user)).Returns("validAccessToken");
        _tokenService.Setup(x => x.GenerateRefreshToken(user))
            .Throws(new Exception("Unexpected error during refresh token generation"));

        // Act
        var result = await _authenticationService.SignInAsync(signInDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.TokenGenerationError");
        result.Error.Description.Should().Contain("Unexpected error during refresh token generation");

        _userRepository.Verify(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone), Times.Once);
        _passwordHasher.Verify(x => x.Verify(signInDTO.Password, user.PasswordHash), Times.Once);

        _userRepository.Verify(x => x.Update(It.Is<User>(u =>
            u.FailedLoginAttempts == 0 &&
            u.LockoutEnd == null
        )), Times.Once);

        _tokenService.Verify(x => x.GenerateJwtToken(It.IsAny<User>()), Times.Once);
        _tokenService.Verify(x => x.GenerateRefreshToken(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task SignInAsync_Success_ReturnsAuthenticationDTO()
    {
        // Arrange
        var user = new UserFaker().Generate();
        var signInDTO = new SignInDTO { EmailOrPhone = user.Email, Password = "correctPassword" };
        var refreshToken = new RefreshToken { Token = "refreshToken", ExpiresOnUtc = DateTime.UtcNow.AddDays(30) };

        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone)).ReturnsAsync(user);
        _passwordHasher.Setup(x => x.Verify(signInDTO.Password, user.PasswordHash)).Returns(true);
        _tokenService.Setup(x => x.GenerateJwtToken(user)).Returns("accessToken");
        _tokenService.Setup(x => x.GenerateRefreshToken(user)).Returns(refreshToken);
        _refreshTokenRepository.Setup(x => x.DeleteByUserIdAsync(user.Id)).Returns(Task.CompletedTask);
        _refreshTokenRepository.Setup(x => x.SaveRefreshTokenAsync(refreshToken)).Returns(Task.CompletedTask);

        // Act
        var result = await _authenticationService.SignInAsync(signInDTO);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.AccessToken.Should().Be("accessToken");
        result.Value!.RefreshToken.Should().Be("refreshToken");

        _userRepository.Verify(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone), Times.Once);
        _passwordHasher.Verify(x => x.Verify(signInDTO.Password, user.PasswordHash), Times.Once);
        _tokenService.Verify(x => x.GenerateJwtToken(It.IsAny<User>()), Times.Once);
        _tokenService.Verify(x => x.GenerateRefreshToken(It.IsAny<User>()), Times.Once);
        _refreshTokenRepository.Verify(x => x.DeleteByUserIdAsync(user.Id), Times.Once);
        _refreshTokenRepository.Verify(x => x.SaveRefreshTokenAsync(refreshToken), Times.Once);
    }

    [Fact]
    public async Task RefreshTokenAsync_TokenDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var refreshToken = "invalidToken";
        _refreshTokenRepository.Setup(x => x.GetByTokenAsync(refreshToken))
            .ReturnsAsync((RefreshToken)null);

        // Act
        var result = await _authenticationService.RefreshTokenAsync(refreshToken);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.InvalidRefreshToken");

        _refreshTokenRepository.Verify(x => x.GetByTokenAsync(refreshToken), Times.Once);
    }

    [Fact]
    public async Task RefreshTokenAsync_TokenIsExpired_ReturnsFailure()
    {
        // Arrange
        var expiredRefreshToken = new RefreshToken
            { Token = "expiredToken", ExpiresOnUtc = DateTime.UtcNow.AddMinutes(-5) };
        _refreshTokenRepository.Setup(x => x.GetByTokenAsync(expiredRefreshToken.Token))
            .ReturnsAsync(expiredRefreshToken);

        // Act
        var result = await _authenticationService.RefreshTokenAsync(expiredRefreshToken.Token);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.InvalidRefreshToken");

        _refreshTokenRepository.Verify(x => x.GetByTokenAsync(expiredRefreshToken.Token), Times.Once);
    }

    [Fact]
    public async Task RefreshTokenAsync_UserNotFound_ReturnsFailure()
    {
        // Arrange
        var refreshToken = new RefreshToken
            { Token = "validToken", ExpiresOnUtc = DateTime.UtcNow.AddMinutes(30), UserId = Guid.NewGuid() };
        _refreshTokenRepository.Setup(x => x.GetByTokenAsync(refreshToken.Token)).ReturnsAsync(refreshToken);
        _userRepository.Setup(x => x.GetByIdWithRoles(refreshToken.UserId)).ReturnsAsync((User)null);

        // Act
        var result = await _authenticationService.RefreshTokenAsync(refreshToken.Token);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("User.NotFound");

        _refreshTokenRepository.Verify(x => x.GetByTokenAsync(refreshToken.Token), Times.Once);
        _userRepository.Verify(x => x.GetByIdWithRoles(refreshToken.UserId), Times.Once);
    }

    [Fact]
    public async Task RefreshTokenAsync_JwtTokenGenerationFails_ReturnsTokenGenerationError()
    {
        // Arrange
        var user = new UserFaker().Generate();
        var refreshToken = new RefreshToken
            { Token = "validToken", ExpiresOnUtc = DateTime.UtcNow.AddMinutes(30), UserId = Guid.NewGuid() };

        _refreshTokenRepository.Setup(x => x.GetByTokenAsync(refreshToken.Token)).ReturnsAsync(refreshToken);
        _userRepository.Setup(x => x.GetByIdWithRoles(refreshToken.UserId)).ReturnsAsync(user);

        _tokenService.Setup(x => x.GenerateJwtToken(user))
            .Throws(new InvalidOperationException("Secret key must be at least 32 bytes long."));

        // Act
        var result = await _authenticationService.RefreshTokenAsync(refreshToken.Token);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.TokenGenerationError");
        result.Error.Description.Should().Contain("Secret key must be at least 32 bytes long.");

        _refreshTokenRepository.Verify(x => x.GetByTokenAsync(refreshToken.Token), Times.Once);
        _userRepository.Verify(x => x.GetByIdWithRoles(refreshToken.UserId), Times.Once);
        _tokenService.Verify(x => x.GenerateJwtToken(It.IsAny<User>()), Times.Once);
        _tokenService.Verify(x => x.GenerateRefreshToken(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task RefreshTokenAsync_RefreshTokenGenerationFails_ReturnsTokenGenerationError()
    {
        // Arrange
        var user = new UserFaker().Generate();
        var refreshToken = new RefreshToken
            { Token = "validToken", ExpiresOnUtc = DateTime.UtcNow.AddMinutes(30), UserId = Guid.NewGuid() };

        _refreshTokenRepository.Setup(x => x.GetByTokenAsync(refreshToken.Token)).ReturnsAsync(refreshToken);
        _userRepository.Setup(x => x.GetByIdWithRoles(refreshToken.UserId)).ReturnsAsync(user);

        _tokenService.Setup(x => x.GenerateJwtToken(user)).Returns("validAccessToken");

        _tokenService.Setup(x => x.GenerateRefreshToken(user))
            .Throws(new InvalidOperationException("Failed to generate a refresh token."));

        // Act
        var result = await _authenticationService.RefreshTokenAsync(refreshToken.Token);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.TokenGenerationError");
        result.Error.Description.Should().Contain("Failed to generate a refresh token.");

        _refreshTokenRepository.Verify(x => x.GetByTokenAsync(refreshToken.Token), Times.Once);
        _userRepository.Verify(x => x.GetByIdWithRoles(refreshToken.UserId), Times.Once);
        _tokenService.Verify(x => x.GenerateJwtToken(It.IsAny<User>()), Times.Once);
        _tokenService.Verify(x => x.GenerateRefreshToken(It.IsAny<User>()), Times.Once);

        _refreshTokenRepository.Verify(x => x.DeleteByUserIdAsync(It.IsAny<Guid>()), Times.Never);
        _refreshTokenRepository.Verify(x => x.SaveRefreshTokenAsync(It.IsAny<RefreshToken>()), Times.Never);
    }

    [Fact]
    public async Task RefreshTokenAsync_ValidRefreshToken_ReturnsSuccess()
    {
        // Arrange
        var user = new UserFaker().Generate();
        var refreshToken = new RefreshToken
        {
            Token = "validToken",
            ExpiresOnUtc = DateTime.UtcNow.AddMinutes(30),
            UserId = Guid.NewGuid()
        };
        var newRefreshToken = new RefreshToken
        {
            Token = "newValidToken",
            ExpiresOnUtc = DateTime.UtcNow.AddMinutes(30),
            UserId = refreshToken.UserId
        };

        _refreshTokenRepository.Setup(x => x.GetByTokenAsync(refreshToken.Token)).ReturnsAsync(refreshToken);
        _userRepository.Setup(x => x.GetByIdWithRoles(refreshToken.UserId)).ReturnsAsync(user);
        _tokenService.Setup(x => x.GenerateJwtToken(user)).Returns("validAccessToken");
        _tokenService.Setup(x => x.GenerateRefreshToken(user)).Returns(newRefreshToken);

        // Act
        var result = await _authenticationService.RefreshTokenAsync(refreshToken.Token);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("validAccessToken");
        result.Value.RefreshToken.Should().Be("newValidToken");
    }

    [Fact]
    public async Task SignUpAsync_UserAlreadyExists_ReturnsFailure()
    {
        // Arrange
        var signUpDTO = new SignUpDTO
        {
            Email = "existing@example.com",
            PhoneNumber = "+123456789123",
            FullName = "Existing User",
            Password = "SecurePassword123!"
        };

        var existingUser = new UserFaker().Generate();
        existingUser.Email = signUpDTO.Email;

        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signUpDTO.Email))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _authenticationService.SignUpAsync(signUpDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.EmailAlreadyExists");
    }

    [Fact]
    public async Task SignUpAsync_PhoneAlreadyExists_ReturnsFailure()
    {
        // Arrange
        var signUpDTO = new SignUpDTO
        {
            Email = "existing@example.com",
            PhoneNumber = "+123456789123",
            FullName = "Existing User",
            Password = "SecurePassword123!"
        };

        var existingUser = new UserFaker().Generate();
        existingUser.PhoneNumber = signUpDTO.PhoneNumber;

        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signUpDTO.PhoneNumber))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _authenticationService.SignUpAsync(signUpDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Authentication.PhoneAlreadyExists");
    }

    [Fact]
    public async Task SignUpAsync_RoleNotFound_ReturnsFailure()
    {
        // Arrange
        var signUpDTO = new SignUpDTO
        {
            Email = "newuser@example.com",
            PhoneNumber = "+123456789123",
            FullName = "New User",
            Password = "SecurePassword123!"
        };

        var existingUser = (User)null;
        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signUpDTO.Email))
            .ReturnsAsync(existingUser);
        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signUpDTO.PhoneNumber))
            .ReturnsAsync(existingUser);
        _roleRepository.Setup(x => x.GetByNameAsync("Worker"))
            .ReturnsAsync((Role)null);

        // Act
        var result = await _authenticationService.SignUpAsync(signUpDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Role.NotFound");
        result.Error.Description.Should().Be("The role was not found");
    }

    [Fact]
    public async Task SignUpAsync_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var signUpDTO = new SignUpDTO
        {
            Email = "newuser@example.com",
            PhoneNumber = "+123456789123",
            FullName = "New User",
            Password = "SecurePassword123!"
        };

        var existingUser = (User)null;
        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signUpDTO.Email))
            .ReturnsAsync(existingUser);
        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signUpDTO.PhoneNumber))
            .ReturnsAsync(existingUser);

        var defaultRole = new Role { Name = "Worker" };
        _roleRepository.Setup(x => x.GetByNameAsync("Worker"))
            .ReturnsAsync(defaultRole);

        _passwordHasher.Setup(x => x.Hash(signUpDTO.Password))
            .Returns("hashedpassword");

        _userRepository.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User());

        // Act
        var result = await _authenticationService.SignUpAsync(signUpDTO);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(IdleUnit.Value);
    }

    [Fact]
    public async Task SignUpAsync_InvalidInput_ReturnsValidationError()
    {
        // Arrange
        var signUpDTO = new SignUpDTO
        {
            Email = "invalid-email",
            PhoneNumber = "12345",
            FullName = "",
            Password = "short"
        };

        var result = await _authenticationService.SignUpAsync(signUpDTO);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("SignUpDTO.ValidationError");
    }
}