using System.Reflection;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a custom property information.
/// </summary>
/// <author>YR 2025-02-24</author>
public class PropertyCustomInfo
{
    /// <summary>
    /// Gets or sets the PropertyInfo associated with this custom property information.
    /// </summary>
    public PropertyInfo PropertyInfo
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the property should be sorted in descending order.
    /// </summary>
    public bool DescendingOrder
    {
        get; set;
    }
}
