using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using vapor.Models;
namespace vapor.DAL
{
    public class VaporInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<VaporContext>
    {
        protected override void Seed(VaporContext context)
        {
            var developers = new List<Developer>
            {
            new Developer{name="Arad", avatar="HackerMan"},
            new Developer{name="Yoav", avatar="YoafOvershoes"},
            };

            developers.ForEach(s => context.Developers.Add(s));
            context.SaveChanges();
        }
    }
}