using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
	public static class ValidationHelper
	{
		internal static void ModelValidation(this object obj)
		{
			var validationContext = new ValidationContext(obj);
			var validationResults = new List<ValidationResult>();
			bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

			if (!isValid)
				throw new ArgumentException(string.Join("\n", validationResults.Select(e => e.ErrorMessage)));
		}
	}
}
