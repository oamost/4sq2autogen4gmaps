//
// This utility grabs checkin data from foursquare and drops it into a .kml file.
// Then, this file can easily be used at "Google My Maps", see: https://www.google.com/maps/d/u/0/?hl=en
//
// USAGE-WARNING:
//
//          - foursquare does limit their API usage on free tier - this is up to approx. 200k requests per month. 
//          - example: if you have 25 000 check-ins, then one launch will cost you 100 requests (1 request gets 250 check-ins)
//
// SIZE-RESTRICTIONS:
//
//          - My Google Maps has a .kml size restriction up to 5 MB
//          - This code doesn't include duplicates (based on (x,y) coords) to be included in the result set
//          
namespace _4sq2autogen4gmaps;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Retrieving access token:
        //    
        Console.WriteLine("To get started, launch Authenticate.html\n");

        Console.WriteLine("access code:");
        string? code = Console.ReadLine();

        Console.WriteLine("client id:");
        string? client_id = Console.ReadLine();

        Console.WriteLine("client secret:");
        string? client_secret = Console.ReadLine();

        string oauth_token_uri = @$"https://foursquare.com/oauth2/access_token?client_id={client_id}&client_secret={client_secret}&grant_type=authorization_code&redirect_uri=https://www.example.com&code={code}";
        string? oauth_token = string.Empty;

        using (HttpClient httpClient = new HttpClient()) 
        {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(oauth_token_uri);
                httpResponse.EnsureSuccessStatusCode();

                string oauth_token_json = await httpResponse.Content.ReadAsStringAsync();
                var jobject = JObject.Parse(oauth_token_json);

                oauth_token = jobject["access_token"]?.ToString();
        }        

        // Retrieving checkins:
        //
        int currentOffset = 0;

        const int queryLimit = 250;
        const int apiVersion = 20231010;

        string kmlHeader = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                                "<kml xmlns=\"http://www.opengis.net/kml/2.2\">\n" + 
                                    "<Document>\n" + 
                                        "<name>4sq2autogen4gmaps_{0}.kml</name>\n" + 
                                        "<Style id=\"icon-1899-097138-labelson-nodesc\">\n" + 
                                            "<IconStyle>\n" + 
                                                "<color>ff387109</color>\n" + 
                                                "<scale>1</scale>\n" + 
                                                "<Icon>\n" + 
                                                "<href>https://www.gstatic.com/mapspro/images/stock/503-wht-blank_maps.png</href>\n" + 
                                                "</Icon>\n" + 
                                                "<hotSpot x=\"32\" xunits=\"pixels\" y=\"64\" yunits=\"insetPixels\"/>\n" + 
                                            "</IconStyle>\n" + 
                                            "<BalloonStyle>\n" + 
                                                "<text><![CDATA[<h3>$[name]</h3>]]></text>\n" + 
                                            "</BalloonStyle>\n" + 
                                        "</Style>\n";

        // {0} :    Venue.name
        // {1} :    Location.lat
        // {2} :    Location.lng
        //
        string kmlEntry =   "<Placemark>\n" +
                                "<name>{0}</name>\n" +
                                "<styleUrl>#icon-1899-097138-labelson-nodesc</styleUrl>\n" +
                                "<Point>\n" +
                                    "<coordinates>\n" +
                                    "{1},{2}\n" +
                                    "</coordinates>\n" +
                                "</Point>\n" +
                            "</Placemark>\n";

        string kmlFooter =  "</Document>\n" + 
                        "</kml>\n";

        var kmlEntries = new List<string>();
        var latlonCache = new HashSet<string>();

        while (true)
        {
            var checkins_endpoint_uri = new RestClientOptions(@$"https://api.foursquare.com/v2/users/self/checkins?v={apiVersion}&limit={queryLimit}&offset={currentOffset}&oauth_token={oauth_token}");
            var client = new RestClient(checkins_endpoint_uri);

            var request = new RestRequest("");        
            request.AddHeader("accept", "application/json");
            
            var response = await client.GetAsync(request);
            string? checkinsJson = response.Content?.ToString();

            if (checkinsJson == null)
                return;

            Root? foursquareCheckinsRootResponseObject = JsonConvert.DeserializeObject<Root>(checkinsJson);
            List<Item>? checkins = foursquareCheckinsRootResponseObject?.response.checkins.items;
            
            if (checkins == null)
                return;

            if (checkins?.Count == 0)
                break;

            for (int i = 0; i < checkins?.Count; i++)
            {
                string? name = checkins?[i].venue?.name;
                double? latitude = checkins?[i].venue?.location?.lat;
                double? longitude = checkins?[i].venue?.location?.lng;

                string entry = string.Empty;

                // Remove '&' occurances from venue names
                // Google My Maps import doesn't seem favor it
                //
                if (name != null && name.Contains('&'))
                    name = name.Replace("&", "and");

                if (name != null && latitude != null && longitude != null)
                    entry = string.Format(kmlEntry, name, longitude, latitude);

                if (entry != string.Empty)
                {
                    string key = string.Concat(latitude,",",longitude);

                    // No need to add duplicates
                    //
                    if (!latlonCache.Contains(key))
                    {
                        kmlEntries.Add(entry);
                        latlonCache.Add(key);
                    }
                        
                }                    
            }

            if (checkins?.Count < queryLimit)
                break;

            currentOffset += queryLimit;
        }
        
        // Generating files in chunks of 2000 entries:
        int entriesPerFile = 2000;
        int totalFiles = (kmlEntries.Count + entriesPerFile - 1) / entriesPerFile; // Calculate the number of files needed

        for (int fileIndex = 0; fileIndex < totalFiles; fileIndex++)
        {
            string content = string.Empty;
            
            // Get the range of entries for this file
            int start = fileIndex * entriesPerFile;
            int count = Math.Min(entriesPerFile, kmlEntries.Count - start);
            List<string> currentEntries = kmlEntries.GetRange(start, count);

            // Concatenate the entries into the KML content
            foreach (string entry in currentEntries)
                content += entry;

            // Prepare the document for this file
            string document = string.Empty;
            string target = $"4sq2autogen4gmaps_{DateTime.Now:yyyy_MM_dd}_{fileIndex + 1}.kml";
            
            // Add header, content, and footer to the KML file
            document += kmlHeader;
            document += content;
            document += kmlFooter;

            // Write the file to the disk
            File.WriteAllText(target, document);

            Console.WriteLine($"KML file '{target}' generated successfully.");
        }

        Console.WriteLine("KML generation completed, press any key to exit...");
        Console.Read();
    }
}
