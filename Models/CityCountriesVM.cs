using System.Collections.Generic;

namespace GlobalCityManager.Models
{
    public class CityCountriesVM{
        public City City { get; set; }
        public IList<Country> Countries { get; set; }
    }
}