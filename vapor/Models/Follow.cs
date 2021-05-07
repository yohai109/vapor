using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Follow
    {
        public string? followingCustomerId { get; set; }
        public Customer followingCustomer { get; set; }
        public string? followedCustomerId { get; set; }
        public Customer followedCustomer { get; set; }
    }
}
