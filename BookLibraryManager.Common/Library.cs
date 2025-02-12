using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BookLibraryManager.Common;

[XmlInclude(typeof(Book))]
[XmlInclude(typeof(MediaData))]
[Serializable]
[XmlRoot("Library")]
/// <summary>
/// Represents a class for a library.
/// </summary>
/// <author>YR 2025-01-09</author>
public class Library : BindableBase, ILibrary, IXmlSerializable
{
    /// <summary>
    /// Gets or sets the unique identifier for the library.
    /// </summary>
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    /// <summary>
    /// Library name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <summary>
    /// Short description of the book.
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    /// <summary>
    /// Gets or sets the collection of books in the library.
    /// </summary>
    [XmlArray]
    public ObservableCollection<Book> BookList
    {
        get => _bookList;
        set => SetProperty(ref _bookList, value);
    }

    /// <summary>
    /// Total numbers of book in the library
    /// </summary>
    [XmlIgnore]
    public int TotalBooks => BookList.Count;

    public XmlSchema? GetSchema() => throw new NotImplementedException();

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString("Id", Id.ToString());
        writer.WriteElementString("Name", Name);
        writer.WriteElementString("Description", Description);

        writer.WriteStartElement("BookList");
        foreach (var book in BookList)
            book.WriteXml(writer);

        writer.WriteEndElement();
    }

    /// <summary>
    /// Reads the media data from the specified XML reader.
    /// </summary>
    /// <param name="reader">The XML reader to read the media data from.</param>
    public void ReadXml(XmlReader reader)
    {
        do
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "Id":
                        Id = reader.ReadElementContentAsInt();
                        break;
                    case "Name":
                        Name = reader.ReadElementContentAsString();
                        break;
                    case "Description":
                        Description = reader.ReadElementContentAsString();
                        break;
                    case "BookList":
                        BookList = [];
                        while (reader.Name == "Book" || reader.Name == "BookList")
                        {
                            var book = new Book() { Author = "", Title = "", TotalPages = 0, Id = 0 };
                            book.ReadXml(reader);
                            if (book.Id != 0)
                                BookList.Add(book);
                        }

                        break;
                }
            }
        }
        while (!reader.EOF && reader.Read());
    }

    #region private fields
    private int _id;
    private string _name;
    private string _description;
    private ObservableCollection<Book> _bookList = [];
    #endregion
}
