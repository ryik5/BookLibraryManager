using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BookLibraryManager.Common;

[XmlRoot(ElementName = "MediaData")]
[Serializable]
/// <summary>
/// Represents media data that can be serialized to XML.
/// </summary>
/// <author>YR 2025-01-26</author>
public class MediaData : BindableBase, IXmlSerializable
{
    /// <summary>
    /// Gets or sets the name of the media data.
    /// </summary>
    public string Name
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the original path of the media data.
    /// </summary>
    public string OriginalPath
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the content type of the loaded data.
    /// </summary>
    [XmlElement("ContentType")]
    public EContentType ContentType
    {
        get => _contentType;
        set => SetProperty(ref _contentType, value);
    }
    private EContentType _contentType = EContentType.None;

    /// <summary>
    /// Gets or sets the image of the media data.
    /// </summary>
    [XmlIgnore]
    public Bitmap Image
    {
        get => _image;
        set => SetProperty(ref _image, value);
    }
    private Bitmap _image;

    /// <summary>
    /// Gets or sets the bitmap image of the media data.
    /// </summary>
    [XmlIgnore]
    public BitmapImage BmImage
    {
        get => _bmImage;
        set => SetProperty(ref _bmImage, value);
    }
    private BitmapImage _bmImage;

    /// <summary>
    /// Gets or sets the picture byte array of the media data.
    /// </summary>
    [XmlAttribute("Source")]
    public byte[] PictureByteArray
    {
        get
        {
            //TODO : rewrite for BitmapImage

            //get a TypeConverter object for converting Bitmap to bytes
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
            return (byte[])converter.ConvertTo(_image, typeof(byte[]));
        }
        set
        {
            //TODO : rewrite for BitmapImage

            _image = new Bitmap(new MemoryStream(value));
        }
    }

    /// <summary>
    /// Returns the XML schema for the media data.
    /// </summary>
    /// <returns>The XML schema for the media data.</returns>
    public XmlSchema GetSchema()
    {
        var schema = new XmlSchema();

        var mediaDataElement = new XmlSchemaElement
        {
            Name = "MediaData"
        };
        schema.Items.Add(mediaDataElement);

        var mediaDataComplexType = new XmlSchemaComplexType();
        mediaDataElement.SchemaType = mediaDataComplexType;

        var nameAttribute = new XmlSchemaAttribute
        {
            Name = "Name",
            SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
        };
        mediaDataComplexType.Attributes.Add(nameAttribute);

        var originalPathAttribute = new XmlSchemaAttribute
        {
            Name = "OriginalPath",
            SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
        };
        mediaDataComplexType.Attributes.Add(originalPathAttribute);

        var contentTypeAttribute = new XmlSchemaAttribute
        {
            Name = "ContentType",
            SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema")
        };
        mediaDataComplexType.Attributes.Add(contentTypeAttribute);

        var sourceAttribute = new XmlSchemaAttribute
        {
            Name = "Source",
            SchemaTypeName = new XmlQualifiedName("base64Binary", "http://www.w3.org/2001/XMLSchema")
        };
        mediaDataComplexType.Attributes.Add(sourceAttribute);

        return schema;
    }

    /// <summary>
    /// Reads the media data from the specified XML reader.
    /// </summary>
    /// <param name="reader">The XML reader to read the media data from.</param>
    public void ReadXml(XmlReader reader)
    {
        reader.ReadToFollowing("Name");
        Name = reader.ReadElementContentAsString();

        reader.GetAttribute("OriginalPath");
        OriginalPath = reader.ReadElementContentAsString();

        reader.GetAttribute("ContentType");
        var type = reader.ReadElementContentAsString();
        Enum.TryParse(type, out EContentType ContentType);

        reader.GetAttribute("Source");
        const int LEN = 4096;
        var buffer = new byte[LEN];
        int read;
        using var ms = new MemoryStream();
        var depth = reader.Depth;

        while (reader.Read() && reader.Depth > depth)
        {
            if (reader.NodeType == XmlNodeType.Text)
            {
                while ((read = reader.ReadContentAsBase64(buffer, 0, LEN)) > 0)
                    ms.Write(buffer, 0, read);

                if (reader.Depth <= depth)
                    break;
            }
        }

        ms.Position = 0;
        Image = new Bitmap(ms);
    }

    /// <summary>
    /// Writes the media data to the specified XML writer.
    /// </summary>
    /// <param name="writer">The XML writer to write the media data to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("Name");
        writer.WriteString(Name);
        writer.WriteEndElement();

        writer.WriteStartElement("OriginalPath");
        writer.WriteString(OriginalPath);
        writer.WriteEndElement();

        writer.WriteStartElement("ContentType");
        writer.WriteString($"{ContentType}");
        writer.WriteEndElement();

        writer.WriteStartElement("Source");
        writer.WriteBase64(PictureByteArray, 0, PictureByteArray.Length);
        writer.WriteEndElement();
    }
}
