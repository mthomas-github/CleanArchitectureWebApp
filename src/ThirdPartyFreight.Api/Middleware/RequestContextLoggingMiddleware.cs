﻿using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace ThirdPartyFreight.Api.Middleware;

public class RequestContextLoggingMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    public RequestContextLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext)))
        {
            return _next(httpContext);
        }
    }

    private static string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}
