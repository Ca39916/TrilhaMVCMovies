using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class Log
    { 
        
        [Key]
        public DateTime dataHora { get; set; }
        public string operacao { get; set;}
        public string operacaoRealizada { get; set; }
        

    }
}