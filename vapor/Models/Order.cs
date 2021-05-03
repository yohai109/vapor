using System;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Order
    {
        [Key]
        public Game game { get; set; }
        [Key]
        public Customer customer { get; set; }
        public DateTime date { get; set; }
    }
}
