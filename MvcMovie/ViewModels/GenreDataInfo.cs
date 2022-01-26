using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace MvcMovie.ViewModels
{
    public class GenreDataInfo
    {
        [Key]
        public int GenreID { get; set; }

        [Display(Name = "Genre")]
        public String GenreName { get; set; }

        [Display(Name = "Count")]
        public int GenreCount { get; set; }

        [Display(Name = "Total Gross")]
        [DataType(DataType.Currency)]
        public decimal TotalGross { get; set; }

        [Display(Name = "Average Rating")]
        [DisplayFormat(DataFormatString = "{ 0 : . 0 }")]
        public double AverageRating { get; set; }
    }
}