# **Book Library Demo Application with Plugins**

The application provides the following functionality:
1. Load a list of books from an XML file (implemented in a separate .dll).
2. Add a new book to the list.
3. Remove a book from the list.
4. Sort the list in alphabetical order by author first, then by title for each author. Example: first all Andersen’s books, then all King’s books. Andersen’s books: first The Little Mermaid, then The Ugly Duckling, etc.
5. Search for a book by part of its author, title, publish year, or total pages.
6. Save the list of books into an XML file (implemented in a separate .dll).

The library is implemented in .NET/C# version 9.0.

## Project Structure

### WpfAppBookManager
This project contains the demo application written in WPF.

### BookLibraryManager.Common
This library contains common objects and the main functionality of the library. It is a mandatory DLL.
### BookLibraryManager.XmlFileLibraryOperator
This library provides operations for managing the library stored as an XML file.
### BookLibraryManagerTests
This project contains unit tests for the xUnit framework.
