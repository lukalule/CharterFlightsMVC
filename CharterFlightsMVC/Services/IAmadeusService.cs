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
        Task<HttpResponseMessage> QueryFlights(string origin, string destination, string departureDate, string returnDate, string currency,
                                                       string adults, string children, string infants, string seniors);

        Task SetNewAccessToken();
    }
}
