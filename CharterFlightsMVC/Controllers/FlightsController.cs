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

        [HttpGet]
        public async Task<IActionResult> QueryFlights(string origin, string destination, string departureDate, string returnDate, string currency,
                                                       string adults, string children, string infants, string seniors)
        {
            string originCode = origin.Trim().Substring(0, 3);
            string destinationCode = destination.Trim().Substring(0, 3);
            string departure = departureDate.Substring(6, 4) + "-" + departureDate.Substring(0, 2) + "-" + departureDate.Substring(3, 2);
            string returndate = DateTimeOffset.TryParse(returnDate, out DateTimeOffset rDate) ?
                                returndate = returnDate.Substring(6, 4) + "-" + returnDate.Substring(0, 2) + "-" + returnDate.Substring(3, 2) : null;

            var httpRresponse = await amadeusService.QueryFlights(originCode, destinationCode, departure, returndate, currency, adults, children, infants, seniors);

            if (httpRresponse.IsSuccessStatusCode)
            {
                var responseAsString = await httpRresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var response = JsonConvert.DeserializeObject<FlightOffers>(responseAsString);

                List<FlightOfferViewModel> flightOffersViewModel = new List<FlightOfferViewModel>();

                foreach (var flightOffer in response.Data)
                {
                    var services = flightOffer.OfferItems.FirstOrDefault().Services;
                    flightOffersViewModel.Add(new FlightOfferViewModel()
                    {
                        Origin = services.FirstOrDefault().Segments.FirstOrDefault().FlightSegment.Departure.IataCode,
                        DepartureDate = services.FirstOrDefault().Segments.FirstOrDefault().FlightSegment.Departure.At.ToString("dddd, MMM dd yyyy HH:mm:ss(UTC) zzz"),

                        Destination = services.FirstOrDefault().Segments.LastOrDefault().FlightSegment.Arrival.IataCode,
                        ReturnDate = !string.IsNullOrEmpty(returndate) ?
                        services.LastOrDefault().Segments.LastOrDefault().FlightSegment.Departure.At.ToString("dddd, MMM dd yyyy HH:mm:ss(UTC) zzz") : "",
                        TotalPrice = flightOffer.OfferItems.FirstOrDefault().Price.Total,
                        StopsDeparture = (services.FirstOrDefault().Segments.Count - 1).ToString(),
                        StopsReturn = services.Count() > 1 ? (services.LastOrDefault().Segments.Count - 1).ToString() : "",
                        Currency = response.Meta.Currency
                    });
                }

                return PartialView("_FlightsTableContent", flightOffersViewModel);
            }
            else if (httpRresponse.StatusCode == System.Net.HttpStatusCode.NotFound || httpRresponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return PartialView("_ItineraryNotFound");
            }

            return PartialView("_InternalServerError");
        }




        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new string[] { "*" })] //cuva jedinstveni query 20 minuta
        public async Task<IActionResult> FilterFlights(string origin, string destination, string departureDate, string returnDate, string currency, 
                                                       string adults, string children, string infants, string seniors)
        {
            string originCode = origin.Trim().Substring(0, 3);
            string destinationCode = destination.Trim().Substring(0, 3);
            string departure = departureDate.Substring(6, 4) + "-" + departureDate.Substring(0, 2) + "-" + departureDate.Substring(3, 2);
            string returndate = DateTimeOffset.TryParse(returnDate, out DateTimeOffset rDate) ? 
                                returndate = returnDate.Substring(6, 4) + "-" + returnDate.Substring(0, 2) + "-" + returnDate.Substring(3, 2) : null;
            
            var httpRresponse = await amadeusService.QueryFlights(originCode, destinationCode, departure, returndate, currency, adults, children, infants, seniors);

            if (httpRresponse.IsSuccessStatusCode)
            {
                var responseAsString = await httpRresponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var response =  JsonConvert.DeserializeObject<FlightOffers>(responseAsString);                

                List<FlightOfferViewModel> flightOffersViewModel = new List<FlightOfferViewModel>();

                foreach (var flightOffer in response.Data)
                {
                    var services = flightOffer.OfferItems.FirstOrDefault().Services;
                    flightOffersViewModel.Add(new FlightOfferViewModel()
                    {
                        Origin = services.FirstOrDefault().Segments.FirstOrDefault().FlightSegment.Departure.IataCode,
                        DepartureDate = services.FirstOrDefault().Segments.FirstOrDefault().FlightSegment.Departure.At.ToString("dddd, MMM dd yyyy HH:mm:ss(UTC) zzz"),

                        Destination = services.FirstOrDefault().Segments.LastOrDefault().FlightSegment.Arrival.IataCode,
                        ReturnDate = !string.IsNullOrEmpty(returndate) ?
                        services.LastOrDefault().Segments.LastOrDefault().FlightSegment.Departure.At.ToString("dddd, MMM dd yyyy HH:mm:ss(UTC) zzz") : "",
                        TotalPrice = flightOffer.OfferItems.FirstOrDefault().Price.Total,
                        StopsDeparture = (services.FirstOrDefault().Segments.Count - 1).ToString(),
                        StopsReturn = services.Count() > 1 ? (services.LastOrDefault().Segments.Count - 1).ToString() : "",
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
            if (!String.IsNullOrEmpty(searchTerm))
                searchTerm = NormalizeString(searchTerm);
            else
                return Json("");

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