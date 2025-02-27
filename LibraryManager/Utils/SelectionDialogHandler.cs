using System.IO;
using BookLibraryManager.Common;
using Microsoft.Win32;

namespace LibraryManager.Utils;

/// <summary>
/// Handles the selection of media data through a dialog.
/// </summary>
/// <author>YR 2025-01-27</author>
internal sealed class SelectionDialogHandler
{
    public Task<MediaData?> OpenAndReadContentAsync(long maxContentLength)
    {
        MediaData? media = null;
        var openFileDialog = new OpenFileDialog
        {
            Title = "Select a file",
            Filter = "All (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var fileInfo = new FileInfo(openFileDialog.FileName);
            media = new MediaData
            {
                Name = fileInfo.Name,
                OriginalPath = openFileDialog.FileName,
                Ext = fileInfo.Extension
            };
            media.IsContentStoredSeparately = true;
            media.IsLoaded = false;

            if (fileInfo.Length < maxContentLength)
            {
                try
                {
                    media.ObjectByteArray = File.ReadAllBytes(openFileDialog.FileName);
                    media.IsLoaded = true;
                    media.IsContentStoredSeparately = false;
                }
                catch
                {
                    media.ObjectByteArray = null;
                }
            }
        }

        return Task.FromResult(media);
    }

    /// <summary>
    /// Saves the content of a book using a save file dialog.
    /// </summary>
    /// <param name="book">The book whose content is to be saved.</param>
    /// <returns>True if the content was saved successfully, false otherwise.</returns>
    public async Task<bool> SaveContentDialogTask(Book book)
    {
        // Create a new save file dialog with the default filter set to all files
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "All Files (*.*)|*.*",
            // Set the default file name to the book's title or the original path if available
            FileName = book.Content?.OriginalPath != null ? Path.GetFileName(book.Content.OriginalPath) : book.Title
        };

        // Check if the book's content is loaded and the save file dialog was shown successfully
        if (book.Content?.ObjectByteArray != null && book.Content.IsLoaded && (saveFileDialog.ShowDialog() ?? false))
        {
            try
            {
                // Save the book's content to the selected file
                File.WriteAllBytes(saveFileDialog.FileName, book.Content.ObjectByteArray);
                await Task.Yield();
                // Send a debug message to the status bar indicating the content was saved successfully
                MessageHandler.PublishDebugMessage($"The book content '{book.Title}' was saved to {saveFileDialog.FileName}");
                // Show a message box indicating the content was saved successfully
                new MessageBoxHandler().Show("Book content saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the save operation
                MessageHandler.PublishDebugMessage($"Error saving book content: {ex.Message}");
                return false;
            }
        }
        else
        {
            // Send a debug message to the status bar indicating the content was not saved
            MessageHandler.PublishDebugMessage($"The book content '{book.Title}' was not saved because it is stored separately");
            await Task.Yield();
            return false;
        }
    }

    /// <summary>
    /// Returns the path to the XML file with the library.
    /// </summary>
    public string? GetPathToXmlFile(string windowTitle)
    {
        var openDialog = new OpenFileDialog()
        {
            Title = windowTitle,
            DefaultExt = ".xml",
            Filter = "XML file (.xml)|*.xml"
        };
        var dialogResult = openDialog.ShowDialog();
        if (!dialogResult.HasValue || !dialogResult.Value)
            return null;

        var path = openDialog.FileName;

        return path;
    }

    /// <summary>
    /// Returns the path to the folder to store the library.
    /// </summary>
    public string? GetPathToFolder(string windowTitle)
    {
        var openDialog = new OpenFolderDialog()
        {
            Title = windowTitle,
            Multiselect = false,
            ValidateNames = true
        };
        var dialogResult = openDialog.ShowDialog();
        if (!dialogResult.HasValue || !dialogResult.Value)
            return null;

        return openDialog.FolderName;
    }
}
