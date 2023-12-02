using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ScmssApiServer.Validators
{
    public class IdCardNumberAttribute : ValidationAttribute
    {
        public const string GeneralErrorMessage = "Invalid identity card number.";

        private const string IdCardNumberPattern = @"^\d{12}$";

        protected override ValidationResult? IsValid(object? value,
                                                     ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var idCardNumber = (string)value;
            if (idCardNumber == null)
            {
                return new ValidationResult(GeneralErrorMessage);
            }

            var match = Regex.Match(idCardNumber, IdCardNumberPattern);
            if (!match.Success)
            {
                return new ValidationResult("Identity card number must have 12 digits");
            }

            return ValidationResult.Success;
        }
    }
}
