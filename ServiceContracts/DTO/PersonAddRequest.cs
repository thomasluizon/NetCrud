using System.ComponentModel.DataAnnotations;
using Entities;
using Newtonsoft.Json;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO;

/// <summary>
///     Acts as a DTO for inserting a new person
/// </summary>
public class PersonAddRequest
{
    [Required] public string? Name { get; set; }

    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] public DateTime? DateOfBirth { get; set; }

    [Required] public GenderOptions? Gender { get; set; }

    [Required] public Guid CountryId { get; set; }

    [Required] public string? Address { get; set; }

    [Required] public bool ReceiveNewsLetters { get; set; }

    /// <summary>
    ///     Converts the current object of PersonAddRequest into a new object of Person type
    /// </summary>
    /// <returns>Returns a new object of the type Person</returns>
    public Person ToPerson()
    {
        var obj = JsonConvert.DeserializeObject<Person>(JsonConvert.SerializeObject(this));
        obj.Id = Guid.NewGuid();
        return obj;
    }
}