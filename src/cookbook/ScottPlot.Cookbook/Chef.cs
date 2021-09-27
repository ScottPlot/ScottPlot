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

        /// <summary>
        /// Use reflection to determine all IRecipe objects in the project, execute each of them, 
        /// and save the output using the recipe ID as its base filename.
        /// </summary>
        public void CreateCookbookImages(string outputPath, int thumbJpegQuality = 95)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            var recipes = Locate.GetRecipes();
            Console.WriteLine($"Cooking {recipes.Length} recipes in: {outputPath}");

            EncoderParameters thumbJpegEncoderParameters = new(1);
            thumbJpegEncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, thumbJpegQuality);
            ImageCodecInfo thumbJpegEncoder = ImageCodecInfo.GetImageEncoders().Where(x => x.MimeType == "image/jpeg").First();

            Parallel.ForEach(recipes, recipe =>
            {
                var plt = new Plot(Width, Height);
                recipe.ExecuteRecipe(plt);

                // save full size image
                Bitmap bmp = plt.Render();
                string filePath = Path.Combine(outputPath, recipe.ID.ToLower() + ".png");
                bmp.Save(filePath, ImageFormat.Png);
                Debug.WriteLine($"Saved: {filePath}");

                // thumbnail
                int thumbHeight = 180;
                int thumbWidth = thumbHeight * bmp.Width / bmp.Height;
                Bitmap thumb = Drawing.GDI.Resize(bmp, thumbWidth, thumbHeight);
                string thumbFilePath = Path.Combine(outputPath, recipe.ID.ToLower() + "_thumb.jpg");
                thumb.Save(thumbFilePath, thumbJpegEncoder, thumbJpegEncoderParameters);
                Debug.WriteLine($"Saved: {thumbFilePath}");
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
                Directory.CreateDirectory(outputPath);

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
                Debug.WriteLine($"Saved: {filePath}");
            });
        }

        /// <summary>
        /// Read all .cs files in the source folder to identify IRecipe source code, isolate just the recipe 
        /// component of each source file, and save the output as a text file using the recipe ID as its base filename.
        /// </summary>
        public void CreateCookbookSourceV2(string sourcePath, string outputPath)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            RecipeSource[] sources = SourceParsing.GetRecipeSources(sourcePath, Width, Height);
            Console.WriteLine($"Creating source code for {sources.Length} recipes in: {outputPath}");

            foreach(RecipeSource recipe in sources)
            {
                StringBuilder sb = new();
                sb.AppendLine("/// ID: " + recipe.ID);
                sb.AppendLine("/// TITLE: " + recipe.Title);
                sb.AppendLine("/// CATEGORY: " + recipe.Category);
                sb.AppendLine("/// DESCRIPTION: " + recipe.Description);
                sb.Append(recipe.Code);

                string filePath = Path.Combine(outputPath, recipe.ID.ToLower() + ".cs");
                File.WriteAllText(filePath, sb.ToString());
                Debug.WriteLine($"Saved: {filePath}");
            }
        }
    }
}
