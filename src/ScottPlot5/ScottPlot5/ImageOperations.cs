using System;

namespace ScottPlot;

#nullable enable

public static class ImageOperations
{
    public static string GetImageHtml(byte[] bytes, string imageType = "png")
    {
        string b64 = Convert.ToBase64String(bytes);
        return $"<img src=\"data:image/{imageType};base64,{b64}\"></img>";
    }
}
