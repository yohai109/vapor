using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string id { get; set; }
        public Customer cusotmer { get; set; }
        public Game game { get; set; }
        public float rating { get; set; }
        public string comment { get; set; }
        public DateTime writtenAt { get; set; }
        public DateTime lastUpdate { get; set; }

    }
}
