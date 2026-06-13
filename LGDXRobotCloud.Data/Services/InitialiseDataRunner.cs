using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using LGDXRobotCloud.Data.DbContexts;
using LGDXRobotCloud.Data.Entities;
using LGDXRobotCloud.Utilities.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LGDXRobotCloud.Data.Services;


public class InitialiseDataRunner(
    LgdxContext context,
    LgdxLogsContext logsContext,
    UserManager<LgdxUser> userManager,
    IConfiguration configuration
  ) : IHostedService
{
  private readonly LgdxContext _context = context ?? throw new ArgumentNullException(nameof(context));
  private readonly LgdxLogsContext _logsContext = logsContext ?? throw new ArgumentNullException(nameof(logsContext));
  private readonly UserManager<LgdxUser> _userManager = userManager;
  private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

  private record CertificateDetail 
  {
    required public string RootCertificate { get; set; }
    required public string RobotCertificatePrivateKey { get; set; }
    required public string RobotCertificatePublicKey { get; set; }
    required public string RobotCertificateThumbprint { get; set; }
    required public DateTime RobotCertificateNotBefore { get; set; }
    required public DateTime RobotCertificateNotAfter { get; set; }
  }

  private static CertificateDetail GenerateRobotCertificate(Guid robotId)
  {
    var rootCertificateSn = Environment.GetEnvironmentVariable("ROOT_CERTIFICATE_SN");

    X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
    store.Open(OpenFlags.OpenExistingOnly);
    X509Certificate2 rootCertificate = store.Certificates.First(c => c.SerialNumber.Contains(rootCertificateSn!));

    var certificateNotBefore = DateTime.UtcNow;
    var certificateNotAfter = DateTimeOffset.UtcNow.AddDays(365);

    var rsa = RSA.Create();
    var certificateRequest = new CertificateRequest("CN=" + robotId.ToString(), rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    var certificate = certificateRequest.Create(rootCertificate, certificateNotBefore, certificateNotAfter, RandomNumberGenerator.GetBytes(20));

    return new CertificateDetail
    {
      RootCertificate = rootCertificate.ExportCertificatePem(),
      RobotCertificatePrivateKey = new string(PemEncoding.Write("PRIVATE KEY", rsa.ExportPkcs8PrivateKey())),
      RobotCertificatePublicKey = certificate.ExportCertificatePem(),
      RobotCertificateThumbprint = certificate.Thumbprint,
      RobotCertificateNotBefore = certificateNotBefore,
      RobotCertificateNotAfter = certificateNotAfter.DateTime
    };
  }

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    _context.Database.Migrate();
    _logsContext.Database.Migrate();
    if (_context.Users.Any())
    {
      return;
    }
    /*
     * Identity
     */
    // Roles
    var defaultRoles = LgdxRolesHelper.DefaultRoles;
    foreach (var (key, value) in defaultRoles)
    {
      var role = new LgdxRole{
        Id = key.ToString(),
        Name = value.Name,
        NormalizedName = value.Name.ToUpper(),
      };
      var roleStore = new RoleStore<LgdxRole>(_context);
      if (!_context.Roles.Any(r => r.Name == role.Name))
      {
        // Create Role
        await roleStore.CreateAsync(role, cancellationToken);
        // Add claims for role
        foreach (var scope in value.Scopes)
        {
          var claim = new Claim("scope", scope);
          await roleStore.AddClaimAsync(role, claim, cancellationToken);
        }
      }
    }
    // Admin User
    var firstUser = new LgdxUser
    {
      Id = Guid.CreateVersion7().ToString(),
      Email = _configuration["email"],
      EmailConfirmed = true,
      LockoutEnabled = true,
      Name = _configuration["fullName"],
      NormalizedEmail = _configuration["email"]!.ToUpper(),
      NormalizedUserName = _configuration["userName"]!.ToUpper(),
      SecurityStamp = Guid.NewGuid().ToString(),
      UserName = _configuration["userName"]
    };

    if (!_context.Users.Any(u => u.UserName == firstUser.UserName))
    {
      var password = new PasswordHasher<LgdxUser>();
      var hashed = password.HashPassword(firstUser, _configuration["password"]!);
      firstUser.PasswordHash = hashed;

      var userStore = new UserStore<LgdxUser>(_context);
      await userStore.CreateAsync(firstUser, cancellationToken);
    }
    // Assign user to roles
    LgdxUser? user = await _userManager.FindByEmailAsync(firstUser.Email!);
    var result = await _userManager.AddToRolesAsync(user!, ["Global Administrator"]);
    await _context.SaveChangesAsync(cancellationToken);

    // Seed Data
    var isSeedData = _configuration["seedData"];
    if (!string.IsNullOrEmpty(isSeedData) && bool.Parse(isSeedData) == true)
    {
      // Seed data from SQL files
      var scriptsPath = Path.Combine(Directory.GetCurrentDirectory(), "SQL");
      var files = Directory.GetFiles(scriptsPath, "*.sql").OrderBy(f => f);
      foreach (var file in files)
      {
        Console.WriteLine($"Executing {Path.GetFileName(file)}");
        var sql = await File.ReadAllTextAsync(file, cancellationToken);
        await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken: cancellationToken);
      }

      // Generate Robot Certificates
      // Note: Assume that the root certificate has been generated
      Console.WriteLine("Generating Robot Certificates");
      var robots = _context.Robots.ToList();
      foreach (var robot in robots)
      {
        // Generate Certificate
        var certificate = GenerateRobotCertificate(robot.Id);
        _context.RobotCertificates.Add(new RobotCertificate {
          Id = Guid.NewGuid(),
          Thumbprint = certificate.RobotCertificateThumbprint,
          ThumbprintBackup = null,
          NotBefore = certificate.RobotCertificateNotBefore.ToUniversalTime(),
          NotAfter = certificate.RobotCertificateNotAfter.ToUniversalTime(),
          RobotId = robot.Id
        });

        // Save Certificate
        var publicKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "Certs", $"{robot.Name}.crt");
        File.WriteAllText(publicKeyPath, certificate.RobotCertificatePublicKey);
        var privateKeyPath = Path.Combine(Directory.GetCurrentDirectory(), "Certs", $"{robot.Name}.key");
        File.WriteAllText(privateKeyPath, certificate.RobotCertificatePrivateKey);
      }
      _context.SaveChanges();
    }
    Console.WriteLine("Initialise Data Completed");

    Environment.Exit(0);
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}

internal class CertificateDetail
{
}