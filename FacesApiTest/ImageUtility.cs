using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FacesApiTest
{
    public class ImageUtility
    {
        public byte[] ConvertToBytes(string imagePath)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (var stream = new FileStream(imagePath, FileMode.Open))
            {
                stream.CopyTo(memoryStream);
            }
            return memoryStream.ToArray();
        }

        public void SaveToFile(byte[] imageBytes, string imagePath)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                Image img = Image.FromStream(ms);
                img.Save(imagePath, ImageFormat.Jpeg);
            }
        }
    }
}
