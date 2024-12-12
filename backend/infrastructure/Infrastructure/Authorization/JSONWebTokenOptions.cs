using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentScheduler.Infrastructure.Authorization;

public sealed class JSONWebTokenOptions
{
    public const string Id = "id";
    public const string UserName = JwtRegisteredClaimNames.Sub;
    public const string FullName = JwtRegisteredClaimNames.Name;
    public const string PhoneNumber = JwtRegisteredClaimNames.PhoneNumber;
    public const string TokenId = JwtRegisteredClaimNames.Jti;
    public const string Permissions = "permission";

    private const int ExpirationMinutes = 500;
    public string SymmetricSecurityKey { get; set; } = "cff934f839bf0d6d56c7eb4d3b2551a73d24b6be939b5d8dfbbad66780dd08950126ea65913b07a49c846df50c62155844e8d27afb57456b0bc3d25da175193c9ee5d028450b07e2b4f1bcf2597c6a2084b5457ce801fd7097f3b43fd02821566ad7a72e97b98130ea6fce2651e726ff9cefb486bad9eed29aa9f27c5972d71b647ed022117379aea483bd916a4e95b55a932e314426091a761ce9c06c1781a6e79756bbadb4e4c66988442aa206554cde20b072d4d582d684a7ac9aad9ebf88cefe584ee641743f81d85914e885423cf57ad3e78f3ac6aa7659d04efa3f01a88e09c1c72b4347a2f03f294690ab6448842b2603dfe25c9d11d259c165b00acf64e8a360977f65d407ed3d30f6b97803a8ec7415643eb99a0aaeda4c7efbf8620ac3228cd94665d8137748c10b8707ce3c1d327d07f65dc132a0ee13a0579c3ff17fa0e6119b4fa7ec02b1c9a6a8a3b3c8712b27830c58365a2c4f3c14de9735fa18b82a6256f9fbacf4e1f386465248046d7e4635b27e5efcc7000053714552";
    public string GetJSONWebToken(IUser user, IRepository repository, ILogger logger = null)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (repository == null) throw new ArgumentNullException(nameof(repository));
        if (!repository.TryGetKeyOf(user, out string id)) return null;
        var claims = new List<Claim> {
            new(Id, id),
            new(UserName, user.UserName),
            new(FullName, user.FullName),
            new(PhoneNumber, user is IPatient p ? p.Phone : user is IDoctor d ? d.Phone : ""),
            new(TokenId, Guid.NewGuid().ToString())
        };
        foreach (var permission in user.Role.Permissions)
            claims.Add(new(Permissions, Enum.GetName(permission) ?? ""));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(ExpirationMinutes),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SymmetricSecurityKey)), SecurityAlgorithms.HmacSha384)
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        logger?.LogInformation("JSON Web Token created");

        return tokenHandler.WriteToken(token);
    }
}