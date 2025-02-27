using System.Diagnostics;
using System.Security;

namespace LibraryManager.Utils;

/// <summary>
/// A utility class for handling unsafe operations.
/// </summary>
/// <author>YR 2025-02-25</author>
public sealed class UnsafeOperationsHandler
{
    /// <summary>
    /// Handles an unsafe operation by invoking the provided action and catching any exceptions that occur.
    /// </summary>
    /// <param name="action">The action to be executed.</param>
    [SecurityCritical]
    public void HandleUnsafeOperation(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (AccessViolationException excpt)
        {
            Debug.WriteLine($"An unsafe code exception occured: {excpt.Message}");
        }
        catch (Exception excpt)
        {
            Debug.WriteLine($"A safe code exception occured: {excpt.Message}");
        }
    }
}