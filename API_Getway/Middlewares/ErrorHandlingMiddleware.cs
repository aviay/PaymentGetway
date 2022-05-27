using System.Net;
using System.Text;

namespace API_Getway.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _log;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string requestBody = string.Empty;
            try
            {

                requestBody = await FormatRequest(httpContext.Request);
                await _next(httpContext);
            }
            catch (InvalidOperationException ex)
            {
                _log.LogError(ex, "Validation error");
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _log.LogError($"{Environment.NewLine}{Environment.NewLine}--------------{Environment.NewLine} General Error. HResult : {ex.HResult}.  Request: {requestBody}{Environment.NewLine}{ex.ToString()}");                
                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, string error, HttpStatusCode httpStatusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;
            return context.Response.WriteAsync(string.Empty);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;
            return $"{request.Method} - {request.Scheme}://{request.Host}{request.Path} {request.QueryString} {Environment.NewLine}{bodyAsText}";
        }

    }
}
