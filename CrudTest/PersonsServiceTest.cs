using Entities;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using ServiceContracts.Interfaces;
using Services;

namespace CrudTest;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;

    public PersonsServiceTest()
    {
        _personsService = new PersonsService();
    }

    #region UpdatePerson

    // When supplying an invalid Id, it should throws Argument Exception
    [Fact]
    public void UpdatePerson_InvalidPersonId()
    {
        var personUpdateRequest = new PersonUpdateRequest
        {
            Id = Guid.NewGuid()
        };

        Assert.Throws<ArgumentException>(() => { _personsService.UpdatePerson(personUpdateRequest); });
    }

    // When supplying invalid person details, it should throws Argument Exception
    [Fact]
    public void UpdatePerson_InvalidPersonDetails()
    {
        var personAddRequest = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personUpdateRequest = _personsService.AddPerson(personAddRequest).ToPersonUpdateRequest();
        personUpdateRequest.Name = null;

        Assert.Throws<ArgumentException>(() => { _personsService.UpdatePerson(personUpdateRequest); });
    }

    // When supplying invalid person details, it should throws Argument Exception
    [Fact]
    public void UpdatePerson_CorrectPersonDetails()
    {
        var personAddRequest = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personUpdateRequest = _personsService.AddPerson(personAddRequest).ToPersonUpdateRequest();
        personUpdateRequest.Name = "John";
        var personResponse = _personsService.UpdatePerson(personUpdateRequest);
        var personExpected = _personsService.GetPersonById(personUpdateRequest.Id);

        Assert.Equal(personExpected, personResponse);
    }

    #endregion

    #region AddPerson

    // When we supply a person object with incorrect details, throws an argument-exception
    [Fact]
    public void AddPerson_IncorrectPersonDetails()
    {
        var personAddRequest = new PersonAddRequest();

        Assert.Throws<ArgumentException>(() => { _personsService.AddPerson(personAddRequest); });
    }

    // When we supply proper person object, it should insert the person into the persons list, and return the person response object with the generated Id
    [Fact]
    public void AddPerson_ProperPersonDetails()
    {
        var personAddRequest = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personResponse = _personsService.AddPerson(personAddRequest);
        var personsList = _personsService.GetAllPersons();

        Assert.True(personResponse.Id != Guid.Empty);
        Assert.Contains(personResponse, personsList);
    }

    #endregion

    #region GetPersonById

    // When a person is not found by the id, returns null
    [Fact]
    public void GetPersonById_PersonNotFound()
    {
        Assert.Null(_personsService.GetPersonById(Guid.Empty));
    }

    // When a person is found by the id, returns the person response object
    [Fact]
    public void GetPersonById_PersonFound()
    {
        var personAddRequest = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personResponseExpected = _personsService.AddPerson(personAddRequest);
        var personResponseReturned = _personsService.GetPersonById(personResponseExpected.Id);

        Assert.Equal(personResponseExpected, personResponseReturned);
    }

    #endregion

    #region GetAllPersons

    // When there's no items in the list, returns an empty list
    [Fact]
    public void GetAllPersons_NoItems()
    {
        Assert.Empty(_personsService.GetAllPersons());
    }

    // When there's items in the list, returns a list of person response with the correct items
    [Fact]
    public void GetAllPersons_ExistItems()
    {
        var personAddRequest1 = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personAddRequest2 = new PersonAddRequest
        {
            Name = "John",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = false
        };

        var expected1 = _personsService.AddPerson(personAddRequest1);
        var expected2 = _personsService.AddPerson(personAddRequest2);

        var list = _personsService.GetAllPersons();

        Assert.Contains(expected1, list);
        Assert.Contains(expected2, list);
    }

    #endregion

    #region GetFilteredPersons

    // When searching for "name" and string search provided is empty, returns the list of all persons 
    [Fact]
    public void GetFilteredPersons_EmptySearchText()
    {
        var personAddRequest1 = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personAddRequest2 = new PersonAddRequest
        {
            Name = "John",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = false
        };

        var expected1 = _personsService.AddPerson(personAddRequest1);
        var expected2 = _personsService.AddPerson(personAddRequest2);

        var list = _personsService.GetFilteredPersons(nameof(Person.Name), "");

        Assert.Contains(expected1, list);
        Assert.Contains(expected2, list);
    }

    // When searching for "name" and string search provided correct, returns a list of all persons that matches the search string 
    [Fact]
    public void GetFilteredPersons_CorrectSearchText()
    {
        var personAddRequest1 = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personAddRequest2 = new PersonAddRequest
        {
            Name = "John",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = false
        };

        var expected = _personsService.AddPerson(personAddRequest1);
        var notExpected = _personsService.AddPerson(personAddRequest2);

        var list = _personsService.GetFilteredPersons(nameof(Person.Name), "Thomas");

        Assert.Contains(expected, list);
        Assert.DoesNotContain(notExpected, list);
    }

    // When searching for "name" and string search provided is with the wrong case, returns a list of all persons that matches the search string case insensitive 
    [Fact]
    public void GetFilteredPersons_WrongCaseSearchText()
    {
        var personAddRequest1 = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personAddRequest2 = new PersonAddRequest
        {
            Name = "John",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = false
        };

        var personAddRequest3 = new PersonAddRequest
        {
            Name = "Thabata",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Female,
            ReceiveNewsLetters = true
        };

        var expected1 = _personsService.AddPerson(personAddRequest1);
        var expected2 = _personsService.AddPerson(personAddRequest3);
        var notExpected = _personsService.AddPerson(personAddRequest2);

        var list = _personsService.GetFilteredPersons(nameof(Person.Name), "th");

        Assert.Contains(expected1, list);
        Assert.Contains(expected2, list);
        Assert.DoesNotContain(notExpected, list);
    }

    #endregion

    #region DeletePerson

    // if invalid person Id is supplied, it should return false
    [Fact]
    public void DeletePerson_InvalidPersonId()
    {
        Assert.False(_personsService.DeletePerson(Guid.NewGuid()));
    }

    // if correct person Id is supplied, it should return true and delete the person
    [Fact]
    public void DeletePerson_CorrectPersonId()
    {
        var personAddRequest = new PersonAddRequest
        {
            Name = "Thomas",
            Address = "Sample",
            CountryId = Guid.NewGuid(),
            DateOfBirth = DateTime.Now,
            Email = "sample@sample.com",
            Gender = GenderOptions.Male,
            ReceiveNewsLetters = true
        };

        var personResponse = _personsService.AddPerson(personAddRequest);
        Assert.True(_personsService.DeletePerson(personResponse.Id));
        var allPerson = _personsService.GetAllPersons();
        Assert.DoesNotContain(personResponse, allPerson);
    }

    #endregion
}