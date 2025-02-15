using System.Windows.Input;

namespace LibraryManager.Models;

/// <summary>
/// Represents a command that can be bound to UI elements and executed based on the provided delegates.
/// </summary>
/// <author>YR 2025-01-24</author>
public sealed class RelayCommand : DelegateCommand
{
    public RelayCommand(Action executeMethode) : base(executeMethode)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class with the specified execute and can execute methods.
    /// </summary>
    /// <param name="executeMethode">The method to execute when the command is invoked.</param>
    /// <param name="canExecuteMethode">The method that determines whether the command can execute.</param>
    public RelayCommand(Action executeMethode, Func<bool> canExecuteMethode) : base(executeMethode, canExecuteMethode)
    {
    }

    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public override event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
