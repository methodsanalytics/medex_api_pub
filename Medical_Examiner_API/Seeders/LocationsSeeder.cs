using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Medical_Examiner_API.Models;
using Medical_Examiner_API.Persistence;
using Newtonsoft.Json;

namespace Medical_Examiner_API.Seeders
{
    /// <summary>
    /// Adds location objects to database from source file
    /// </summary>
    public class LocationsSeeder
    {
        private ILocationsSeederPersistence _locationSeederPersistence;

        /// <summary>
        /// Constructor
        /// </summary>
        /// /// <param name="locationSeederPersistence">persistence object that writes to persistence destination</param>
        public LocationsSeeder(ILocationsSeederPersistence locationSeederPersistence)
        {
            _locationSeederPersistence = locationSeederPersistence;
        }

        /// <summary>
        /// List of location objects that this class will create
        /// </summary>
        public List<Location> Locations { get; private set; }

        /// <summary>
        /// Create Locations from file
        /// </summary>
        /// <param name="jsonFileName">full name of source path</param>
        public void LoadFromFile(string jsonFileName)
        {
            var json = File.ReadAllText(jsonFileName);
            Locations = JsonConvert.DeserializeObject<List<Location>>(json);
            InferParent();
        }

        /// <summary>
        /// Write locations to the data layer
        /// </summary>
        public void SubmitToDataLayer()
        {
            _locationSeederPersistence.SaveAllLocationsAsync(Locations);
        }

        /// <summary>
        /// Determine parent org of each location based on format of code
        /// </summary>
        private void InferParent()
        {
            foreach (var location in Locations)
            {
                var code = location.Code;

                //code length of 5 indicates site. Parent will be trust, as determined by first 3 characters of code
                if (code.Length == 5)
                {
                    location.Parent = code.Substring(0, 3);
                }
            }
        }
    }
}
