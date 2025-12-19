using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CartPricing.Api.Middleware
{
    public sealed class ErrorHandlingMiddleware
    {
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false
        };

        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.BadRequest, "ARGUMENT_OUT_OF_RANGE", ex.Message);
            }
            catch (ArgumentException ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.BadRequest, "ARGUMENT_ERROR", ex.Message);
            }
            catch (Exception ex)
            {
                await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "INTERNAL_SERVER_ERROR", ex.Message);
            }
        }

        private static Task WriteErrorAsync(HttpContext context, HttpStatusCode status, string code, string message)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)status;
                context.Response.ContentType = "application/json";
            }

            var payload = new { code, message, traceId = context.TraceIdentifier };
            var json = JsonSerializer.Serialize(payload, JsonOpts);
            return context.Response.WriteAsync(json);
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseStandardErrorHandling(this IApplicationBuilder app)
            => app.UseMiddleware<ErrorHandlingMiddleware>();
    }

}
