# **The book library demo application with plugins**

The application provides following functionality:
1. Load a list of books from an XML-file (implemented in the separated .dll).
2. Add a new book to the list.
3. Remove a book from the list.
4. Sort the list in alphabetical order by author first. Then for each author sort it in alphabetical order by title. Example: first all Andersen’s books, then all King’s books. Andersen’s books: first The Little Mermaid, then The Ugly Duckling etc.
5. Book search by a part of its author, title, publish year, total pages.
6. Save the list of books into an XML-file. (implemented in the separated .dll)
The library is implemented in .NET/C# ver 9.0


This project contains Demo application written on WPF

```
WpfAppBookManager
```



BookLibraryManager.Common
This library contains common objects and main functinality of the library. it is mandatory dll
```
BookLibraryManager.Common.dll
```


BookLibraryManager.XmlFileLibraryOperator
This library provides operation the library stored as XML file :
```
BookLibraryManager.XmlLibraryProvider.dll
```

This project contains unit tests for xUnit framework
```
BookLibraryManagerTests
```
