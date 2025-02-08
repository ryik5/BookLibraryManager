﻿using BookLibraryManager.Common;

namespace BookLibraryManager.XmlLibraryProvider.Keepers;

/// <summary>
/// Saver of the Book as XML file on the local disk.
/// <author>YR 2025-01-09</author>
public class XmlBookKeeper : IBookKeeper
{
    /// <summary>
    /// Saves the book to an XML file at the specified path.
    /// </summary>
    /// <param name="book">The instance of the Book to save.</param>
    /// <param name="pathToFile">The path to the file where the book will be saved.</param>
    public bool TrySaveBook(Book book, string pathToFile)
    {
        try
        {
            XmlObjectSerializer.Save(book, pathToFile);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
