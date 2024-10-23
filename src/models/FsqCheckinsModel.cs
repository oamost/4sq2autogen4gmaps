namespace FsqAutogen.Models
{

    // Automatically generated from sample at: https://json2csharp.com/
    // Validated with https://jsonformatter.org/json-pretty-print
    //
    using Newtonsoft.Json;

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    [JsonObject("Root")]
    public class FsqCheckinsModel
    {
        public Meta meta { get; set; }
        public List<Notification> notifications { get; set; }
        public Response response { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string name { get; set; }
        public string pluralName { get; set; }
        public string shortName { get; set; }
        public Icon icon { get; set; }
        public int categoryCode { get; set; }
        public string mapIcon { get; set; }
        public bool primary { get; set; }
    }

    public class Checkins
    {
        public int count { get; set; }
        public List<Item> items { get; set; }
    }

    public class Comments
    {
        public int count { get; set; }
    }

    public class Icon
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public int createdAt { get; set; }
        public string type { get; set; }
        public string visibility { get; set; }
        public int timeZoneOffset { get; set; }
        public Venue venue { get; set; }
        public Likes likes { get; set; }
        public bool like { get; set; }
        public bool isMayor { get; set; }
        public Photos photos { get; set; }
        public Posts posts { get; set; }
        public Comments comments { get; set; }
        public Source source { get; set; }
    }

    public class LabeledLatLng
    {
        public string label { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Likes
    {
        public int count { get; set; }
        public List<object> groups { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public List<LabeledLatLng> labeledLatLngs { get; set; }
        public string cc { get; set; }
        public string country { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
        public string requestId { get; set; }
    }

    public class Notification
    {
        public string type { get; set; }
        public Item item { get; set; }
    }

    public class Photos
    {
        public int count { get; set; }
        public List<object> items { get; set; }
    }

    public class Posts
    {
        public int count { get; set; }
        public int textCount { get; set; }
    }

    public class Response
    {
        public Checkins checkins { get; set; }
    }

    public class Source
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Venue
    {
        public string id { get; set; }
        public string name { get; set; }
        public Location location { get; set; }
        public List<Category> categories { get; set; }
        public int createdAt { get; set; }
    }
}
