using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SharedLib.Filters
{
    public class SortValidationAttribute : ValidationAttribute
    {
        private string _valdatedProperty;
        private readonly string[] _availableFilters;
        private readonly string[] _sortTypes;

        public SortValidationAttribute(string[] availableFilters, string[] sortTypes)
        {
            _availableFilters = availableFilters;
            _sortTypes = sortTypes;
        }

        public string GetErrorMessage() => $"Ошибка валидации фильтра. Поле: {_valdatedProperty}";

        protected override ValidationResult IsValid(object sort, ValidationContext validationContext)
        {
            if (sort == null) return ValidationResult.Success; ;

            _valdatedProperty = validationContext.MemberName;
            foreach (var sortItem in (string[])sort)
            {
                string[] items = sortItem.ToLower().Split('-');
                if (items.Length != 2)
                {
                    return new ValidationResult(GetErrorMessage());
                }
                if (!_availableFilters.Contains(items[0]) || !_sortTypes.Contains(items[1]))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }
    }
}