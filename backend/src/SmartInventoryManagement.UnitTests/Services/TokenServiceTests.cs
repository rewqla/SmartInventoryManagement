using Application.Authentication;
using Application.Interfaces.Authentication;
using Application.Services.Authentication;
using Microsoft.Extensions.Configuration;

namespace SmartInventoryManagement.Tests.Services;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;
    private readonly Mock<IConfiguration> _configurationMock;

    public TokenServiceTests()
    {
        _configurationMock = new Mock<IConfiguration>();

        _configurationMock.Setup(c => c["Jwt:Secret"]).Returns("SuperSecretKeyThatIsAtLeast32BytesLong!");
        _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
        _configurationMock.Setup(c => c["Jwt:AccessTokenLifetime"]).Returns("00:30:00");
        _configurationMock.Setup(c => c["Jwt:RefreshTokenLifetime"]).Returns("7.00:00:00");

        _tokenService = new TokenService(_configurationMock.Object);

        _testUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "TestUser",
            Role = new Role { Name = "Admin" }
        };
    }

    [Fact]
    public async Task SignInAsync()
    {
        // Arrange

        // Act

        // Assert
    }
    
}