using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class MapCoordinates
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        [Required]

        public string name { get; set; }
        [Required]

        public double latitude { get; set; }
        [Required]

        public double longitude { get; set; }
    }
}
