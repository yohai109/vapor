using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public Developer developer { get; set; }
        public double price { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime releaseDate { get; set; }

        public virtual ICollection<GameImage> images { get; set; }
        public virtual ICollection<Review> reviews { get; set; }
        public virtual ICollection<Genre> generes { get; set; }
    }
}
