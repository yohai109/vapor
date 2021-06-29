using System;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace vapor.Models
{
    public class Order
    {
        public string gameId { get; set; }
        public string customerId { get; set; }
        public DateTime date { get; set; }
        public Game game { get; set; }
        public Customer customer { get; set; }
    }
}
