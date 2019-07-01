using CharterFlightsMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CharterFlightsMVC
{
    public class ValidateAccessTokenHandler : DelegatingHandler
    {
        private IMemoryCache Cache;
        public ValidateAccessTokenHandler(IMemoryCache memoryCache)
        {
            Cache = memoryCache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!Cache.TryGetValue("amadeusTokenEntry", out DateTimeOffset tokenEntry))
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(
                    "You must supply a valid access token.")
                };

                return response;
            }
            

            return await base.SendAsync(request, cancellationToken);
        }


    }

}
