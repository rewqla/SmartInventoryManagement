using Application.DTO.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests.Auth;

public class AuthEndpointsTests:
    IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly HttpClient _httpClient;

    public AuthEndpointsTests(IntegrationTestWebAppFactory appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }
    
    [Fact]
    public async Task SignIn_ReturnsOk_WhenValidCredentialsProvided()
    {
        // Arrange
        var signInDto = new SignInDTO
        {
            EmailOrPhone = "testuser@example.com",
            Password = "Test@1234"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/auth/signin", signInDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var authResponse = await response.Content.ReadFromJsonAsync<AuthenticationDTO>();
        authResponse.Should().NotBeNull();
        authResponse!.AccessToken.Should().NotBeNullOrWhiteSpace();
        authResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task SignIn_ReturnsUnauthorized_WhenPasswordIsInvalid()
    {
        // Arrange
        var signInDto = new SignInDTO
        {
            EmailOrPhone = "testuser@example.com",
            Password = "Tes21323423t@1234"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/auth/signin", signInDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

   
}