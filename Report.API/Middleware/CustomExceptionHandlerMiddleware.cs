using Report.Application.ExceptionHandle;
using Report.Application.ExceptionHandle.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Report.API.Middleware
{
    public class CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                DeleteImportExcel();
                await HandleExceptionAsync(context, exception);
            }

        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = exception switch
            {
                ForbiddenException => StatusCodes.Status403Forbidden,
                DatabaseException => StatusCodes.Status500InternalServerError,
                BadRequestException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError,
            };
            context.Response.StatusCode = code;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(exception.Message);

        }

        private void DeleteImportExcel()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "import.xlsx");
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
