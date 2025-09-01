using System.Security.Cryptography;

namespace ScottPlot.Testing;

#pragma warning disable CA1850 // Use the older 'ComputeHash' because 'MD5.HashData' is not supported on all targets we build for
#pragma warning disable IDE0079 // Don't warn me that I'm using CA1850 lol

public static class MD5Hasher
{
    public static string GetHash(string input)
    {
        using MD5 md5Hash = MD5.Create();
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return string.Join(string.Empty, data.Select(x => x.ToString("x2")));
    }
}
