using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class IBookListLoadableTests
{
    [Fact]
    public void LoadLibrary_ValidPath_ReturnsLibrary()
    {
        // Arrange
        var mockBookListLoadable = new Mock<ILibraryLoader>();
        var path = "validPath";
        mockBookListLoadable.Setup(x => x.LoadLibrary(path, out It.Ref<ILibrary>.IsAny)).Returns(true);

        // Act
        var result = mockBookListLoadable.Object.LoadLibrary(path, out It.Ref<ILibrary>.IsAny);

        // Assert
        Xunit.Assert.True(result);
    }

    [Fact]
    public void LoadLibrary_InvalidPath_ReturnsFalse()
    {
        // Arrange
        var path = "invalidPath";
        var mockBookListLoadable = new Mock<ILibraryLoader>();
        mockBookListLoadable.Setup(x => x.LoadLibrary(It.IsAny<string>(), out It.Ref<ILibrary>.IsAny)).Returns(false);

        // Act
        var result = mockBookListLoadable.Object.LoadLibrary(path, out var library);

        // Assert
        Xunit.Assert.False(result);
    }
}
