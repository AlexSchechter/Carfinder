using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarFinder.Models
{
    public class CarsDb : DbContext
    {
        public CarsDb() : base("DefaultConnection")
        {
        }

        public static CarsDb Create()
        {
            return new CarsDb();
        }
    }
}