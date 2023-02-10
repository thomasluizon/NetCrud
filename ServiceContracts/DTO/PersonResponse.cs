using System.Reflection;
using Entities;
using Newtonsoft.Json;

namespace ServiceContracts.DTO;

public class PersonResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public Guid CountryId { get; set; }
    public string? Country { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(PersonResponse))
            return false;

        var other = (PersonResponse)obj;

        return Name == other.Name && Email == other.Email && DateOfBirth == other.DateOfBirth && Age == other.Age &&
               Gender == other.Gender && CountryId == other.CountryId && Address == other.Address &&
               ReceiveNewsLetters == other.ReceiveNewsLetters;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public PersonUpdateRequest ToPersonUpdateRequest()
    {
        return JsonConvert.DeserializeObject<PersonUpdateRequest>(JsonConvert.SerializeObject(this));
    }
}

public static class PersonExtensions
{
    private static int? GetAge(DateTime? birth)
    {
        if (birth == null)
            return null;

        return Convert.ToInt32(Math.Round((DateTime.Now - birth.Value).TotalDays / 365.25));
    }

    public static PersonResponse ToPersonResponse(this Person person)
    {
        var p = JsonConvert.DeserializeObject<PersonResponse>(JsonConvert.SerializeObject(person));
        p.Age = GetAge(p.DateOfBirth);
        return p;
    }

    /// <summary>
    ///     Extension for 'Object' that copies the properties to a destination object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public static void CopyProperties(this object source, object destination)
    {
        // If any this null throw an exception
        if (source == null || destination == null)
            throw new Exception("Source or/and Destination Objects are null");
        // Getting the Types of the objects
        var typeDest = destination.GetType();
        var typeSrc = source.GetType();

        // Iterate the Properties of the source instance and  
        // populate them from their destination counterparts  
        var srcProps = typeSrc.GetProperties();
        foreach (var srcProp in srcProps)
        {
            if (!srcProp.CanRead) continue;

            var targetProperty = typeDest.GetProperty(srcProp.Name);
            if (targetProperty == null) continue;

            if (!targetProperty.CanWrite) continue;

            if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true)!.IsPrivate) continue;

            if ((targetProperty.GetSetMethod()!.Attributes & MethodAttributes.Static) != 0) continue;

            if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)) continue;

            // Passed all tests, lets set the value
            targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
        }
    }
}