using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MvcMovie.Filters     //
{
    public class ActionFilter : ActionFilterAttribute, IActionFilter
    {
        private MovieDBContext movieDb = new MovieDBContext();
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext); //OnActionExecuted = ao executar uma action grava no log
            {
                var log = new Log()
                {
                    operacao = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    operacaoRealizada = string.Concat(filterContext.ActionDescriptor.ActionName, " (Logged by: Action Filter)"),
                    dataHora = filterContext.HttpContext.Timestamp,
                    
                };
                movieDb.Log.Add(log); //add novo log
                movieDb.SaveChanges(); //Salva alteracoes do log no banco de dados 
                //OnActionExecuted(filterContext); //

            }

        }
    }
}