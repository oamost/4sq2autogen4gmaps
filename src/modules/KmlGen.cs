using FsqAutogen.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

namespace FsqAutogen
{
    /// <summary>
    /// 
    /// 
    ///This utility grabs checkin data from foursquare and drops it into a .kml file.
    /// Then, this file can easily be used at "Google My Maps", see: https://www.google.com/maps/d/u/0/?hl=en
    ///
    ///USAGE-WARNING:
    ///
    ///         - foursquare does limit their API usage on free tier - this is up to approx. 200k requests per month. 
    ///         - example: if you have 25 000 check-ins, then one launch will cost you 100 requests (1 request gets 250 check-ins)
    ///
    ///SIZE-RESTRICTIONS:
    ///
    ///         - My Google Maps has a .kml size restriction up to 5 MB
    ///         - This code doesn't include duplicates (based on (x,y) coords) to be included in the result set
    ///
    ///</summary>    
    ///
    public class KmlGen
    {
        /// <summary>
        /// 
        ///     Generate Google Maps compatible KML file from the user's Foursquare checkin history
        ///     
        /// </summary>
        /// 
        public async Task<bool> ProcessCheckins()
        {
            bool result = false;

            UserInputModel input = GetUserInput();
            string token = await GetOauthAccessToken(input);
            List<string> checkins = await GetCheckins(token);
            result = GenerateKmlFile(checkins);

            return result;
        }

        /// <summary>
        /// 
        ///     Get neccessary input to interact with the Foursquare API
        ///     
        /// </summary>
        //
        private UserInputModel  GetUserInput()
        {
            var result = new UserInputModel();

            Console.WriteLine("To get started, launch Authenticate.html\n");

            Console.WriteLine("\nPaste client id:");
            result.ClientId = Console.ReadLine();

            Console.WriteLine("\nPaste client secret:");
            result.ClientSecret = Console.ReadLine();

            Console.WriteLine("\nPaste access code:");
            result.AccessCode = Console.ReadLine();


            Console.WriteLine("\nUser input ready...");
            return result;
        }

        /// <summary>
        /// 
        ///     Get the OAuth access token
        ///     
        /// </summary>
        ///     
        private async Task<string> GetOauthAccessToken(UserInputModel input)
        {
            string? result = string.Empty;
            Console.WriteLine("Obtaining access token...");

            string oauth_token_uri = @$"https://foursquare.com/oauth2/access_token?client_id={input.ClientId}&client_secret={input.ClientSecret}&grant_type=authorization_code&redirect_uri=https://www.example.com&code={input.AccessCode}";

            using (HttpClient httpClient = new HttpClient()) 
            {
                    HttpResponseMessage httpResponse = await httpClient.GetAsync(oauth_token_uri);
                    httpResponse.EnsureSuccessStatusCode();

                    string oauth_token_json = await httpResponse.Content.ReadAsStringAsync();
                    var jobject = JObject.Parse(oauth_token_json);

                    result = jobject["access_token"]?.ToString();
            }        

            Console.WriteLine("\nObtained OAuth access token...");
            return result;
        }

        /// <summary>
        /// 
        ///     Fetch all the checkin history of the currently logged in Foursquare user
        ///     
        /// </summary>
        //
        private async Task<List<string>> GetCheckins(string token)
        {
            var result = new List<string>();

            Console.WriteLine("\nFetching checkin history...");
            
            int currentOffset = 0;
            const int queryLimit = 250;
            const int apiVersion = 20231010;                
            var latlonCache = new HashSet<string>();

            while (true)
            {
                var checkins_endpoint_uri = new RestClientOptions(@$"https://api.foursquare.com/v2/users/self/checkins?v={apiVersion}&limit={queryLimit}&offset={currentOffset}&oauth_token={token}");
                var client = new RestClient(checkins_endpoint_uri);

                var request = new RestRequest("");        
                request.AddHeader("accept", "application/json");
                
                var response = await client.GetAsync(request);
                string? checkinsJson = response.Content?.ToString();

                if (checkinsJson == null)
                    return null;

                FsqCheckinsModel? foursquareCheckinsRootResponseObject = JsonConvert.DeserializeObject<FsqCheckinsModel>(checkinsJson);
                List<Item>? checkins = foursquareCheckinsRootResponseObject?.response.checkins.items;
                
                if (checkins == null)
                    return null;

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
                        entry = string.Format(KmlSchemaModel.KmlEntry, name, longitude, latitude);

                    if (entry != string.Empty)
                    {
                        string key = string.Concat(latitude,",",longitude);

                        // No need to add duplicates
                        //
                        if (!latlonCache.Contains(key))
                        {
                            result.Add(entry);
                            latlonCache.Add(key);
                        }
                            
                    }                    
                }

                if (checkins?.Count < queryLimit)
                    break;

                currentOffset += queryLimit;
            }

            Console.WriteLine("\nFinished feching checkin history.");
            return result;
        }

        /// <summary>
        /// 
        ///     Generate the KML file. Currently includes the following data points per entry:
        ///     
        ///             - Checkin -> Venue -> Name;
        ///             - Checkin -> Venue -> Location -> Latitude;
        ///             - Checkin -> Venue -> Location -> Longitude;
        ///             
        ///     Update this list when the relevant code changes.
        ///     
        /// </summary>
        //
        private bool GenerateKmlFile(List<string> checkins)
        {
            bool result = false;
            Console.WriteLine("\nGenerating KML file...");

            if (checkins == null || checkins.Count == 0)
            {
                Console.WriteLine("An error occured. Reason: invalid checkins list.");
                return result;
            }

            //  Generating files in chunks of 2000 entries:
            int entriesPerFile = 2000;
            int totalFiles = (checkins.Count + entriesPerFile - 1) / entriesPerFile; // Calculate the number of files needed

            for (int fileIndex = 0; fileIndex < totalFiles; fileIndex++)
            {
                string content = string.Empty;
                
                // Get the range of entries for this file
                int start = fileIndex * entriesPerFile;
                int count = Math.Min(entriesPerFile, checkins.Count - start);
                List<string> currentEntries = checkins.GetRange(start, count);

                // Concatenate the entries into the KML content
                foreach (string entry in currentEntries)
                    content += entry;

                // Prepare the document for this file
                string document = string.Empty;
                string target = $"4sq2autogen4gmaps_{DateTime.Now:yyyy_MM_dd}_{fileIndex + 1}.kml";
                
                // Add header, content, and footer to the KML file
                document += KmlSchemaModel.KmlHeader;
                document += content;
                document += KmlSchemaModel.KmlFooter;

                // Write the file to the disk
                File.WriteAllText(target, document);

                Console.WriteLine($"KML file '{target}' generated successfully.");
                result = true;
            }

            Console.WriteLine("KML generation completed.");

            return result;
        }
    }
}