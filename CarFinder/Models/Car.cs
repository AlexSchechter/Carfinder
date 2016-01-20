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
        public string Model_trim { get; set; }
        public string Model_year { get; set; }
        public string Body_style { get; set; }
        public string Engine_cc { get; set; }
        public string Engine_num_cyl { get; set; }
        public string Engine_power_ps { get; set; }
        public string Doors { get; set; }
        public string Engine_valves_per_cyl { get; set; }
        public string Engine_position { get; set; }
        public string Engine_fuel { get; set; }
        public string Top_speed_kph { get; set; }
        public string Zero_to_100_kph { get; set; }
        public string Drive_type { get; set; }
        public string Transmission_type { get; set; }
        public string Seats { get; set; }
        public string Weight_kg { get; set; }
        public string Length_mm { get; set; }
        public string Width_mm { get; set; }
        public string Height_mm { get; set; }
        public string Wheelbase { get; set; }
        public string Fuel_capacity_l { get; set; }
        public string CO2 { get; set; }
    }
}