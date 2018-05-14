namespace IceCreamRatingsAPI
{
    public class Rating
    {
        public Rating()
        {
        }
        public string id { get;  set; }
        public string userid { get;  set; }
        public string productid { get;  set; }
        public string timestamp { get;  set; }
        public string locationname { get;  set; }
        public int? rating { get;  set; }
        public string usernotes { get;  set; }
    }
}