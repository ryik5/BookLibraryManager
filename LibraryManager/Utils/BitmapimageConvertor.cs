using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace LibraryManager.Utils;

/// <author>YR 2025-01-26</author>
internal sealed class BitmapimageConvertor
{
    public Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
    {
        using var outStream = new MemoryStream();

        BitmapEncoder enc = new BmpBitmapEncoder();
        enc.Frames.Add(BitmapFrame.Create(bitmapImage));
        enc.Save(outStream);
        var bitmap = new Bitmap(outStream);

        return new Bitmap(bitmap);
    }

    /// <author>YR 2025-01-26</author>
    public BitmapImage BitmapToBitmapImage(Bitmap image)
    {
        using (var ms = new MemoryStream())
        {
            image.Save(ms, ImageFormat.Png);
            var bImg = new BitmapImage();
            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(ms.ToArray());
            bImg.EndInit();
            ms.Close();

            return bImg;
        }
    }

    public BitmapImage BitmapImageFromBytes(byte[] bytes)
    {
        var image = new BitmapImage();

        try
        {
            using var ms = new MemoryStream();

            image.BeginInit();
            image.StreamSource = new MemoryStream(ms.ToArray());
            image.EndInit();
            ms.Close();

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        return image;
    }
}
