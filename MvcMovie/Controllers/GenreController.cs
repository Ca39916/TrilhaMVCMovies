using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMovie.ViewModels;

using MvcMovie.Models;

namespace MvcMovie.Controllers
{
   public class GenreController : Controller
   {
      private MovieDBContext movieDb = new MovieDBContext();

      // GET: Genre
      public ActionResult Index()
      {
         var genres = movieDb.Genres.OrderBy(g => g.Name);
         return View(genres.ToList());
      }

        public ActionResult About()
        {
            var data = from movie in movieDb.Movies group movie by movie.Genre into dateGroup
                select new GenreDataInfo()
                {
                GenreID = dateGroup.Key.GenreID,
                GenreName = dateGroup.Key.Name,
                GenreCount = dateGroup.Count(),
                TotalGross = dateGroup.Sum(m => m.Gross),
                AverageRating = dateGroup.Average(m => m.Rating)
                };
            return View(data.OrderByDescending(g=>g.GenreCount).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            movieDb.Dispose();
            base.Dispose(disposing);
        }
    }

    
}