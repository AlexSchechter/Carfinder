using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarFinder.Models
{
    public class CarData
    {
        public Car Car { get; set; }
        public Recalls Recalls { get; set; }
        public string[] ImageURLs { get; set; }
    }
}