using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharterFlightsMVC.Services;
using CharterFlightsMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CharterFlightsMVC.Pages
{
    public class ListModel : PageModel
    {
        private readonly IAmadeusService AmadeusService;
        public ListModel(IAmadeusService service)
        {
            AmadeusService = service;
        }

        [BindProperty(SupportsGet = true)]
        public string Origin { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Destination { get; set; }
        [BindProperty(SupportsGet = true)]
        public string DepartureDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ReturnDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public string NoAdults { get; set; }
        [BindProperty(SupportsGet = true)]
        public string NoChildren { get; set; }
        [BindProperty(SupportsGet = true)]
        public string NoInfants { get; set; }
        [BindProperty(SupportsGet = true)]
        public string NoSeniors { get; set; }

        public IEnumerable<FlightOfferViewModel> Itineraries { get; set; }
        public void OnGet()
        {
            
        }
    }
}