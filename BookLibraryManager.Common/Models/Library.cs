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
/// Represents a class for a <see cref="Library"/>.
/// </summary>
/// <author>YR 2025-01-09</author>
public class Library : BindableBase, ILibrary, IXmlSerializable
{
    /// <summary>
    /// Gets or sets the unique identifier for the <see cref="Library"/>.
    /// </summary>
    public int Id
    {
        get => _id;
        set
        {
            if (SetProperty(ref _id, value))
                LibraryIdChanged?.Invoke(this, new EventArgs());
        }
    }

    /// <summary>
    /// <see cref="Library"/> name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <summary>
    /// Short description of the <see cref="Library"/>.
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    /// <summary>
    /// Gets or sets the collection of books in the <see cref="Library"/>.
    /// </summary>
    [XmlArray]
    public ObservableCollection<Book> BookList
    {
        get => _bookList;
        set => SetProperty(ref _bookList, value);
    }

    /// <summary>
    /// Total numbers of books in the <see cref="BookList"/> 
    /// </summary>
    [XmlIgnore]
    public int TotalBooks => BookList.Count;

    /// <summary>
    /// Occurs when the library  <see cref="Id"/>  changes.
    /// </summary>
    public event EventHandler<EventArgs>? LibraryIdChanged;


    public XmlSchema? GetSchema() => throw new NotImplementedException();

    /// <summary>
    /// Writes the <see cref="Library"/> to the specified XML writer.
    /// </summary>
    /// <param name="writer">The XML writer to write the <see cref="Library"/> to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteElementString(nameof(Id), Id.ToString());
        writer.WriteElementString(nameof(Name), Name);
        writer.WriteElementString(nameof(Description), Description);

        writer.WriteStartElement(nameof(BookList));
        foreach (var book in BookList)
            book.WriteXml(writer);

        writer.WriteEndElement();
    }

    /// <summary>
    /// Reads the <see cref="Library"/> from the specified XML reader.
    /// </summary>
    /// <param name="reader">The XML reader to read the <see cref="Library"/> from.</param>
    public void ReadXml(XmlReader reader)
    {
        do
        {
            if (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case nameof(Id):
                        Id = reader.ReadElementContentAsInt();
                        break;
                    case nameof(Name):
                        Name = reader.ReadElementContentAsString();
                        break;
                    case nameof(Description):
                        Description = reader.ReadElementContentAsString();
                        break;
                    case nameof(BookList):
                        BookList = [];
                        while (reader.Name == "Book" || reader.Name == nameof(BookList))
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
    private string _name = string.Empty;
    private string _description = string.Empty;
    private ObservableCollection<Book> _bookList = [];
    #endregion
}
