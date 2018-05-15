using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamRatingsAPI
{
    public partial class PO
    {
        [JsonProperty("ponumber")]
        public string PoNumber { get; set; }

        [JsonProperty("datetime")]
        public string Datetime { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("locationaddress")]
        public string Locationaddress { get; set; }

        [JsonProperty("locationpostcode")]
        public string Locationpostcode { get; set; }

        [JsonProperty("totalcost")]
        public double Totalcost { get; set; }

        [JsonProperty("totaltax")]
        public double Totaltax { get; set; }

        [JsonProperty("lineItems")]
        public LineItem[] LineItems { get; set; }
    }

    public partial class LineItem
    {
        [JsonProperty("ponumber")]
        public string PoNumber { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("productDescription")]
        public string ProductDescription { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("unitcost")]
        public double Unitcost { get; set; }

        [JsonProperty("totalcost")]
        public double Totalcost { get; set; }

        [JsonProperty("totaltax")]
        public double Totaltax { get; set; }
    }
}
