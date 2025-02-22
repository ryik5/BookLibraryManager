using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BookLibraryManager.Common;

[Serializable]
/// <summary>
/// Represents media data that can be serialized to XML.
/// </summary>
/// <author>YR 2025-01-26</author>
public class MediaData : BindableBase, IXmlSerializable
{
    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string Name
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the original path of the file.
    /// </summary>
    public string OriginalPath
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the Extension of the file.
    /// </summary>
    public string Ext
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the IsLoaded state of the file.
    /// </summary>
    public bool IsLoaded
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the byte array of the file.
    /// </summary>
    [XmlAttribute("Source")]
    public byte[] ObjectByteArray
    {
        get; set;
    }

    /// <summary>
    /// Returns the XML schema for the file.
    /// </summary>
    public XmlSchema? GetSchema() => throw new NotImplementedException();


    /// <summary>
    /// Reads the media data from the specified XML reader.
    /// </summary>
    public void ReadXml(XmlReader reader)
    {
        var isRead = true;
        while (reader.Read() && isRead)
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    switch (reader.Name)
                    {
                        case "Name":
                            Name = reader.ReadElementContentAsString();
                            break;
                        case "OriginalPath":
                            OriginalPath = reader.ReadElementContentAsString();
                            break;
                        case "Ext":
                            Ext = reader.ReadElementContentAsString();
                            break;
                        case "IsLoaded":
                            var isLoaded = bool.TryParse(reader.ReadElementContentAsString(), out var result);
                            IsLoaded = isLoaded && result;

                            break;
                        case "Source":
                            const int LEN = 4096;
                            var buffer = new byte[LEN];
                            int read;
                            using (var ms = new MemoryStream())
                            {
                                var depth = reader.Depth;

                                do
                                {
                                    if (reader.NodeType == XmlNodeType.Text)
                                    {
                                        while ((read = reader.ReadContentAsBase64(buffer, 0, LEN)) > 0)
                                            ms.Write(buffer, 0, read);
                                    }
                                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Source")
                                    {
                                        break;
                                    }

                                } while (reader.Read() && reader.NodeType != XmlNodeType.EndElement);

                                ms.Position = 0;
                                ObjectByteArray = ms.ToArray();
                            }

                            isRead = false;
                            reader.ReadEndElement(); // Added to ensure proper reading of the 'Source' element
                            break;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Writes the media data to the specified XML writer.
    /// </summary>
    /// <param name="writer">The XML writer to write the media data to.</param>
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("MediaData");

        writer.WriteElementString("Name", Name);
        writer.WriteElementString("OriginalPath", OriginalPath);
        writer.WriteElementString("Ext", Ext);
        writer.WriteElementString("IsLoaded", $"{IsLoaded}");

        writer.WriteStartElement("Source");

        if (ObjectByteArray != null&& IsLoaded)
            writer.WriteBase64(ObjectByteArray, 0, ObjectByteArray.Length);
        writer.WriteEndElement();

        writer.WriteEndElement();
    }
}
