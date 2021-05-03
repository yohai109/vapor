using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vapor.Models;

namespace vapor.Data
{
    public class vaporContext : DbContext
    {
        public vaporContext (DbContextOptions<vaporContext> options)
            : base(options)
        {
        }

        public DbSet<vapor.Models.Game> Game { get; set; }
    }
}
