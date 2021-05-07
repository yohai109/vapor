using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class GameImage
    {
        public string id { get; set; }
        public string imageUrl { get; set; }
        public Game game { get; set; }
    }
}
