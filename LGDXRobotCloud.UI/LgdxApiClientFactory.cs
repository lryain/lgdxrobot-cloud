using LGDXRobotCloud.UI.Authorisation;
using LGDXRobotCloud.UI.Client;
using LGDXRobotCloud.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace LGDXRobotCloud.UI;

public class LgdxApiClientFactory(
    AuthenticationStateProvider authenticationStateProvider,
    HttpClient httpClient,
    ITokenService tokenService,
    ILogger<LgdxAccessTokenProvider> logger
  )
{
  private readonly IAuthenticationProvider _authenticationProvider = new BaseBearerTokenAuthenticationProvider(
      new LgdxAccessTokenProvider(authenticationStateProvider, tokenService, logger)
    );
  private readonly HttpClient _httpClient = httpClient;

  public LgdxApiClient GetClient() 
  {
    return new LgdxApiClient(new HttpClientRequestAdapter(_authenticationProvider, httpClient: _httpClient));
  }
}