using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceCreamRatingsAPI
{
    public partial class Sentiment
    {
        [JsonProperty("documents")]
        public DocumentSentiment[] Documents { get; set; }
    }

    public partial class DocumentSentiment
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public partial class SentimentReturn
    {
        [JsonProperty("documents")]
        public DocumentReturn[] Documents { get; set; }

        [JsonProperty("errors")]
        public object[] Errors { get; set; }
    }

    public partial class DocumentReturn
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
