using Microsoft.IdentityModel.Tokens;
using Report.Data.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Report.API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            var referer = context.Request.Headers["Referer"].FirstOrDefault();

           
            if (path.StartsWithSegments("/api"))
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Sistemə icazəniz yoxdur");
                    return;
                }

                if (token != null && !AttachUserToContext(context, token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Sessiya müddəti bitmişdir.");
                    return;
                }
            }

            await _next(context);
        }

       
        private bool AttachUserToContext(HttpContext context, string token)
        {
            var jwt = EnvironmentHelper.GetJwt();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key ?? string.Empty));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudiences = new List<string> {jwt.Audience },

                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var identity = new ClaimsIdentity(jwtToken.Claims);
                var principal = new ClaimsPrincipal(identity);

                context.User = principal;
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
