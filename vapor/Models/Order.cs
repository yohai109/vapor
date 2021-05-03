using System;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Order
    {
        
        public Game game { get; set; }
        public string gameID { get; set; }
        
        public Customer customer { get; set; }
        public string customerID { get; set; }
        public DateTime date { get; set; }
    }
}
