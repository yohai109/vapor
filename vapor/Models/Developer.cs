using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Developer
    {
        public string id { get; set; }
        public string name { get; set; }
        public ICollection<Game> games { get; set; }
    }
}
