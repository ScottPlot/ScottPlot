using SkiaSharp;
using System.IO;

namespace ScottPlot;

internal class SvgImage : IDisposable
{
    private bool IsDisposed = false;
    public readonly int Width;
    public readonly int Height;
    private readonly MemoryStream Stream;
    public readonly SKCanvas Canvas;

    public SvgImage(int width, int height)
    {
        Width = width;
        Height = height;
        SKRect rect = new(0, 0, width, height);
        Stream = new MemoryStream();
        Canvas = SKSvgCanvas.Create(rect, Stream);        
    }

    public string GetXmlOriginal()
    {        
        var xmlString = Encoding.ASCII.GetString(Stream.ToArray()) + "</svg>";
        return xmlString;
    }


    public string GetXml()
    {
        MemoryStream destinationStream = new MemoryStream();

        Stream.Position = 0;

        // Copy the contents of the input MemoryStream to the output MemoryStream
        Stream.CopyTo(destinationStream);


        // Wrap the MemoryStream in an SKManagedWStream
        using (var managedStream = new SKManagedWStream(destinationStream))
        {
            // Create an SKXmlStreamWriter that writes to the SKManagedWStream
            using (var writer = new SKXmlStreamWriter(managedStream))
            {
                // Get the SKCanvas from the SKSvgCanvas
                SKCanvas canvas = Canvas;

                
                // Perform your drawing operations on the canvas
                // ...

                // Now write the SVG content to the memory stream
                // Note: The actual writing to the memory stream happens when the writer is disposed
            }
        }
    }




    var xmlString = Encoding.ASCII.GetString(destinationStream.ToArray()) + "</svg>";
        return xmlString;
    }

    public void Dispose()
    {
        if (IsDisposed)
            return;

        Canvas.Dispose();
        IsDisposed = true;

        GC.SuppressFinalize(this);
    }
}
