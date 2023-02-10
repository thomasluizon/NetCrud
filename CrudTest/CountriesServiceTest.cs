using ServiceContracts.DTO;
using ServiceContracts.Interfaces;
using Services;

namespace CrudTest
{
    public class CountriesServiceTest
	{
		private readonly ICountriesService _countriesService;

		public CountriesServiceTest()
		{
			_countriesService = new CountriesService();
		}

		#region AddCountry
		// When CountryAddRequest is null, it should throw ArgumentNullException
		[Fact]
		public void AddCountry_CountryIsNull()
		{
			CountryAddRequest? request = null;

			Assert.Throws<ArgumentNullException>(() =>
			{
				_countriesService.AddCountry(request);
			});
		}

		// When country name is null, it should throw ArgumentException
		[Fact]
		public void AddCountry_CountryNameIsNull()
		{
			var request = new CountryAddRequest() { Name = null };

			Assert.Throws<ArgumentException>(() =>
			{
				_countriesService.AddCountry(request);
			});
		}

		// When country name is duplicate, it should throw ArgumentException
		[Fact]
		public void AddCountry_CountryNameIsDuplicate()
		{
			var request = new CountryAddRequest() { Name = "USA" };
			var request2 = new CountryAddRequest() { Name = "USA" };

			Assert.Throws<ArgumentException>(() =>
			{
				_countriesService.AddCountry(request);
				_countriesService.AddCountry(request2);
			});
		}

		// When country is correct, it should add the country to the existing list of countries
		[Fact]
		public void AddCountry_CountryIsAdded()
		{
			var request = new CountryAddRequest() { Name = "USA" };
			CountryResponse response = _countriesService.AddCountry(request);

			Assert.True(response.Id != Guid.Empty);
		}
		#endregion

		#region GetAllCountries
		// When country is added, it has the correct properties
		[Fact]
		public void GetAllCountries_CorrectCountryDetails()
		{
			CountryAddRequest request = new CountryAddRequest() { Name = "Japan" };
			CountryResponse response = _countriesService.AddCountry(request);
			List<CountryResponse> countries = _countriesService.GetAllCountries();

			Assert.True(response.Id != Guid.Empty);
			Assert.Contains(response, countries);
		}

		// When no country has been added, returns an empty list
		[Fact]
		public void GetAllCountries_EmptyList()
		{
			var list = _countriesService.GetAllCountries();

			Assert.Empty(list);
		}

		// When countries are added, returns the correct list
		[Fact]
		public void GetAllCountries_AddsFewCountries()
		{
			var list = new List<CountryAddRequest>()
			{
				new CountryAddRequest()
				{
					Name = "USA"
				},
				new CountryAddRequest()
				{
					Name = "Brasil"
				}
			};

			var expectedList = new List<CountryResponse>();

			foreach (var country in list)
			{
				CountryResponse response = _countriesService.AddCountry(country);
				expectedList.Add(response);
			}

			var resultList = _countriesService.GetAllCountries();

			foreach (var country in expectedList)
			{
				Assert.Contains(country, resultList);
			}
		}
		#endregion

		#region GetCountryById
		// When country is not found, should return null
		[Fact]
		public void GetCountryById_CountryNotFound()
		{
			Assert.Null(_countriesService.GetCountryById(Guid.NewGuid()));
		}

		// When country is found, should return the country
		[Fact]
		public void GetCountryById_CorrectCountry()
		{
			CountryAddRequest request = new CountryAddRequest()
			{
				Name = "Japan"
			};

			CountryResponse responseFromAdd = _countriesService.AddCountry(request);

			CountryResponse? responseFromGet = _countriesService.GetCountryById(responseFromAdd.Id);

			Assert.Equal(responseFromAdd, responseFromGet);
		}
		#endregion
	}
}

