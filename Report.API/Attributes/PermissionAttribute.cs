using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Report.Application.ExceptionHandle.Models;
using Report.Core.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Report.API.Attributes
{
    public class PermissionAttribute : ActionFilterAttribute
    {
        private readonly ClaimEnum[] _claims;
        public PermissionAttribute(params ClaimEnum[] claims) =>
       _claims = claims;
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                SetForbiddenResult(context);
                return;
            }

            var roleClaim = identity.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            if (string.IsNullOrEmpty(roleClaim)|| !Enum.TryParse(roleClaim, out ClaimEnum claim) ||
                       !_claims.Contains(claim))
            {
                SetForbiddenResult(context);
                return;
            }

            await next();
        }

        private void SetForbiddenResult(ActionExecutingContext context)
        {
            throw new ForbiddenException("Əməliyyatı icra etməyə icazəniz yoxdur.");
            //context.Result = new ObjectResult(new { Message = "Access forbidden" })
            //{
            //    StatusCode = StatusCodes.Status403Forbidden
            //};
        }
    }
}