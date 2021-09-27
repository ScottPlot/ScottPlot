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
    public class Chef
    {
        public int Width = 600;
        public int Height = 400;

        const string Ext = ".png";
        const string ExtThumb = "_thumb.jpg";

        /// <summary>
        /// Use reflection to determine all IRecipe objects in the project, execute each of them, 
        /// and save the output using the recipe ID as its base filename.
        /// </summary>
        public void CreateCookbookImages(string outputPath)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            var recipes = Locate.GetRecipes();
            Console.WriteLine($"Cooking {recipes.Length} recipes in: {outputPath}");

            int thumbJpegQuality = 95;
            EncoderParameters thumbJpegEncoderParameters = new EncoderParameters(1);
            thumbJpegEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, thumbJpegQuality);
            ImageCodecInfo thumbJpegEncoder = ImageCodecInfo.GetImageEncoders().Where(x => x.MimeType == "image/jpeg").First();

            Parallel.ForEach(recipes, recipe =>
            {
                Debug.WriteLine($"Executing {recipe.ID}");
                var plt = new Plot(Width, Height);
                recipe.ExecuteRecipe(plt);

                // save full size image
                Bitmap bmp = plt.Render();
                string fileName = (recipe.ID + Ext).ToLower();
                string filePath = Path.Combine(outputPath, fileName);
                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);

                // thumbnail
                int thumbHeight = 180;
                int thumbWidth = thumbHeight * bmp.Width / bmp.Height;
                Bitmap thumb = Drawing.GDI.Resize(bmp, thumbWidth, thumbHeight);
                string thumbFileName = (recipe.ID + ExtThumb).ToLower();
                string thumbFilePath = Path.Combine(outputPath, thumbFileName);
                thumb.Save(thumbFilePath, thumbJpegEncoder, thumbJpegEncoderParameters);
            });

        }

        /// <summary>
        /// Read all .cs files in the source folder to identify IRecipe source code, isolate just the recipe 
        /// component of each source file, and save the output as a text file using the recipe ID as its base filename.
        /// </summary>
        public void CreateCookbookSource(string sourcePath, string outputPath)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                throw new ArgumentException($"output path does not exist: {outputPath}");

            var sources = SourceParsing.GetRecipeSources(sourcePath, Width, Height);
            Console.WriteLine($"Creating source code for {sources.Length} recipes in: {outputPath}");

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
