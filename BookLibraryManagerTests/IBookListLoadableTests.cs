using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

public class IBookListLoadableTests
{
    [Fact]
    public void LoadLibrary_ValidPath_ReturnsLibrary()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var mockBookListLoadable = new Mock<IBookListLoadable>();
        var path = "validPath";
        mockBookListLoadable.Setup(x => x.LoadLibrary(path)).Returns(mockLibrary.Object);

        // Act
        var result = mockBookListLoadable.Object.LoadLibrary(path);

        // Assert
        Xunit.Assert.NotNull(result);
        Xunit.Assert.IsAssignableFrom<ILibrary>(result);
    }
}
