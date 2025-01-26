using System.IO;
using System.Xml.Serialization;

namespace BookLibraryManager.XmlLibraryProvider;

/// <author>YR 2025-01-26</author>
public class XMLObjectSerializer
{
    public static void Save<T>(T obj, string flieName)
    {
        var serializer = new XmlSerializer(typeof(T));
        //Create a FileStream object connected to the target file
        var fileStream = new FileStream(flieName, FileMode.Create);
        serializer.Serialize(fileStream, obj);
        fileStream.Close();
    }

    public static T Load<T>(string fileName)
    {
        var deserializer = new XmlSerializer(typeof(T));
        T obj = (T)deserializer.Deserialize(new FileStream(fileName, FileMode.Open));
        return obj;
    }
}
