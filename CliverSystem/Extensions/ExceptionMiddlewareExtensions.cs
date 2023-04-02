﻿using CliverSystem.Error;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CliverSystem.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app,
       ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                    {
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";

                        if (contextFeature != null)
                        {
                            logger.LogError($"Something went wrong: {contextFeature.Error}");

                            var errorDetail = contextFeature.Error as ApiException;
                            string message = "Internal Server Error.";

                            if (errorDetail != null)
                            {
                                context.Response.StatusCode = errorDetail.StatusCode;
                                message = errorDetail.Message;
                            }

                            if (contextFeature.Error is DbUpdateException updateErr)
                            {
                                message = updateErr?.InnerException?.Message ?? updateErr?.Message ?? "";
                            }

                            Exception errorRes = new ApiException(message)
                            {
                                StatusCode = context.Response.StatusCode
                            };
                            await context.Response.WriteAsync(errorRes
                          .ToString());
                        }
                    });
            });
        }

    }
}
