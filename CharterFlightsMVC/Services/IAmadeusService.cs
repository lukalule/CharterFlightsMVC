using CharterFlightsMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CharterFlightsMVC.Services
{
    public interface IAmadeusService
    {
        Task<HttpResponseMessage> QueryFlights(string origin, string destination, string departureDate, string currency);

        Task<FlightOffers> SendRequestForFlightsAsync(HttpRequestMessage request);

        Task SetNewAccessToken();
    }
}
