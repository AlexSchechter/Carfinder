using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarFinder.Models
{
    public class Recalls
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public RecallItem[] Results { get; set; }
    }
}



