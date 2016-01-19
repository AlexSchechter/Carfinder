using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarFinder.Models
{
    public class Recalls
    {
        int Count { get; set; }
        string Message { get; set; }
        public RecallItem[] Results { get; set; }
    }
}



