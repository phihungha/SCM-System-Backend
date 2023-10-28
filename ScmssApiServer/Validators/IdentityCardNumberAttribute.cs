using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ScmssApiServer.Validators
{
    public class IdentityCardNumberAttribute : ValidationAttribute
    {
        public const string GeneralErrorMessage = "Invalid identity card number.";

        private const string CardNumberPattern = @"^\d{12}$";

        protected override ValidationResult? IsValid(object? value,
                                                     ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(GeneralErrorMessage);
            }

            var cardNumber = (string)value;
            if (cardNumber == null)
            {
                return new ValidationResult(GeneralErrorMessage);
            }

            var match = Regex.Match(cardNumber, CardNumberPattern);
            if (!match.Success)
            {
                return new ValidationResult("Identity card number must have 12 digits");
            }

            return ValidationResult.Success;
        }
    }
}
