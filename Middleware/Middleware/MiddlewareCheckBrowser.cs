using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware.Middleware
{
    public class MiddlewareCheckBrowser : IMiddleware
    {
        private readonly IBrowserDetector _browserDetector;
        public MiddlewareCheckBrowser(IBrowserDetector browserDetector)
        {
            _browserDetector = browserDetector;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var browser = _browserDetector.Browser;
            if (!ChcekTheBrowserSupported(browser.Name))
            {
                await context.Response.WriteAsync("Browser not supported");
            }
            await next.Invoke(context);
        }

        /// <summary>
        /// Check that the browser is supported.
        /// </summary>
        /// <param name="browser"></param>
        /// <returns> If browser is supported return true if not return false. </returns>
        private bool ChcekTheBrowserSupported(string browser)
        {
            if (browser == BrowserNames.InternetExplorer || browser == BrowserNames.Edge || browser == BrowserNames.EdgeChromium)
                return false;
            return true;
        }
    }

    public static class MiddlewareCheckBrowserExtetions
    {
        public static IApplicationBuilder UseMiddlewareCheckBrowser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddlewareCheckBrowser>();
        }
    }
}
