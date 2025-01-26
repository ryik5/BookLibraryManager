using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace BookLibraryManager.Common;

/// <author>YR 2025-01-26</author>
public class BitmapimageConvertor
{
    public Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
    {
        using var outStream = new MemoryStream();

        BitmapEncoder enc = new BmpBitmapEncoder();
        enc.Frames.Add(BitmapFrame.Create(bitmapImage));
        enc.Save(outStream);
        var bitmap = new Bitmap(outStream);

        return new Bitmap(bitmap);
    }

    public BitmapImage BitmapConverter(Bitmap image)
    {
        using (var ms = new MemoryStream())
        {
            image.Save(ms, ImageFormat.Png);
            BitmapImage bImg = new BitmapImage();
            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(ms.ToArray());
            bImg.EndInit();
            ms.Close();

            return bImg;
        }
    }
}
