using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace vapor.Models
{
    public class GameImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public string fileContentType { get; set; }
        public string fileBase64 { get; set; }
        [Required]
        public string gameID { get; set; }
        public Game game { get; set; }
    }
}
