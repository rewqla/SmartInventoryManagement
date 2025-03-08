using Application.Authentication;
using Application.DTO.Authentication;
using Application.Interfaces.Authentication;

namespace SmartInventoryManagement.Tests.Services;

public class AuthenticationServiceTests
{
    private readonly AuthenticationService _authenticationService;
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IPasswordHasher> _passwordHasher;

    public AuthenticationServiceTests()
    {
        _passwordHasher = new Mock<IPasswordHasher>();
        _refreshTokenRepository = new Mock<IRefreshTokenRepository>();
        _userRepository = new Mock<IUserRepository>();
        _tokenService = new Mock<ITokenService>();

        _authenticationService =
            new AuthenticationService(_tokenService.Object, _passwordHasher.Object, _userRepository.Object,
                _refreshTokenRepository.Object);
    }

    // [Fact]
    public async Task SignInAsync_Success_ReturnsAuthenticationDTO()
    {
        // Arrange
        var user = new UserFaker().Generate();
        var signInDTO = new SignInDTO { EmailOrPhone = "user@example.com", Password = "password" };
        var refreshToken = new RefreshToken { Token = "refreshToken", ExpiresOnUtc = DateTime.UtcNow.AddDays(30) };
        
        _userRepository.Setup(x => x.GetByEmailOrPhoneAsync(signInDTO.EmailOrPhone)).ReturnsAsync(user);
        _passwordHasher.Setup(x => x.Verify(signInDTO.Password, user.PasswordHash)).Returns(true);

        _refreshTokenRepository.Setup(x => x.DeleteByUserIdAsync(user.Id)).Returns(Task.CompletedTask);
        
        // Act
        var result = await _authenticationService.SignInAsync(signInDTO);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
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
    
  
    
    //todo:  write tests for token service
}