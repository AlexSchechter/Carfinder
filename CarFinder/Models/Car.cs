using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace CarFinder.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model_name { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Model_trim { get; set; }
        public string Model_year { get; set; }
        public string Body_style { get; set; }
        public string Engine_cc { get; set; }
        public string Engine_num_cyl { get; set; }
        public string Engine_power_ps { get; set; }
        public string Doors { get; set; }
        public string Make_display { get; set; }
    }
}