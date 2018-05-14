namespace IceCreamRatingsAPI
{
    public class Rating
    {
        public Rating()
        {
        }
        public string id { get;  set; }
        public string userid { get; internal set; }
        public string productid { get; internal set; }
        public string timestamp { get; internal set; }
        public string locationname { get; internal set; }
        public int? rating { get; internal set; }
        public string usernotes { get; internal set; }
    }
}