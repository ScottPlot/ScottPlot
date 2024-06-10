namespace ScottPlot.Testing;

/// <summary>
/// Helper class to detect for duplicate items in complex collections
/// and display helpful error messages to the console the facilitate debugging.
/// </summary>
public class DuplicateIdentifier<T>(string title)
{
    readonly List<KeyValuePair<string, T>> Things = [];

    readonly string Title = title;

    public void Add(string id, T thing)
    {
        Things.Add(new KeyValuePair<string, T>(id, thing));
    }

    public void ShouldHaveNoDuplicates()
    {
        HashSet<string> duplicateIDs = new();
        HashSet<string> seenIDs = new();
        for (int i = 0; i < Things.Count; i++)
        {
            string id = Things[i].Key;
            if (seenIDs.Contains(id))
            {
                duplicateIDs.Add(id);
            }
            seenIDs.Add(id);
        }

        if (!duplicateIDs.Any())
            return;

        StringBuilder sb = new();
        foreach (string id in duplicateIDs)
        {
            IEnumerable<T> duplicateThings = Things.Where(x => x.Key == id).Select(x => x.Value);
            string duplicateThingsString = string.Join(", ", duplicateThings);
            sb.AppendLine($"The {Title} \"{id}\" is not unique as it is shared by: {duplicateThingsString}");
        }
        throw new InvalidOperationException(sb.ToString());
    }
}
