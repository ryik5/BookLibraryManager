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
/// <author>YR 2025-01-26</author>
public class MediaData : BindableBase, IXmlSerializable
{
    public string Name
    {
        get; set;
    }

    public string OriginalPath
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the content type of the loaded data.
    /// </summary>
    [XmlElement("ContentType")]
    public ContentTypeEnum ContentType
    {
        get => _contentType;
        set => SetProperty(ref _contentType, value);
    }
    private ContentTypeEnum _contentType = ContentTypeEnum.None;

    [XmlIgnore]
    public Bitmap Image
    {
        get => _image;
        set => SetProperty(ref _image, value);
    }
    private Bitmap _image;

    [XmlIgnore]
    public BitmapImage BmImage
    {
        get => _bmImage;
        set => SetProperty(ref _bmImage, value);
    }
    private BitmapImage _bmImage;

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

    public XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadToFollowing("Name");
        Name = reader.ReadElementContentAsString();

        reader.GetAttribute("OriginalPath");
        OriginalPath = reader.ReadElementContentAsString();

        reader.GetAttribute("ContentType");
        var type = reader.ReadElementContentAsString();
        Enum.TryParse(type, out ContentTypeEnum ContentType);

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
