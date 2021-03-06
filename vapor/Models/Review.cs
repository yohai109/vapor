using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string id { get; set; }
        public string customerId { get; set; }
        public Customer cusotmer { get; set; }
        public string gameId { get; set; }
        public Game game { get; set; }
        [Required(ErrorMessage = "A score is required")]
        public float rating { get; set; } = 1;
        public string comment { get; set; }
        public DateTime writtenAt { get; set; }
        public DateTime lastUpdate { get; set; }

    }
}
