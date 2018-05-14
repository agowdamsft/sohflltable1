using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamRatingsAPI
{
    class Helpers
    {
    }

    public partial class iRating
    {
        [JsonProperty("userid")]
        public string Userid { get; set; }

        [JsonProperty("productid")]
        public string Productid { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("rating")]
        public string Rating { get; set; }

        [JsonProperty("locationname")]
        public string Locationname { get; set; }

        [JsonProperty("usernotes")]
        public string Usernotes { get; set; }
    }

   

}
