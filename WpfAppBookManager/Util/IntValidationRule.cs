using System.Globalization;
using System.Windows.Controls;

namespace BookLibraryManager.DemoApp.Util;

/// <summary>
/// A validation rule that checks if an integer value falls within a specified range.
/// </summary>
/// <author>YR 2025-01-23</author>
class IntValidationRule : ValidationRule
{
    /// <summary>
    /// Gets or sets the minimum allowed value.
    /// </summary>
    public int Min
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the maximum allowed value.
    /// </summary>
    public int Max
    {
        get; set;
    }

    /// <summary>
    /// Validates whether the input value is an integer within the specified range.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="cultureInfo">The culture information.</param>
    /// <returns>A ValidationResult indicating whether the value is valid.</returns>
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        var minValue = 400;

        try
        {
            if (((string)value).Length > 0)
                minValue = int.Parse((string)value);
        }
        catch (Exception e)
        {
            return new ValidationResult(false, $"Illegal characters or {e.Message}");
        }

        if ((minValue < Min) || (minValue > Max))
        {
            return new ValidationResult(false,
              $"Please enter a valid value: from {Min} to {Max}.");
        }
        return ValidationResult.ValidResult;
    }
}
