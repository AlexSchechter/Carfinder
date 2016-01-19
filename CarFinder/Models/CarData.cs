using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarFinder.Models
{
    public class CarData
    {
        public Car car { get; set; }
        public Recalls recalls { get; set; }
        public string[] imageURLs { get; set; }
    }
}