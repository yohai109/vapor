using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Customer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }

        public virtual ICollection<Review> reviews { get; set; }
        public virtual ICollection<Order> orders { get; set; }
    }
}
