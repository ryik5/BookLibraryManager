using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class IBookListSaveableTests
{
    [Fact]
    public void SaveLibrary_ShouldReturnTrue_WhenLibraryIsSavedSuccessfully()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var mockBookListSaveable = new Mock<ILibraryKeeper>();
        var selectedFolder = "C:\\LibraryFolder";

        mockBookListSaveable.Setup(x => x.SaveLibrary(mockLibrary.Object, selectedFolder)).Returns(true);

        // Act
        var result = mockBookListSaveable.Object.SaveLibrary(mockLibrary.Object, selectedFolder);

        // Assert
        Xunit.Assert.True(result);
    }

    [Fact]
    public void SaveLibrary_ShouldReturnFalse_WhenLibrarySaveFails()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var mockBookListSaveable = new Mock<ILibraryKeeper>();
        var selectedFolder = "C:\\LibraryFolder";

        mockBookListSaveable.Setup(x => x.SaveLibrary(mockLibrary.Object, selectedFolder)).Returns(false);

        // Act
        var result = mockBookListSaveable.Object.SaveLibrary(mockLibrary.Object, selectedFolder);

        // Assert
        Xunit.Assert.False(result);
    }
}
