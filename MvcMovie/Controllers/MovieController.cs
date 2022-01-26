using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcMovie.Models;
using MvcMovie.Filters;


using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MovieController : Controller
    {
        private MovieDBContext movieDb = new MovieDBContext();

        // GET: Movie
        public ViewResult Index(string searchString, int? SelectedGenre, string sortOrder)//
        {
            var genres = movieDb.Genres.OrderBy(g => g.Name).ToList();
            ViewBag.SelectedGenre = new SelectList(genres, "GenreID", "Name", SelectedGenre);
            int genreID = SelectedGenre.GetValueOrDefault();

            var movies = movieDb.Movies
                .Where(c => !SelectedGenre.HasValue || c.GenreID == genreID);

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString) || s.Director.Contains(searchString));
            }

            //variavel temporaria usada para enviar dados da controler para tela  
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_asc" : "Name";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title_asc" : "Title";
            ViewBag.ReleaseDateSortParm = sortOrder == "ReleaseDate" ? "ReleaseDate_asc" : "ReleaseDate";
            ViewBag.DirectorSortParm = sortOrder == "Director" ? "Director_asc" : "Director";
            ViewBag.GrossSortParm = sortOrder == "Gross" ? "Gross_asc" : "Gross";
            ViewBag.RatingSortParm = sortOrder == "Rating" ? "Rating_asc" : "Rating";


            //sortOrder variavel que recebe a informaçao que o usuario digitou 
            switch (sortOrder)
            {
                case "Genre.Name": //orderByDescending ordena por ordem decrecente
                    movies = movies.OrderByDescending(s => s.Genre.Name);
                    break;

                case "Genre.Name_asc": //ordena por ordem crescente
                    movies = movies.OrderBy(s => s.Genre.Name);
                    break;

                case "Title":
                    movies = movies.OrderByDescending(s => s.Title);
                    break;

                case "Title_asc":
                    movies = movies.OrderBy(s => s.Title);
                    break;


                case "ReleaseDate":
                    movies = movies.OrderByDescending(s => s.ReleaseDate);
                    break;

                case "ReleaseDate_asc":
                    movies = movies.OrderBy(s => s.ReleaseDate);
                    break;


                case "Director":
                    movies = movies.OrderByDescending(s => s.Director);
                    break;

                case "Director_asc":
                    movies = movies.OrderBy(s => s.Director);
                    break;


                case "Gross":
                    movies = movies.OrderByDescending(s => s.Gross);
                    break;

                case "Gross_asc":
                    movies = movies.OrderBy(s => s.Gross);
                    break;


                case "Rating":
                    movies = movies.OrderByDescending(s => s.Rating);
                    break;

                case "rating_asc":
                    movies = movies.OrderBy(s => s.Rating);
                    break;

            }
            return View(movies);

        }

        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = movieDb.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // GET: Movie/Create

        public ActionResult Create()
        {
            ViewBag.GenreID = new SelectList(movieDb.Genres, "GenreID", "Name");
            return View();
        }

        // POST: Movie/Create
        [Authorize]  //Annotation Authorize= usuario precisa estar logado para executar a action  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Director,ReleaseDate,Gross,Rating,GenreID")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                movieDb.Movies.Add(movie);
                movieDb.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenreID = new SelectList(movieDb.Genres, "GenreID", "Name", movie.GenreID);
            return View(movie);
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int? id) //Action que exibe a tela de alteração do filme seleciona 
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                throw new HttpException(500, "Id is required.");
            }

            Movie movie = movieDb.Movies.Find(id);
            if (movie == null)
            {
                //return HttpNotFound();
                throw new HttpException(404, "Invalid Id");

            }
            ViewBag.GenreID = new SelectList(movieDb.Genres, "GenreID", "Name", movie.GenreID);
            return View(movie);

        }


        // POST: Movie/Edit/5  exercicio 2.11
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionFilter]
        public ActionResult edit(int id , string title , string director, //Action que valida e salva no banco de dados as alterações do cadstro do filme realizadas pelo usuario 
            DateTime releaseDate, decimal gross,
                double rating , string imageUrl, int genreID,
                HttpPostedFileBase image)
        {
            var movie = movieDb.Movies.Find(id);
            if(ModelState. IsValid && movie != null)
            {
                movie.Title = title;
                movie.Director = director;
                movie.ReleaseDate = releaseDate;
                movie.Gross = gross; movie.Rating
                = rating; movie.ImageUrl =
               imageUrl; movie.GenreID =
               genreID;

                //if objeto de imagem não está vazio atualize a foto atribute , usando image info
                if(image != null )
{
                    movie.ImageMimeType = image.ContentType;
                    movie.ImageFile = new byte[image.ContentLength];
                    // salva o arquivo de foto usando o método image.InputStream.Read
                    image.InputStream.Read(movie.ImageFile, 0, image.ContentLength);

                }
                movieDb.Entry(movie).State = EntityState.Modified;
                movieDb.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.GenreID = new SelectList(movieDb.Genres, "GenreID", "Name", movie.GenreID);
            return View(movie);
                       
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = movieDb.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [Authorize(Users = "admin@mvc.br")] // 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = movieDb.Movies.Find(id);
            movieDb.Movies.Remove(movie);
            movieDb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                movieDb.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Catalogo(string titulo)
        {
            string filePath = Server.MapPath("~/Catalogo/") + titulo.ToLower() + ".pdf";
            if (System.IO.File.Exists(filePath))
                return new FilePathResult(filePath, "application/pdf");
            else return HttpNotFound();
        }
        public JsonResult Filmes()
        {
            // atenção: este código é apenas um exemplo; 
            // ver possíveis vulnerabilidades em:
            // http://msdn.microsoft.com/query/dev11.query?appId=Dev11IDEF1&l=EN-US&k=k(System.Web.Mvc.JsonRequestBehavior);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv4.5);k(DevLang-csharp)&rd=true 
            // http://haacked.com/archive/2008/11/20/anatomy-of-a-subtle-json-vulnerability.aspx
            // http://msdn.microsoft.com/en-us/library/hh404095.aspx

            var model = from movie in movieDb.Movies
                        select new
                        {
                            Titulo = movie.Title,
                            Diretor = movie.Director,
                            Ano = movie.ReleaseDate.Year,
                            Genero = movie.Genre.Name
                        };

            return Json(model.OrderBy(m => m.Ano), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Browse(string genre = "Action")
        {
            var genreModel = movieDb.Genres.Include("Movies").Single(g => g.Name == genre);

            return View(genreModel);
        }

        //exercicio 6 gitimage
        public ActionResult GetImage(int id)
        {
            Movie movie = movieDb.Movies.Find(id);
            if (movie != null  && movie.ImageFile != null)
            {
                return File(movie.ImageFile, movie.ImageMimeType);
            }
            else
            {
                return new FilePathResult (" ~/ Images/ nao-disponivel. jpg", "image/ jpeg");
      
            }
        }

        // GET: /Movie/GenreMenu
        [ChildActionOnly]
        public ActionResult GenreMenu(int num = 5)  /*1.6 retorna uma lista parcial de gêneros*/
        {
            var genres = movieDb.Genres
            .OrderByDescending(g => g.Movies.Count)
            .Take(num)
            .ToList();
            return this.PartialView(genres);
        }
        //metodo autocomplete, necessário tambem, incluir o script na view
        public ActionResult MovieFilter(string term)
        {
            term = term.ToLower();
            var movies = from movie in movieDb.Movies
                         where (movie.Title.ToLower().Contains(term))
                         select movie.Title;
            return Json(movies, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserData()
        {
            if (!Request.IsAuthenticated)
            {
                return Content("Not Authenticated");
            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var userManager = new UserManager<ApplicationUser>(store); ApplicationUser user =
            userManager.FindByNameAsync(User.Identity.Name).Result;
            return Content("Id: " + User.Identity.Name + " Name: " + user.FullName);
        }


    }

}
