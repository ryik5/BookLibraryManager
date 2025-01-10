using BookLibraryManager;
using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManagerTests;

public class XmlBookListSaverTests
{
    [Fact]
    public void SaveLibrary_ShouldSaveLibraryToFile()
    {

        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        mockLibrary.SetupGet(lib => lib.Id).Returns(1);
        mockLibrary.SetupGet(lib => lib.BookList).Returns(new List<Book>
        {
            new() { Id = 1, Author = "Author1", Title = "Title1", PageNumber = 1 },
            new() { Id = 2, Author = "Author2", Title = "Title2", PageNumber = 2 }
        });

        var saver = new XmlBookListSaver();
        var pathToFile = "testLibrary.xml";

        // Act
        var result = saver.SaveLibrary(mockLibrary.Object, pathToFile);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.True(File.Exists(pathToFile));

        // Clean up
        File.Delete(pathToFile);
    }
}
