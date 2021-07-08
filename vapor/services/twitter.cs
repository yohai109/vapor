using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using vapor.Models;

namespace vapor.services
{
    public class twitter
    {
        private HttpClient client;
        private string standard_url;
        private string url;
        private RestClient restClient;

        public twitter()
        {
            client = new HttpClient();
            standard_url = "https://v1.nocodeapi.com/vapor/twitter/CLXSGtoiwdnjxnnA";

            url = "https://app.ayrshare.com/api/post";
            restClient = new RestClient("https://app.ayrshare.com/api/post");

        }

        public void postTweet(Game newGame)
        {
            string status = "A brand new game on vapor: " + newGame.name + " Developed by: " + newGame.developer.name;
            postTweet(status);
        }
        public void postTweet(Developer newDeveloper)
        {
            string status = "Welcome to the brand new developer on vapor: " + newDeveloper.name.ToString();
            postTweet(status);
        }

        public async void postTweet(string status)
        {
            //Alternative Way
            restClient.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer C0BJX5N-8WC4BHC-HM83R64-141206Y");
            var body = "{\"post\": \"" + status + "\", \"platforms\": [\"twitter\"]}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = restClient.Execute(request);
            Console.WriteLine(response.Content);

            //Standard Way
            var values = new Dictionary<string, string> { };
            var content = new FormUrlEncodedContent(values);

            var standard_response = await client.PostAsync(standard_url + "?status=" + status, content);

            var responseString = await standard_response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);

        }
}
}
