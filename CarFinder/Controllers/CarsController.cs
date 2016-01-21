using System.Web.Http;
using CarFinder.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Net;

namespace CarFinder.Controllers
{
    public class CarsController : ApiController
    {
        // declare a new instance of your database context (connection object)
        CarsDb db = new CarsDb();
        /// <summary>
        /// Retrieves a list of available car model years
        /// </summary>
        /// <returns>IEnumerable list of model years</returns>
        // GET: api/Cars/Years
        // custom URL routing modification using an action name 
        [ActionName("Years")]
        // Here we are protecting our requests from conflicting with each other by making 
        //	asynchronous queries. We can talk more about this tomorrow.
        public async Task<IHttpActionResult> GetYears()
        {
            // execute the stored procedure in the database and get back a result set (list)
            var retval = await db.Database.SqlQuery<string>("EXEC GetYears").ToListAsync();
            // if the result is empty, return nor found, otherwise, return ok with the data
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }

        /// <summary>
        /// Retrieves a list of makes for a  model year
        /// </summary>
        /// <param name="year"></param>
        /// <returns>Enumerable list of makes for a model year</returns>
        // GET: api/Cars/MakesByYear
        [ActionName("MakesByYear")]
        public async Task<IHttpActionResult> GetMakesByYear(string year) 
        {
            // remember the additional sql parameters to the stored procedure
            var retval = await db.Database.SqlQuery<string>("EXEC GetMakesByYear @year", new SqlParameter("year", year)).ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }
        /// <summary>
        /// Retrieves a list of models for a year and a make
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <returns>Enumerable list of models for a year and a make</returns>
        [ActionName("ModelsByYearAndMake")]
        public async Task<IHttpActionResult> GetModelsByYearAndMake(string year, string make)
        {
            var retval = await db.Database.SqlQuery<string>("EXEC GetModelsByYearAndMake @year, @make", new SqlParameter("year", year), new SqlParameter("make", make)).ToListAsync();
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }
        /// <summary>
        /// Retrieves a list of trims for a year, a make and a model
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>Enumerable list of trims for a year, a make and a model</returns>
        [ActionName("TrimsByYearMakeAndModel")]
        public async Task<IHttpActionResult> GetTrimsByYearMakeAndModel(string year, string make, string model)
        {
            var retval = await db.Database.SqlQuery<string>("Exec GetTrimsByYearMakeAndModel @year, @make, @model", new SqlParameter("year", year), new SqlParameter("make", make), 
                new SqlParameter("model", model)).ToListAsync(); 
            if (retval != null)
                return Ok(retval);
            else
                return NotFound();
        }
        /// <summary>
        /// Retrieves a car for a year, a make, a model and a trim
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="trim"></param>
        /// <returns>A carData object for a year, a make, a model and a trim</returns>
        [ActionName("Cardata")]
        public async Task<IHttpActionResult> GetCars(string year, string make, string model, string trim)
        {
            var carData = new CarData();
            carData.Car = await db.Database.SqlQuery<Car>("EXEC GetAllByYearMakeModelAndTrim @year, @make, @model, @trim", new SqlParameter("year", year),
                new SqlParameter("model", model), new SqlParameter("make", make), new SqlParameter("trim", trim)).FirstAsync();

            carData.Recalls = GetRecalls(year, make, model);
            carData.ImageURLs = GetImages(year, make, model, trim);
            return Ok(carData);
        }
        /// <summary>
        /// Retrieves a car for a year, a make, a model (overload if no trim options available)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <returns>A carData object for a year, a make, a model and a trim</returns>
        [ActionName("Cardata")]
        public async Task<IHttpActionResult> GetCars(string year, string make, string model)
        {
            var carData = new CarData();
            carData.Car = await db.Database.SqlQuery<Car>("EXEC GetAllByYearMakeAndModel @year, @make, @model", new SqlParameter("year", year),
                new SqlParameter("model", model), new SqlParameter("make", make)).FirstAsync();

            carData.Recalls = GetRecalls(year, make, model);
            carData.ImageURLs = GetImages(year, make, model);
            return Ok(carData);
        }

        private Recalls GetRecalls(string year, string make, string model)
        {
            HttpResponseMessage response;
            Recalls recalls;
            //HttpClient (and certain other web request objects) are not neccessarily disposable objects, unlike HttpResponseMessage, which is. A best practice is to wrap such
            //resources within a using statement to allow early and determined cleanup. 
            using (var client = new HttpClient())
            {
                //1. establish base client address
                client.BaseAddress = new System.Uri("http://www.nhtsa.gov/");
              
                try
                {
                    //2. make request to specific URL on the client
                    response = client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + year + "/make/" + make + "/model/" + model + "?format=json").Result;

                    //3. construct a Recalls object from the resulting JSON data
                    recalls = response.Content.ReadAsAsync<Recalls>().Result; 

                }
                catch
                {
                    return null;
                }

            }
            return recalls;
        }

        private string[] GetImages(string year, string make, string model, string trim)
        {
            string query = string.Concat(year, " ", make, " ", model, " ", trim);

            // Create a Bing container.
            string rootUri = "https://api.datamarket.azure.com/Bing/Search";
            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));

            // My account key.
            var accountKey = "SFqfTsHuISE5EHYum81ONhrG5Eji5oaqZqGmv2QwjjM=";

            // Configure bingContainer to use my credentials.
            bingContainer.Credentials = new NetworkCredential(accountKey, accountKey);
        
            var imageResults = bingContainer.Image(query, null, null, null, null, null, null)// Build the query but do not executy the query
                                .AddQueryOption("$top", 5)//limit images returned to 5
                                .Execute();//execute the query         
            List<string> images = new List<string>();
            foreach (var results in imageResults)
            {
                images.Add(results.Thumbnail.MediaUrl);
            }
            return images.ToArray();
        }
        private string[] GetImages(string year, string make, string model)
        {
            string query = string.Concat(year," ", make, " ", model);
            string rootUri = "https://api.datamarket.azure.com/Bing/Search";
            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));
            var accountKey = "SFqfTsHuISE5EHYum81ONhrG5Eji5oaqZqGmv2QwjjM=";
            bingContainer.Credentials = new NetworkCredential(accountKey, accountKey);

            var imageResults = bingContainer.Image(query, null, null, null, null, null, null).AddQueryOption("$top", 5).Execute();
            List<string> images = new List<string>();
            foreach (var results in imageResults)
            {
                images.Add(results.Thumbnail.MediaUrl);
            }
            return images.ToArray();
        }
    }
}