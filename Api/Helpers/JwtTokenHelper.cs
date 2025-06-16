using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

public class JwtTokenHelper
{
  private readonly IConfiguration _configuration;

  public JwtTokenHelper(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string GenerateToken(string userId, string userName)
  {
    var jwtSettings = _configuration.GetSection("Jwt");

    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.Sub, userId),
      new Claim(JwtRegisteredClaimNames.UniqueName, userName),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: jwtSettings["Issuer"],
      audience: jwtSettings["Audience"],
      claims: claims,
      expires: DateTime.Now.AddDays(double.Parse(jwtSettings["ExpireDays"])),
      signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

}
