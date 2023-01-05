using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ScottPlot
{
    public class Image
    {
        private SKImage skiaImage { get; set; }
        public int Width => skiaImage.Width;
        public int Height => skiaImage.Height;

        public Image(SKImage skiaImage)
        {
            this.skiaImage = skiaImage;
        }

        public byte[] GetImageBytes(ImageFormat format = ImageFormat.Png, int quality = 100)
        {
            SKEncodedImageFormat skFormat = format.ToSKFormat();
            SKData data = skiaImage.Encode(skFormat, quality);
            return data.ToArray();
        }

        public void SaveImageToStream(Stream stream, ImageFormat format = ImageFormat.Png, int quality = 100)
        {
            SKEncodedImageFormat skFormat = format.ToSKFormat();
            SKData data = skiaImage.Encode(skFormat, quality);
            data.SaveTo(stream);
        }

        public string SaveJpeg(string path, int quality = 85)
        {
            byte[] bytes = GetImageBytes(ImageFormat.Jpeg, quality);
            File.WriteAllBytes(path, bytes);
            return Path.GetFullPath(path);
        }

        public string SavePng(string path)
        {
            byte[] bytes = GetImageBytes(ImageFormat.Png, 100);
            File.WriteAllBytes(path, bytes);
            return Path.GetFullPath(path);
        }

        // TODO: This currently throws because SKSnapshot.Encode(SKEncodedImageFormat.Bmp) returns null
        //public string SaveBmp(string path, int width, int height)
        //{
        //    byte[] bytes = GetImageBytes(width, height, ImageFormat.Bmp, 100);
        //    File.WriteAllBytes(path, bytes);
        //    return Path.GetFullPath(path);
        //}

        public string SaveWebp(string path, int quality = 85)
        {
            byte[] bytes = GetImageBytes(ImageFormat.Webp, quality);
            File.WriteAllBytes(path, bytes);
            return Path.GetFullPath(path);
        }

        public string Save(string path, ImageFormat format = ImageFormat.Png, int quality = 85)
        {
            return format switch
            {
                ImageFormat.Png => SavePng(path),
                ImageFormat.Jpeg => SaveJpeg(path, quality),
                ImageFormat.Webp => SaveWebp(path, quality),
                _ => throw new ArgumentException($"Unsupported image format: {format}"),
            };
        }
    }
}
