using Xunit;

namespace BookLibraryManager.Tests;

public class XmlBookListLoaderTests
{
    [Fact]
    public void LoadLibrary_ValidXmlFile_ReturnsLibrary()
    {
        // Arrange
        var xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
                            <LibraryAbstract xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xsi:type=""LibraryModel"">
                                <Id>1</Id>
                                <BookList>
                                    <Book>
                                        <Id>1</Id>
                                        <Author>Author1</Author>
                                        <Title>Title1</Title>
                                        <PageNumber>1</PageNumber>
                                    </Book>
                                    <Book>
                                        <Id>2</Id>
                                        <Author>Author2</Author>
                                        <Title>Title2</Title>
                                        <PageNumber>2</PageNumber>
                                    </Book>
                                </BookList>
                            </LibraryAbstract>";
        var filePath = "testLibrary.xml";
        File.WriteAllText(filePath, xmlContent);

        var loader = new XmlBookListLoader();

        // Act
        var library = loader.LoadLibrary(filePath);

        // Assert
        Xunit.Assert.NotNull(library);
        Xunit.Assert.Equal(1, library.Id);
        Xunit.Assert.Equal(2, library.BookList.Count);
        Xunit.Assert.Equal("Author1", library.BookList[0].Author);
        Xunit.Assert.Equal("Title1", library.BookList[0].Title);
        Xunit.Assert.Equal(1, library.BookList[0].PageNumber);
        Xunit.Assert.Equal("Author2", library.BookList[1].Author);
        Xunit.Assert.Equal("Title2", library.BookList[1].Title);
        Xunit.Assert.Equal(2, library.BookList[1].PageNumber);

        // Cleanup
        File.Delete(filePath);
    }
}
