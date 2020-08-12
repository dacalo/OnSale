using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Validations
{
    public class SizeFileValidation : ValidationAttribute
    {
        private readonly int _sizeMaximum;

        public SizeFileValidation(int sizeMaximum)
        {
            _sizeMaximum = sizeMaximum;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return ValidationResult.Success;

            if (formFile.Length > _sizeMaximum * 1024 * 1024)
                return new ValidationResult($"El peso del archvio no debe ser mayor a {_sizeMaximum}mb");

            return ValidationResult.Success;
        }
    }
}
