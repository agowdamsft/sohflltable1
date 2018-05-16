using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamRatingsAPI
{
   public partial class POS
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("details")]
        public Detail[] Details { get; set; }

    }

    public partial class Detail
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("unitCost")]
        public string UnitCost { get; set; }

        [JsonProperty("totalCost")]
        public string TotalCost { get; set; }

        [JsonProperty("totalTax")]
        public string TotalTax { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("productDescription")]
        public string ProductDescription { get; set; }
    }

    public partial class Header
    {
        [JsonProperty("salesNumber")]
        public string SalesNumber { get; set; }

        [JsonProperty("dateTime")]
        public DateTimeOffset DateTime { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("locationAddress")]
        public string LocationAddress { get; set; }

        [JsonProperty("locationPostcode")]
        public string LocationPostcode { get; set; }

        [JsonProperty("totalCost")]
        public string TotalCost { get; set; }

        [JsonProperty("totalTax")]
        public string TotalTax { get; set; }
    }
}
