using ServiceContracts.DTO;

namespace ServiceContracts.Interfaces;

/// <summary>
///     Represents business logic to manipulate Person entity
/// </summary>
public interface IPersonsService
{
    /// <summary>
    ///     Adds a person to the list of persons
    /// </summary>
    /// <param name="personAddRequest">Receives the PersonAddRequest object</param>
    /// <returns>Returns the PersonResponse object</returns>
    PersonResponse AddPerson(PersonAddRequest personAddRequest);

    /// <summary>
    ///     Gets the list of all persons
    /// </summary>
    /// <returns>Returns a list of PersonResponse objects</returns>
    List<PersonResponse> GetAllPersons();

    /// <summary>
    ///     Gets a person by it's Id
    /// </summary>
    /// <param name="id">Receives the person Id</param>
    /// <returns>Returns the PersonResponse object that matches the person Id</returns>
    PersonResponse? GetPersonById(Guid id);

    /// <summary>
    ///     Gets a list of persons based on a filter
    /// </summary>
    /// <param name="searchBy">Receives the property that will be filtered</param>
    /// <param name="searchString">Receives the string that will be searched</param>
    /// <returns>Returns a list of the matching PersonResponse objects</returns>
    List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

    /// <summary>
    ///     Updates a person
    /// </summary>
    /// <param name="personUpdateRequest">Receives the PersonUpdateRequest object</param>
    /// <returns>Returns the PersonResponse of the update object</returns>
    PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest);

    /// <summary>
    ///     Deletes a person based on the Id
    /// </summary>
    /// <param name="personId">Receives the person Id</param>
    /// <returns>returns true if the person is deleted and false if the person is not deleted</returns>
    bool DeletePerson(Guid personId);
}