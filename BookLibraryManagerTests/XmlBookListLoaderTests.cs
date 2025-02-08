using System.Xml.Serialization;
using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class XmlBookListLoaderTests
{
    /// <summary>
    /// Tests that a valid file path returns a correctly loaded library.
    /// </summary>
    [Fact]
    public void LoadLibrary_ValidFilePath_ReturnsLibrary()
    {
        // Arrange
        var filePath = "testLibraryLoad.xml";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        var library = new LibraryManagerModel
        {
            Id = 1,
            BookList =
            [
                new() { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 1 },
                new() { Id = 2, Author = "Author2", Title = "Title2", TotalPages = 2 }
            ]
        };

        var serializer = new XmlSerializer(typeof(Library));

        using (var writer = new StreamWriter(filePath))
        {
            serializer.Serialize(writer, library);
        }

        var loader = new XmlLibraryLoader();

        // Act
        var result = loader.TryLoadLibrary(filePath, out var checkedLibrary);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.NotNull(checkedLibrary);
        Xunit.Assert.Equal(library.Id, checkedLibrary.Id);
        Xunit.Assert.Equal(library.BookList.Count, checkedLibrary.BookList.Count);
        Xunit.Assert.Equal(library.BookList[0].Id, checkedLibrary.BookList[0].Id);
        Xunit.Assert.Equal(library.BookList[0].Author, checkedLibrary.BookList[0].Author);
        Xunit.Assert.Equal(library.BookList[0].Title, checkedLibrary.BookList[0].Title);
        Xunit.Assert.Equal(library.BookList[0].TotalPages, checkedLibrary.BookList[0].TotalPages);

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    /// <summary>
    /// Tests that a valid XML file returns true.
    /// </summary>
    [Fact]
    public void LoadLibrary_ValidXmlFile_ReturnsTrue()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var loader = new XmlLibraryLoader();
        var pathToLibrary = "validLibrary.xml";
        var xmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<LibraryAbstract xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xsi:nil=\"true\" />";

        if (File.Exists(pathToLibrary))
        {
            File.Delete(pathToLibrary);
        }

        File.WriteAllText(pathToLibrary, xmlContent);

        // Act
        var result = loader.TryLoadLibrary(pathToLibrary, out It.Ref<ILibrary>.IsAny);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.IsAssignableFrom<ILibrary>(mockLibrary.Object);

        if (File.Exists(pathToLibrary))
        {
            File.Delete(pathToLibrary);
        }
    }

    /// <summary>
    /// Tests that an invalid XML file returns false.
    /// </summary>
    [Fact]
    public void LoadLibrary_InvalidXmlFile_ReturnsFalse()
    {
        // Arrange
        var loader = new XmlLibraryLoader();
        var pathToLibrary = "invalidLibrary.xml";
        var xmlContent = "<InvalidXml></InvalidXml>";

        if (File.Exists(pathToLibrary))
        {
            File.Delete(pathToLibrary);
        }
        File.WriteAllText(pathToLibrary, xmlContent);

        // Act
        var result = loader.TryLoadLibrary(pathToLibrary, out var library);

        // Assert
        Xunit.Assert.False(result);
        Xunit.Assert.Null(library);
        File.Delete(pathToLibrary);
    }

    /// <summary>
    /// Tests that a non-existent file returns false.
    /// </summary>
    [Fact]
    public void LoadLibrary_FileNotFound_ReturnsFalse()
    {
        // Arrange
        var loader = new XmlLibraryLoader();
        var pathToLibrary = "nonExistentLibrary.xml";

        // Act
        var result = loader.TryLoadLibrary(pathToLibrary, out var library);

        // Assert
        Xunit.Assert.False(result);
        Xunit.Assert.Null(library);
    }
}
