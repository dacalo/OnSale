using Microsoft.AspNetCore.Http;
using OnSale.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace OnSale.Web.Validations
{
    public class TypeFileValidation : ValidationAttribute
    {
        private readonly string[] _validTypes;

        public TypeFileValidation(string[] validTypes)
        {
            _validTypes = validTypes;
        }

        public TypeFileValidation(GroupTypeFile groupTypeFile)
        {
            switch (groupTypeFile)
            {
                case GroupTypeFile.Image:
                    _validTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
                    break;
                default:
                    break;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            IFormFile formFile = value as IFormFile;

            if (formFile == null)
                return ValidationResult.Success;

            if (!_validTypes.Contains(formFile.ContentType))
                return new ValidationResult($"El tipo de archivo debe ser uno de los siguientes: {string.Join(",", _validTypes)}");

            return ValidationResult.Success;
        }
    }
}
