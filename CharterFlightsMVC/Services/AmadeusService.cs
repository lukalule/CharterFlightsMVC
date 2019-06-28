using CharterFlightsMVC.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CharterFlightsMVC.Services
{
    public class AmadeusService : IAmadeusService
    {
        private IMemoryCache Cache;
        public HttpClient Client;

        public AmadeusService(HttpClient client, IMemoryCache memoryCache)
        {
            client.BaseAddress = new Uri("https://test.api.amadeus.com/v1/");
            client.Timeout = TimeSpan.FromMinutes(3);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            Client = client;
            Cache = memoryCache;
        }        

        public async Task<HttpResponseMessage> QueryFlights(string origin, string destination, string departureDate, string currency)
        {
            var queryString = $"shopping/flight-offers?origin={origin}&destination={destination}&departureDate={departureDate}&currency={currency}";

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(queryString, UriKind.Relative),
                Method = HttpMethod.Get
            };

            if (Cache.TryGetValue("amadeusTokenEntry", out DateTimeOffset tokenEntryTime) && Cache.TryGetValue("tokenValue", out string tokenValue))
            {
                if (DateTimeOffset.UtcNow - tokenEntryTime >= TimeSpan.FromMinutes(28))
                {
                    await SetNewAccessToken().ConfigureAwait(false);
                    return await Client.SendAsync(request).ConfigureAwait(false);
                }

                Client.SetBearerToken(tokenValue);
                return await Client.SendAsync(request).ConfigureAwait(false);
            }

            await SetNewAccessToken().ConfigureAwait(false);

            return await Client.SendAsync(request).ConfigureAwait(false);
        }

        public async Task SetNewAccessToken()
        {
            var accessToken = await Client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://test.api.amadeus.com/v1/security/oauth2/token",

                ClientId = "RoiPpDkfENNjKwGoAeUibmpyHpHt5eHL",
                ClientSecret = "jCsYYGeoYTGydwmK",
                Scope = "api1"
            });

            Cache.Set("amadeusTokenEntry", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(30));
            Cache.Set("tokenValue", accessToken.AccessToken, DateTimeOffset.UtcNow.AddMinutes(60));

            Client.SetBearerToken(accessToken.AccessToken);
        }

        

    }
}
