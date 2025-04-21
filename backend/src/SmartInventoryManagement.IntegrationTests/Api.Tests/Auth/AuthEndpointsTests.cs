using API.Endpoints.Auth;
using Application.DTO.Authentication;
using Microsoft.AspNetCore.Mvc;
using SmartInventoryManagement.IntegrationTests.Common;
using SmartInventoryManagement.IntegrationTests.Helpers;

namespace SmartInventoryManagement.IntegrationTests.Api.Tests.Auth;

[Collection(nameof(SmartInventoryCollection))]
public class AuthEndpointsTests
{
    private readonly HttpClient _httpClient;

    public AuthEndpointsTests(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
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

    [Fact]
    public async Task RefreshToken_ReturnsToManyRequests_WhenRateLimitIsExceeded()
    {
        // Arrange
        var refreshTokenRequest = new RefreshTokenRequest { RefreshToken = "some-token" };

        // Act
        for (int i = 0; i < 10; i++)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenRequest);
            response.StatusCode.Should().NotBe(HttpStatusCode.TooManyRequests);
        }

        // Assert
        var blockedResponse = await _httpClient.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenRequest);
        blockedResponse.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task RefreshToken_Returns429ForTHeFirstUser_Then200ForTheSecondUser()
    {
        // Arrange
        var refreshTokenRequest = new RefreshTokenRequest { RefreshToken = "some-token" };
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Astral"));
        
        // Act
        for (int i = 0; i < 10; i++)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenRequest);
            Assert.NotEqual(HttpStatusCode.TooManyRequests, response.StatusCode);
        }
        
        // Assert
        var blockedResponse = await _httpClient.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenRequest);
        blockedResponse.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        
        _httpClient.DefaultRequestHeaders.Authorization =
            new("Bearer", AccessTokenProvider.GenerateToken("Admin"));

        var authenticatedResponse  = await _httpClient.PostAsJsonAsync("/api/auth/refresh-token", refreshTokenRequest);
        authenticatedResponse .StatusCode.Should().NotBe(HttpStatusCode.TooManyRequests);

        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}