using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vapor.Data;

namespace vapor.Controllers
{
    public class AnalysisController : Controller
    {

        private readonly vaporContext _context;

        public AnalysisController(vaporContext context)
        {
            _context = context;

        }
        // GET: AnalysisController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGamesAnalysis()
        {
            //var searchResult = _context.Game
            //    .GroupBy(g => g.releaseDate.Date).Count();

            var searchResult = _context.Game.GroupBy(g => g.releaseDate.Date).Select(g => new { 
                amount = g.Count(),
                relaseDate = g.OrderBy(p => p.releaseDate.Date)
            });

            return Json(searchResult);

            //    Select(g => new
            //{
            //    id = g.id,
            //    name = g.name,
            //    developer = g.developer.name,
            //    generes = g.generes,
            //    imageid = g.images.FirstOrDefault().id
            //});
            return View();
        }
    }
}
