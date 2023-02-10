using Entities;
using ServiceContracts.DTO;
using ServiceContracts.Interfaces;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countryList = new();

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            var country = countryAddRequest.ToCountry();

            if (country.Name == null)
                throw new ArgumentException(nameof(country));

            if (_countryList.Any(c => c.Name == country.Name))
                throw new ArgumentException("Country name already exists");

            _countryList.Add(country);

            return CountryExtensions.ToCountryResponse(country);
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countryList.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryById(Guid id)
        {
            var country = _countryList.Find(country => country.Id == id);

            return country?.ToCountryResponse();
        }
    }
}