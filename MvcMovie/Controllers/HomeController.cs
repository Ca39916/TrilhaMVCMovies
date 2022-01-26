using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{

   public class HomeController : Controller
   {
        private MovieDBContext movieDb = new MovieDBContext();

      public ActionResult Index()
      {
         return View();
      }

      public ActionResult About()
      {
         ViewBag.Message = "Your application description page.";

         return View();
      }

      public ActionResult Contact()
      {
         ViewBag.Message = "Your contact page.";

         return View();
      }

      public void Agente()
      {
         string agent = Request.Browser.Browser;
         string useragent = Request.UserAgent;
         string source = Request.UserHostAddress;

         Response.Write("<H1>Hello MVC!</H1>");
         Response.Write("<h2>Welcome, you are using ");
         Response.Write(agent);
         Response.Write(" and your address is ");
         Response.Write(source);
         Response.Write("! </h2>");
         Response.Write("<h2>User agent: ");
         Response.Write(useragent + "</h2>");
      }

      public void HelloName(string name)
      {
         string value = HttpUtility.HtmlEncode(name);

         Response.Write("<H1>Hello ");
         Response.Write(value);
         Response.Write("!</H1>");
      }

      public void HelloId(string id)
      {
         string value = HttpUtility.HtmlEncode(id);

         Response.Write("<H1>Hello ");
         Response.Write(value);
         Response.Write("!</H1>");
      }

        public ActionResult Log()
        {
            var listalog = movieDb.Log.OrderBy(g => g.dataHora).ToList(); // lista de log ordenada pela data 
            return View(listalog);
        }

   }
}