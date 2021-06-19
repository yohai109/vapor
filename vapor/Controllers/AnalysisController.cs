using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vapor.Data;

namespace vapor.Controllers
{
    [Authorize(Roles = "Admin")]
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
            var searchResult = from g in _context.Game
                               select new { date = g.releaseDate.Date };

            return Json(searchResult);
        }

        public ActionResult GetOrdersAnalysis()
        {
            var searchResult = from order in _context.Order
                               select new { date = order.date.Date };

            return Json(searchResult);
        }

        public ActionResult GetGameReviewAnalysis()
        {
            /* (from a in context.Accounts
               join c in context.Clients on a.UserID equals c.UserID
               where c.ClientID == yourDescriptionObject.ClientID
               select a.Balance)
             */

            var searchResult = from game in _context.Game
                               join review in _context.Review on game.id equals review.gameId into groupReviews
                               from groupReview in groupReviews.DefaultIfEmpty()
                               group groupReview by game.name into groupReviewes
                               select new
                               {
                                   name = groupReviewes.Key,
                                   averageRating = groupReviewes.Average(r => r == null ? 0 : r.rating),
                               };

            return Json(searchResult);
        }
    }
}
