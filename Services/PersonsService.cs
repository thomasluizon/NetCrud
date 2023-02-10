using Entities;
using ServiceContracts.DTO;
using ServiceContracts.Interfaces;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly ICountriesService _countriesService = new CountriesService();
    private readonly List<Person> _personList = new();

    public PersonResponse AddPerson(PersonAddRequest personAddRequest)
    {
        // Validation
        personAddRequest.ModelValidation();

        // Converting to person object
        var person = personAddRequest.ToPerson();

        // Adding to list
        _personList.Add(person);

        // Return the converted person response
        return ConvertPersonIntoPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _personList.Select(ConvertPersonIntoPersonResponse).ToList();
    }

    public PersonResponse? GetPersonById(Guid id)
    {
        var person = _personList.FirstOrDefault(person => person.Id == id);

        return person == null ? null : ConvertPersonIntoPersonResponse(person);
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        var allPersons = GetAllPersons();

        if (string.IsNullOrEmpty(searchString))
            return allPersons;

        var matchingPersons = allPersons.FindAll(person =>
            GetPropValue(person, searchBy)!.ToString()!.Contains(searchString, StringComparison.OrdinalIgnoreCase));

        return matchingPersons.Any() ? matchingPersons : new List<PersonResponse>();
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest)
    {
        // Validation
        personUpdateRequest.ModelValidation();

        // Get matching person object to update
        var personToUpdate = _personList.FirstOrDefault(person => person.Id == personUpdateRequest.Id);

        if (personToUpdate == null)
            throw new ArgumentException("Wrong person Id");

        personToUpdate.CopyProperties(personUpdateRequest);
        return personToUpdate.ToPersonResponse();
    }

    public bool DeletePerson(Guid personId)
    {
        if (GetPersonById(personId) == null)
            return false;

        _personList.RemoveAll(person => person.Id == personId);
        return true;
    }

    private PersonResponse ConvertPersonIntoPersonResponse(Person person)
    {
        var personResponse = person.ToPersonResponse();
        personResponse.Country = _countriesService.GetCountryById(person.CountryId)?.Name;
        return personResponse;
    }

    private static object? GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName)?.GetValue(src, null);
    }
}