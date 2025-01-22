using System.Globalization;
using System.Windows.Controls;

namespace BookLibraryManager.DemoApp.Util;

class IntValidationRule : ValidationRule
{
    public int Min
    {
        get; set;
    }
    public int Max
    {
        get; set;
    }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        var minPublishYear = 400;

        try
        {
            if (((string)value).Length > 0)
                minPublishYear = int.Parse((string)value);
        }
        catch (Exception e)
        {
            return new ValidationResult(false, $"Illegal characters or {e.Message}");
        }

        if ((minPublishYear < Min) || (minPublishYear > Max))
        {
            return new ValidationResult(false,
              $"Please enter an valid value: from - {Min}, till - {Max}.");
        }
        return ValidationResult.ValidResult;
    }
}
