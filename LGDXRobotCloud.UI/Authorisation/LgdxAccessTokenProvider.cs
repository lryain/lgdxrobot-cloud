using LGDXRobotCloud.UI.Constants;
using LGDXRobotCloud.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Kiota.Abstractions.Authentication;

namespace LGDXRobotCloud.UI.Authorisation;

public class LgdxAccessTokenProvider(
    AuthenticationStateProvider authenticationStateProvider,
    ITokenService tokenService,
    ILogger<LgdxAccessTokenProvider> logger
  ) : IAccessTokenProvider
{
  private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
  private readonly ITokenService _tokenService = tokenService;
  private readonly ILogger<LgdxAccessTokenProvider> _logger = logger;

  public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = default, CancellationToken cancellationToken = default)
  {
    var user = _authenticationStateProvider.GetAuthenticationStateAsync().Result.User;
    
    // Debug logging
    var userId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
    _logger.LogInformation("Getting access token for user: {UserId}, IsAuthenticated: {IsAuthenticated}", userId, user.Identity?.IsAuthenticated);

    // Logout if the access token is expired
    var accessTokenExpiresAt = _tokenService.GetAccessTokenExpiresAt(user);
    _logger.LogInformation("Access token expires at: {ExpiresAt}, Current time: {CurrentTime}", accessTokenExpiresAt, DateTime.UtcNow);
    
    if (DateTime.UtcNow >= accessTokenExpiresAt)
    {
      _logger.LogWarning("Access token expired, logging out user: {UserId}", userId);
      _tokenService.Logout(user);
    }

    var token = _tokenService.GetAccessToken(user);
    _logger.LogInformation("Retrieved access token: {HasToken}", !string.IsNullOrEmpty(token));
    
    return Task.FromResult(token);
  }

  public AllowedHostsValidator AllowedHostsValidator { get; } = new AllowedHostsValidator();
}