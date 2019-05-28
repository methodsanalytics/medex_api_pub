using System.ComponentModel.DataAnnotations;

namespace MedicalExaminer.API.Attributes
{
    /// <summary>
    ///     Custom Validation attribute.
    /// </summary>
    public class RequiredIfAttributesMatch : RequiredAttribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RequiredIfAttributesMatch" /> class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="desiredValue">The desired valuer of the property.</param>
        public RequiredIfAttributesMatch(string propertyName, object desiredValue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredValue;
        }

        private string PropertyName { get; }

        private object DesiredValue { get; }

        /// <summary>
        ///     Validates the attribute.
        /// </summary>
        /// <param name="value">The object the validation is performed on.</param>
        /// <param name="context">the context of the validation.</param>
        /// <returns>ValidationResult.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);
            if (propertyValue == null || propertyValue.ToString() != DesiredValue.ToString())
            {
                return ValidationResult.Success;
            }

            return base.IsValid(value, context);
        }
    }

    /// <summary>
    ///     Custom Validation attribute.
    /// </summary>
    public class RequiredIfAttributesMatchAndOtherAttributeMatchesNotNull : RequiredIfAttributesMatch
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RequiredIfAttributesMatch" /> class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="desiredValue">The desired valuer of the property.</param>
        public RequiredIfAttributesMatchAndOtherAttributeMatchesNotNull(string propertyName, object desiredValue, string theOtherName)
            : base(propertyName, desiredValue)
        {
            PropertyName = propertyName;
            TheOtherName = theOtherName;
        }

        private string PropertyName { get; set; }

        private string TheOtherName { get; }

        /// <summary>
        ///     Validates the attribute.
        /// </summary>
        /// <param name="value">The object the validation is performed on.</param>
        /// <param name="context">the context of the validation.</param>
        /// <returns>ValidationResult.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var result = base.IsValid(value, context);
            if (result == null)
            {
                var instance = context.ObjectInstance;
                var type = instance.GetType();
                var propertyValue = type.GetProperty(TheOtherName).GetValue(instance, null);

                if ((propertyValue == null && value == null) || (propertyValue != null && value != null))
                {
                    return new ValidationResult("temp error msg");
                }
            }

            return result;
        }
    }
}