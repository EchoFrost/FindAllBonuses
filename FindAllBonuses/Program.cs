using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FindAllBonuses
{
    /// <summary>
    /// Executes all operations for finding all global map bonuses.
    /// </summary>
    public static class Program
    {
        public static readonly RestClient restClient = new RestClient("http://kztimerglobal.com/api/v2.0");

        /// <summary>
        /// Code entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            // Fetches the user's global API key. Possibly helps with rate limiting?
            var apiKey = Environment.GetEnvironmentVariable("GLOBAL_API_KEY");

            // Build a request to get all global maps. There are hidden response size limits that setting an arbitrarily big number for limit seems to resolves.
            var globalMapRequest = new RestRequest("maps", DataFormat.Json).AddQueryParameter("limit", "9999");

            // If we found an API key, add it to our request headers.
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                globalMapRequest.AddHeader("X-ApiKey", apiKey);
            }

            // Execute the API request.
            var globalMapRequestResponse = restClient.Get(globalMapRequest);

            // Deserialize the response into a list of map objects.
            var globalMaps = JsonConvert.DeserializeObject<List<MapModel>>(globalMapRequestResponse.Content);

            // Create a new list to store response objects in.
            var responseList = new List<ResponseModel>();

            // Iterate over the global maps.
            foreach (var map in globalMaps)
            {
                // Build a request to get all record filters for the map. There are hidden response size limits that setting an arbitrarily big number for limit seems to resolves.
                var recordFilterRequest = new RestRequest("record_filters", DataFormat.Json).AddQueryParameter("limit", "9999").AddQueryParameter("map_ids", map.id.ToString());

                // If we found an API key, add it to our request headers.
                if (!string.IsNullOrWhiteSpace(apiKey))
                {
                    recordFilterRequest.AddHeader("X-ApiKey", apiKey);
                }

                // Execute the API request.
                var recordFilterRequestRespoonse = restClient.Get(recordFilterRequest);

                // Deserialize the response into a list of map filter objects.
                var mapFilters = JsonConvert.DeserializeObject<RecordFilterModel[]>(recordFilterRequestRespoonse.Content);

                // Calculate the number of bonuses the map has. We subtract 1 because we don't want to count the map itself- which is stage 0 in our data.
                var number_of_bonuses = mapFilters.Where(r => r.map_id == map.id).GroupBy(x => x.stage).Count() - 1;

                // Build a response object.
                var response = new ResponseModel { MapId = map.id, MapName = map.name, NumberOfBonuses = number_of_bonuses };

                // Add the response object to our response list.
                responseList.Add(response);

                // If there are bonuses for the map, print out the properties of our response object.
                if (number_of_bonuses > 0)
                {
                    Console.WriteLine(response.MapId + " " + response.MapName + " " + response.NumberOfBonuses);
                }

                // The API will rate limit us if we hit it too fast. This adds a second delay between requests. I know it makes things slow. Sorry :(
                Thread.Sleep(1000);
            }

            // Calculate and print out the total number of maps with bonuses.
            Console.WriteLine("Total maps with bonuses: " + responseList.Where(m => m.NumberOfBonuses > 0).Count());

            // Calculate and print out the total number of global bonuses.
            Console.WriteLine("Total bonuses: " + responseList.Select(m => m.NumberOfBonuses).Sum());
        }
    }
}
