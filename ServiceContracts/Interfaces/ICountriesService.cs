using ServiceContracts.DTO;

namespace ServiceContracts.Interfaces
{
	/// <summary>
	/// Represents business logic for manipulating Country entity
	/// </summary>
	public interface ICountriesService
	{
		/// <summary>
		/// Adds a country object to the list of countries
		/// </summary>
		/// <param name="countryAddRequest">Country object to add</param>
		/// <returns>Returns the country object after adding it</returns>
		CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

		/// <summary>
		/// Get all countries on the list
		/// </summary>
		/// <returns>Returns the list of all countries</returns>
		List<CountryResponse> GetAllCountries();

		/// <summary>
		/// Returns a country object based on the given country id
		/// </summary>
		/// <param name="id">The id that will be searched</param>
		/// <returns>Matching country as CountryResponse object</returns>
		CountryResponse? GetCountryById(Guid id);
	}
}