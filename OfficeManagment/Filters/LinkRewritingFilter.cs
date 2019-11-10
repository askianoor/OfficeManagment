using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using OfficeManagment.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OfficeManagment.Filters
{
    /// <summary>
    /// Handling Normal Responses
    /// </summary>
    public class LinkRewritingFilter : IAsyncResultFilter
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public LinkRewritingFilter(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var asObjectResult = context.Result as ObjectResult;
            //Check the asObjectResult to make sure it is not null or does not have an ok resposne
            bool shouldSkip = asObjectResult?.Value == null || asObjectResult?.StatusCode != (int)HttpStatusCode.OK;

            if (shouldSkip)
            {
                await next();
                return;
            }

            var rewriter = new LinkRewriter(_urlHelperFactory.GetUrlHelper(context));
            RewriteAllLinks(asObjectResult.Value, rewriter);

            await next();
        }

        private static void RewriteAllLinks(object model, LinkRewriter rewriter)
        {

        }

    }
}
