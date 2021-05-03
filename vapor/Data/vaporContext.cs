﻿using System;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>().HasKey(table => new {
                table.customerID,
                table.gameID
            });
        }

        public DbSet<vapor.Models.Game> Game { get; set; }

        public DbSet<vapor.Models.Order> Order { get; set; }

        public DbSet<vapor.Models.Customer> Customer { get; set; }
        public DbSet<vapor.Models.Developer> Developer { get; set; }
    }
}