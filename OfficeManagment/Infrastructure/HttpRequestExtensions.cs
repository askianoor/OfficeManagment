﻿using Microsoft.AspNetCore.Http;

namespace OfficeManagment.Infrastructure
{
    public static class HttpRequestExtensions
    {
        //Add GetEtagHandler to Http Request
        public static IEtagHandlerFeature GetEtagHandler(this HttpRequest request)
                    => request.HttpContext.Features.Get<IEtagHandlerFeature>();
    }
}
