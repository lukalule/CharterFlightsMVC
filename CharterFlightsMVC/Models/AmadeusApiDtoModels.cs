using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CharterFlightsMVC.Models
{
    public partial class FlightOffers
    {
        [JsonProperty("data")]
        public List<FlightOffer> Data { get; set; }

        [JsonProperty("dictionaries")]
        public Dictionaries Dictionaries { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }
    }

    public partial class FlightOffer
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("offerItems")]
        public List<OfferItem> OfferItems { get; set; }
    }

    public partial class OfferItem
    {
        [JsonProperty("services")]
        public List<Service> Services { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("pricePerAdult")]
        public Price PricePerAdult { get; set; }
    }

    public partial class Price
    {
        [JsonProperty("total")]
        public string Total { get; set; }

        [JsonProperty("totalTaxes")]
        public string TotalTaxes { get; set; }
    }

    public partial class Service
    {
        [JsonProperty("segments")]
        public List<Segment> Segments { get; set; }
    }

    public partial class Segment
    {
        [JsonProperty("flightSegment")]
        public FlightSegment FlightSegment { get; set; }

        [JsonProperty("pricingDetailPerAdult")]
        public PricingDetailPerAdult PricingDetailPerAdult { get; set; }
    }

    public partial class FlightSegment
    {
        [JsonProperty("departure")]
        public Arrival Departure { get; set; }

        [JsonProperty("arrival")]
        public Arrival Arrival { get; set; }

        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("aircraft")]
        public Aircraft Aircraft { get; set; }

        [JsonProperty("operating")]
        public Operating Operating { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }
    }

    public partial class Aircraft
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public partial class Arrival
    {
        [JsonProperty("iataCode")]
        public string IataCode { get; set; }

        [JsonProperty("at")]
        public DateTimeOffset At { get; set; }

        [JsonProperty("terminal", NullValueHandling = NullValueHandling.Ignore)]
        public string Terminal { get; set; }
    }

    public partial class Operating
    {
        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }
    }

    public partial class PricingDetailPerAdult
    {
        [JsonProperty("travelClass")]
        public string TravelClass { get; set; }

        [JsonProperty("fareClass")]
        public string FareClass { get; set; }

        [JsonProperty("availability")]
        public long Availability { get; set; }

        [JsonProperty("fareBasis")]
        public string FareBasis { get; set; }
    }

    public partial class Dictionaries
    {
        [JsonProperty("carriers")]
        public Carriers Carriers { get; set; }

        [JsonProperty("currencies")]
        public Currencies Currencies { get; set; }

        [JsonProperty("aircraft")]
        public Dictionary<string, string> Aircraft { get; set; }

        [JsonProperty("locations")]
        public Dictionary<string, Location> Locations { get; set; }
    }

    public partial class Carriers
    {
        [JsonProperty("6X")]
        public string The6X { get; set; }

        [JsonProperty("DY")]
        public string Dy { get; set; }
    }

    public partial class Currencies
    {
        [JsonProperty("BAM")]
        public string Bam { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("subType")]
        public string SubType { get; set; }

        [JsonProperty("detailedName")]
        public string DetailedName { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("defaults")]
        public Defaults Defaults { get; set; }
    }

    public partial class Defaults
    {
        [JsonProperty("nonStop")]
        public bool NonStop { get; set; }

        [JsonProperty("adults")]
        public long Adults { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }
    }

}
