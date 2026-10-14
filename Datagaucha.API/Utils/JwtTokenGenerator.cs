/* Copyright (c) 2026 David-irusta
    This code is licensed under the MIT License (see LICENSE file for details) */

using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;

public class JwtTokenGenerator
{
    private readonly IConfiguration configuration;
	
	public JwtTokenGenerator(IConfiguration configuration)
	{
		this.configuration = configuration;
	}

    public string GenerateToken(long userId)
    {
        var secretKey = configuration["Jwt:Key"] ?? string.Empty;
        var issuer = configuration["Jwt:Issuer"] ?? string.Empty;
        var audience = configuration["Jwt:Audience"] ?? string.Empty;
        var expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(descriptor);

        return token;
    }
}