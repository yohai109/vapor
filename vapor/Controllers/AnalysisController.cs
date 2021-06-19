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
            var searchResult = from review in _context.Review
                               join game in _context.Game on review.gameId equals game.id into groupReviews
                               from groupReview in groupReviews.DefaultIfEmpty()
                               select new
                               {
                                   name = groupReview.name,
                                   averageRating = groupReview.reviews.Average(r => r.rating),
                               };

            return Json(searchResult);
        }
    }
}
