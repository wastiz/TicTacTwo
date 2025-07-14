using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Hubs;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _jwtSecretKey;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _jwtSecretKey = configuration["Jwt:Key"];
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();
        if (path.StartsWith("/css") || path.StartsWith("/js") || 
            path.StartsWith("/images") || path.StartsWith("/lib") || 
            path == "/index" || path.StartsWith("/register"))
        {
            await _next(context);
            return;
        }

        var token = context.Request.Cookies["authToken"];

        if (!string.IsNullOrEmpty(token))
        {
            var claimsPrincipal = ValidateJwtToken(token, _jwtSecretKey);

            if (claimsPrincipal != null)
            {
                context.User = claimsPrincipal;
                if (path == "/")
                {
                    context.Response.Redirect("/");
                    return;
                }
            }
            else
            {
                context.Response.Redirect("/login");
                return;
            }
        }
        else
        {
            context.Response.Redirect("/login");
            return;
        }

        await _next(context);
    }

    private ClaimsPrincipal ValidateJwtToken(string token, string secretKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
            return null;
        }
    }
}
