using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        /// Use REFLECTION to locate all recipes, execute them and save the output images.
        /// </summary>
        /// <returns>array of recipes found using reflection</returns>
        public static IRecipe[] CreateCookbookImages(string outputPath, int width = 600, int height = 400, int thumbJpegQuality = 95)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            IRecipe[] recipes = Locate.GetRecipes();

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

            return recipes;
        }

        /// <summary>
        /// Use SOURCE CODE FILE PARSING to locate all recipes in the project and store their information in a JSON file
        /// </summary>
        /// <returns>array of recipes found using source code file parsing</returns>
        public static RecipeSource[] CreateRecipesJson(string cookbookFolder, string saveFilePath, int width = 600, int height = 400)
        {
            RecipeSource[] recipes = SourceParsing.GetRecipeSources(cookbookFolder, width, height);

            using var stream = File.OpenWrite(saveFilePath);
            var options = new JsonWriterOptions() { Indented = true };
            using var writer = new Utf8JsonWriter(stream, options);

            writer.WriteStartObject();
            writer.WriteString("version", ScottPlot.Plot.Version);
            writer.WriteString("generated", DateTime.UtcNow);

            writer.WriteStartArray("recipes");
            foreach (RecipeSource recipe in recipes)
            {
                writer.WriteStartObject();
                writer.WriteString("id", recipe.ID.ToLower());
                writer.WriteString("category", recipe.Category);
                writer.WriteString("title", recipe.Title);
                writer.WriteString("description", recipe.Description);
                writer.WriteString("code", recipe.Code.Replace("\r", ""));
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

            return recipes;
        }
    }
}
