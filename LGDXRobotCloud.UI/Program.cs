using System.Security.Cryptography.X509Certificates;
using LGDXRobotCloud.UI;
using LGDXRobotCloud.UI.Authorisation;
using LGDXRobotCloud.UI.Components;
using LGDXRobotCloud.UI.Constants;
using LGDXRobotCloud.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddMemoryCache();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
store.Open(OpenFlags.ReadOnly);
var redisOptions = ConfigurationOptions.Parse(builder.Configuration["Redis:ConnectionString"]!);

// Check if SSL is enabled
var useSsl = builder.Configuration.GetValue<bool>("Redis:UseSsl", false);
redisOptions.Ssl = useSsl;

if (useSsl)
{
	redisOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
	redisOptions.CertificateSelection += delegate
	{
		var certSN = builder.Configuration["Redis:CertificateSN"];
		if (!string.IsNullOrEmpty(certSN))
		{
			return store.Certificates.FirstOrDefault(cert => cert.SerialNumber.Contains(certSN));
		}
		return null;
	};
	redisOptions.CertificateValidation += (sender, cert, chain, errors) =>
	{
		if (cert == null)
		{
			return false;
		}
		var certSN = builder.Configuration["Redis:CertificateSN"];
		if (!string.IsNullOrEmpty(certSN))
		{
			var myCert = store.Certificates.FirstOrDefault(cert => cert.SerialNumber.Contains(certSN));
			if (myCert != null && myCert.Issuer == cert.Issuer)
			{
				return true;
			}
		}
		return false;
	};
}
else
{
	redisOptions.AbortOnConnectFail = false;
}
var redis = ConnectionMultiplexer.Connect(redisOptions);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

// Add API
var apiCertThumbprint = builder.Configuration["LGDXRobotCloudAPI:CertificateThumbprint"];
var url = new Uri(builder.Configuration["LGDXRobotCloudAPI:Url"]!);

X509Certificate2? certificate = null;
if (!string.IsNullOrEmpty(apiCertThumbprint))
{
	certificate = store.Certificates.FirstOrDefault(cert => cert.Thumbprint.Equals(apiCertThumbprint, StringComparison.OrdinalIgnoreCase));
}

builder.Services.AddKiotaHandlers();
builder.Services.AddScoped<LgdxApiClientFactory>();
builder.Services.AddHttpClient<LgdxApiClientFactory>((sp, client) => 
{
  client.BaseAddress = url;
})
	.AddHttpMessageHandler<HeadersInspectionHandler>()
	.ConfigurePrimaryHttpMessageHandler(() => {
			var handler = new HttpClientHandler
			{
				AllowAutoRedirect = true,
				UseDefaultCredentials = true
			};
			if (certificate != null)
			{
				handler.ClientCertificates.Add(certificate);
			}
			return handler;
	})
	.AttachKiotaHandlers();
builder.Services.AddScoped(sp => sp.GetRequiredService<LgdxApiClientFactory>().GetClient());
builder.Services.AddHttpClient<IRefreshTokenService, RefreshTokenService>(client =>
{ 
	client.BaseAddress = url;
})
	.ConfigurePrimaryHttpMessageHandler(() => {
			var handler = new HttpClientHandler
			{
				AllowAutoRedirect = true,
				UseDefaultCredentials = true
			};
			if (certificate != null)
			{
				handler.ClientCertificates.Add(certificate);
			}
			return handler;
	});
builder.Services.AddScoped<ICachedRealmService, CachedRealmService>();
builder.Services.AddScoped<IRobotDataService, RobotDataService>();
builder.Services.AddScoped<ISlamDataService, SlamDataService>();
builder.Services.AddSingleton<IRealTimeService, RealTimeService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(cfg =>
	{
		cfg.LoginPath = AppRoutes.Identity.Login;
	});
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, LgdxAuthenticationStateProvider>();

// Policy
builder.Services.AddScoped<IAuthorizationHandler, ValidateLgdxUserAccessHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ValidateLgdxUserAccesPolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.MapPost("api/Logout", async (HttpContext context) =>
{
	await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
	return TypedResults.LocalRedirect(AppRoutes.Identity.Login);
});

app.Run();