using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CharterFlightsMVC.Services;
using CharterFlightsMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Globalization;
using CharterFlightsMVC.Models;

namespace CharterFlightsMVC.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IAmadeusService amadeusService;
        public FlightsController(IAmadeusService service) => amadeusService = service;
        
        public IActionResult Index()
        {           
            return View();
        }

        [ResponseCache(Duration = 1200, VaryByQueryKeys = new string[] { "*" })] //cuva jedinstveni query 20 minuta
        public async Task<IActionResult> FilterFlights(string origin, string destination, string departureDate, string returndate, string currency)
        {
            string originCode = origin.Trim().Substring(0, 3);
            string destinationCode = destination.Trim().Substring(0, 3);
            string departure = departureDate.Substring(6, 4) + "-" + departureDate.Substring(0, 2) + "-" + departureDate.Substring(3, 2);
            string returnDate = returndate != null ? returnDate = returndate.Substring(6, 4) + "-" + returndate.Substring(0, 2) + "-" + returndate.Substring(3, 2) : null;
            
            var httpRresponse = await amadeusService.QueryFlights(originCode, destinationCode, departure, currency);

            if (httpRresponse.IsSuccessStatusCode)
            {
                var responseAsString = await httpRresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var response =  JsonConvert.DeserializeObject<FlightOffers>(responseAsString);
                DateTimeOffset.TryParse(returnDate, out DateTimeOffset dateOfReturn);
                List<FlightOffer> pom = new List<FlightOffer>();
                if (returnDate != null)
                    response.Data = response.Data.Where(o => o.OfferItems.FirstOrDefault().Services.FirstOrDefault().Segments.LastOrDefault().FlightSegment.Arrival.At.Date == dateOfReturn.Date).ToList();

                List<FlightOfferViewModel> flightOffersViewModel = new List<FlightOfferViewModel>();

                foreach (var flightOffer in response.Data)
                {
                    var segments = flightOffer.OfferItems.FirstOrDefault().Services.FirstOrDefault().Segments;
                    flightOffersViewModel.Add(new FlightOfferViewModel()
                    {
                        Origin = segments.FirstOrDefault().FlightSegment.Departure.IataCode,
                        DepartureTime = segments.FirstOrDefault().FlightSegment.Departure.At.ToString("dddd, MMM dd yyyy HH:mm:ss(UTC) zzz"),

                        Destination = segments.LastOrDefault().FlightSegment.Arrival.IataCode,
                        ArrivalTime = segments.LastOrDefault().FlightSegment.Arrival.At.ToString("dddd, MMM dd yyyy HH:mm:ss(UTC) zzz"),

                        TotalPrice = flightOffer.OfferItems.FirstOrDefault().Price.Total,
                        Stops = segments.Count - 1,
                        Currency = response.Meta.Currency
                    });
                }

                return PartialView("_FlightsTableContent", flightOffersViewModel);
            }
            else if(httpRresponse.StatusCode == System.Net.HttpStatusCode.NotFound || httpRresponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return PartialView("_ItineraryNotFound");
            }

            return PartialView("_InternalServerError");
        }

        public JsonResult Select2Data(string searchTerm)
        {
            searchTerm = NormalizeString(searchTerm);
            JArray jArray = JArray.Parse(System.IO.File.ReadAllText("IataCodes/AllCodes.json"));
            IEnumerable<IataCodeDto> filteredItems = new List<IataCodeDto>();

            if (!String.IsNullOrEmpty(searchTerm))
            {
                List<IataCodeDto> items = JsonConvert.DeserializeObject<List<IataCodeDto>>(jArray.ToString());
                filteredItems = items.Where(x => NormalizeString(x.text).Contains(searchTerm));
            }

            return Json(filteredItems);
        }        

        public static string NormalizeString(string str)
        {
            string normal = str.Normalize(NormalizationForm.FormD);
            var withoutDiacritics = normal.Where(
                c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            string final = new string(withoutDiacritics.ToArray());
            if (final != str)
                str = final;
            return str.ToLower();
        }
        
    }
}