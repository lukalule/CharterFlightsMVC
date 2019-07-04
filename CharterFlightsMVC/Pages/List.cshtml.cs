using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CharterFlightsMVC.Services;
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
        
        

        public void OnGet()
        {
            
        }
    }
}