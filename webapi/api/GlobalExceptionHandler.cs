using System;
using System.Net;
using Contracts;
using Entities.ErrorModel;
using Entities.Exceptions;
using Logger.Contract;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API;

public class GlobalExceptionHandler : IExceptionHandler
{

    private readonly ILoggerManager _logger;
    public GlobalExceptionHandler(ILoggerManager logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                Exception exception,
                                                CancellationToken cancellationToken)
    {

        _logger.LogError($"Exception occurred: {exception.Message}");
        var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

        if (contextFeature != null)
        {

            httpContext.Response.StatusCode = contextFeature.Error switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };


            var problemDetails = new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = contextFeature.Error.Message,
            };


            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        }
        return true;
    }
}