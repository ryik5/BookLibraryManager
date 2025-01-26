using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BookLibraryManager.Common;

[XmlRoot(ElementName = "MediaData")]
[Serializable]
public class MediaData : IXmlSerializable
{
    public string Name
    {
        get; set;
    }

    public string OriginalPath
    {
        get; set;
    }

    [XmlIgnore]
    public Bitmap Image
    {
        get => source;
        set => source = value;
    }
    private Bitmap source;


    [XmlAttribute("Source")]
    public byte[] PictureByteArray
    {
        get
        {
            //get a TypeConverter object for converting Bitmap to bytes
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Bitmap));
            return (byte[])converter.ConvertTo(source, typeof(byte[]));
        }
        set => source = new Bitmap(new MemoryStream(value));
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
                {
                    ms.Write(buffer, 0, read);
                }

                if (reader.Depth <= depth)
                    break;
            }
        }

        ms.Position = 0;
        if (ms != null)
            Image = new Bitmap(ms);
        //reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("Name");
        writer.WriteString(Name);
        writer.WriteEndElement();

        writer.WriteStartElement("OriginalPath");
        writer.WriteString(OriginalPath);
        writer.WriteEndElement();

        writer.WriteStartElement("Source");
        writer.WriteBase64(PictureByteArray, 0, PictureByteArray.Length);
        writer.WriteEndElement();
    }
}
