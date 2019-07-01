using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharterFlightsMVC.ViewModels
{
    public class FlightOfferViewModel
    {

        public string Origin { get; set; }
        public string Destination { get; set; }        
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public int Stops { get; set; }
        public string Currency { get; set; }
        public string TotalPrice { get; set; }

        
    }
}
