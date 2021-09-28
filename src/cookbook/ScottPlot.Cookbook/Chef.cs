using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScottPlot.Cookbook
{
    /// <summary>
    /// The Chef can execute recipes (making Bitmaps) and describe recipes (reporting source code)
    /// </summary>
    public static class Chef
    {
        /// <summary>
        /// Use reflection to determine all IRecipe objects in the project, execute each of them, 
        /// and save the output using the recipe ID as its base filename.
        /// </summary>
        public static void CreateCookbookImages(string outputPath, int width = 600, int height = 400, int thumbJpegQuality = 95)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            var recipes = Locate.GetRecipes();

            EncoderParameters thumbJpegEncoderParameters = new(1);
            thumbJpegEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, thumbJpegQuality);
            ImageCodecInfo thumbJpegEncoder = ImageCodecInfo.GetImageEncoders().Where(x => x.MimeType == "image/jpeg").First();

            Parallel.ForEach(recipes, recipe =>
            {
                var plt = new Plot(width, height);
                recipe.ExecuteRecipe(plt);

                // save full size image
                Bitmap bmp = plt.Render();
                string filePath = Path.Combine(outputPath, recipe.ID.ToLower() + ".png");
                bmp.Save(filePath, ImageFormat.Png);

                // thumbnail
                int thumbHeight = 180;
                int thumbWidth = thumbHeight * bmp.Width / bmp.Height;
                Bitmap thumb = Drawing.GDI.Resize(bmp, thumbWidth, thumbHeight);
                string thumbFilePath = Path.Combine(outputPath, recipe.ID.ToLower() + "_thumb.jpg");
                thumb.Save(thumbFilePath, thumbJpegEncoder, thumbJpegEncoderParameters);
            });
        }

        /// <summary>
        /// Read all .cs files in the source folder to identify IRecipe source code, isolate just the recipe 
        /// component of each source file, and save the output as a text file using the recipe ID as its base filename.
        /// </summary>
        public static void CreateCookbookSource(string sourcePath, string outputPath, int width = 600, int height = 400)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            var sources = SourceParsing.GetRecipeSources(sourcePath, width, height);

            Parallel.ForEach(sources, recipe =>
            {
                string filePath = Path.Combine(outputPath, recipe.ID.ToLower() + ".cs");
                StringBuilder sb = new();
                sb.AppendLine("// " + recipe.Title);
                sb.AppendLine("// " + recipe.Description);
                sb.Append(recipe.Code);
                File.WriteAllText(filePath, sb.ToString());
            });
        }
    }
}
