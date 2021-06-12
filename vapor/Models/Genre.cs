using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string id { get; set; }
        public string name { get; set; }

        public virtual ICollection<Game> games { get; set; }
    }
}