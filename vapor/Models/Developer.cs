using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class Developer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string fileContentType { get; set; }

        public ICollection<Game> games { get; set; }
    }
}