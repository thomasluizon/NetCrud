using Entities;
using Newtonsoft.Json;

namespace ServiceContracts.DTO
{
	/// <summary>
	/// DTO class for adding a new country
	/// </summary>
	public class CountryAddRequest
	{
		public string? Name { get; set; }

		public Country ToCountry()
		{
			Country obj = JsonConvert.DeserializeObject<Country>(JsonConvert.SerializeObject(this));
			obj.Id = Guid.NewGuid();
			return obj;
		}
	}
}
