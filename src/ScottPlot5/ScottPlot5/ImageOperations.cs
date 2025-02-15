namespace ScottPlot;

public static class ImageOperations
{
    public static string GetImageHtml(byte[] bytes, string imageType = "png", string classContent = "", string styleContent = "")
    {
        string b64 = Convert.ToBase64String(bytes);
        return $"<img class='{classContent}' style='{styleContent}' src=\"data:image/{imageType};base64,{b64}\"></img>";
    }
}
