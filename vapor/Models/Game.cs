using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Game
    {
        public string id { get; set; }
        public Developer developer { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime release_date { get; set; }
    }
}
