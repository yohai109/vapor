using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using vapor.Models;

namespace vapor.services
{
    public class twitter
    {
        private HttpClient client;
        private string url;


        public twitter()
        {
            client = new HttpClient();
            url = "https://v1.nocodeapi.com/vapor/twitter/qKGXDIYbBwqJkPxZ";
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
            var values = new Dictionary<string, string> { };
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(url + "?status=" + status, content);

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseString);
        }
    }
}
