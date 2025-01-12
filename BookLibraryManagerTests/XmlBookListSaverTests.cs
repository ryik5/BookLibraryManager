using BookLibraryManager;
using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManagerTests;

public class XmlBookListSaverTests
{
    [Fact]
    public void SaveLibrary_ValidLibrary_Success()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        mockLibrary.SetupGet(lib => lib.Id).Returns(1);
        mockLibrary.SetupGet(lib => lib.BookList).Returns(
        [
            new() { Id = 1, Author = "Author1", Title = "Title1", PageNumber = 1 },
            new() { Id = 2, Author = "Author2", Title = "Title2", PageNumber = 2 }
        ]);
        var saver = new XmlBookListSaver();
        var pathToFile = "testLibrary.xml";

        // Act
        var result = saver.SaveLibrary(mockLibrary.Object, pathToFile);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.True(File.Exists(pathToFile));

        // Cleanup
        if (File.Exists(pathToFile))
        {
            File.Delete(pathToFile);
        }
    }

    [Fact]
    public void SaveLibrary_NullLibrary_ThrowsException()
    {
        // Arrange
        var xmlBookListSaver = new XmlBookListSaver();
        var pathToFile = "testLibrary.xml";

        // Act & Assert
        bool result;
        try
        {
            Xunit.Assert.Throws<InvalidCastException>(() => xmlBookListSaver.SaveLibrary(null, pathToFile));
            result = true;
        }
        catch
        {
            result = false;
        }
        Xunit.Assert.False(result);
    }
}