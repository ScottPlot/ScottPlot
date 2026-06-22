using ScottPlot.DataSources;
using System.Reflection;

namespace ScottPlotTests.UnitTests;

internal class ReadOnlyListExtensionsTests
{
    [Test]
    public void Test_ReadOnlyListExtensions_SelectView_IsLazy()
    {
        List<int> source = [1, 2, 3];
        int callCount = 0;

        IReadOnlyList<int> view = SelectView<int, int>(source, x =>
        {
            callCount++;
            return x * 10;
        });

        Assert.That(view.Count, Is.EqualTo(3));
        Assert.That(callCount, Is.EqualTo(0));

        source[1] = 7;

        Assert.That(view[1], Is.EqualTo(70));
        Assert.That(callCount, Is.EqualTo(1));
        Assert.That(view.ToArray(), Is.EqualTo(new[] { 10, 70, 30 }));
        Assert.That(callCount, Is.EqualTo(4));
    }

    [Test]
    public void Test_ReadOnlyListExtensions_ZipView_IsLazyAndUsesShortestList()
    {
        List<int> xs = [1, 2, 3];
        List<int> ys = [10, 20];

        IReadOnlyList<int> view = ZipView<int, int, int>(xs, ys, (x, y) => x + y);

        Assert.That(view.Count, Is.EqualTo(2));

        xs[1] = 5;
        ys[1] = 40;

        Assert.That(view.ToArray(), Is.EqualTo(new[] { 11, 45 }));
    }

    [Test]
    public void Test_ReadOnlyListExtensions_GrowView_ExpandsItems()
    {
        string[] source = ["A", "B", "C"];

        IReadOnlyList<string> view = GrowView(source, 3, (step, originalIndex, originalList) =>
            $"{originalList[originalIndex]}-{step}");

        Assert.That(view.Count, Is.EqualTo(9));
        Assert.That(view.ToArray(), Is.EqualTo(new[]
        {
            "A-0", "A-1", "A-2",
            "B-0", "B-1", "B-2",
            "C-0", "C-1", "C-2",
        }));
    }

    [TestCase(1, 3, new[] { 2, 3, 4 })]
    [TestCase(-1, 2, new[] { 1, 2 })]
    [TestCase(2, 99, new[] { 3, 4, 5 })]
    [TestCase(99, 3, new int[] { })]
    [TestCase(0, -1, new int[] { })]
    public void Test_ReadOnlyListExtensions_SkipViewAndTakeView_MatchLinqBoundaries(int skip, int take, int[] expected)
    {
        int[] source = [1, 2, 3, 4, 5];

        IReadOnlyList<int> view = TakeView(SkipView(source, skip), take);

        Assert.That(view.ToArray(), Is.EqualTo(expected));
    }

    [Test]
    public void Test_ReadOnlyListExtensions_TakeView_ThrowsForFirstIndexOutsideView()
    {
        int[] source = [1, 2, 3];

        IReadOnlyList<int> view = TakeView(source, 2);

        Assert.That(view.Count, Is.EqualTo(2));
        Assert.Throws<ArgumentOutOfRangeException>(() => _ = view[2]);
    }

    private static IReadOnlyList<TOutput> SelectView<TInput, TOutput>(
        IReadOnlyList<TInput> inputList,
        Func<TInput, TOutput> mappingFunction)
    {
        return InvokeReadOnlyListExtension<TOutput>(
            nameof(SelectView),
            [typeof(TInput), typeof(TOutput)],
            inputList,
            mappingFunction);
    }

    private static IReadOnlyList<T> GrowView<T>(
        IReadOnlyList<T> inputList,
        uint growthFactor,
        Func<int, int, IReadOnlyList<T>, T> growthFunction)
    {
        return InvokeReadOnlyListExtension<T>(
            nameof(GrowView),
            [typeof(T)],
            inputList,
            growthFactor,
            growthFunction);
    }

    private static IReadOnlyList<TOutput> ZipView<TFirst, TSecond, TOutput>(
        IReadOnlyList<TFirst> inputList,
        IReadOnlyList<TSecond> toZipWith,
        Func<TFirst, TSecond, TOutput> zippingFunction)
    {
        return InvokeReadOnlyListExtension<TOutput>(
            nameof(ZipView),
            [typeof(TFirst), typeof(TSecond), typeof(TOutput)],
            inputList,
            toZipWith,
            zippingFunction);
    }

    private static IReadOnlyList<T> SkipView<T>(IReadOnlyList<T> inputList, int toSkip)
    {
        return InvokeReadOnlyListExtension<T>(
            nameof(SkipView),
            [typeof(T)],
            inputList,
            toSkip);
    }

    private static IReadOnlyList<T> TakeView<T>(IReadOnlyList<T> inputList, int toTake)
    {
        return InvokeReadOnlyListExtension<T>(
            nameof(TakeView),
            [typeof(T)],
            inputList,
            toTake);
    }

    private static IReadOnlyList<T> InvokeReadOnlyListExtension<T>(
        string methodName,
        Type[] genericTypes,
        params object[] parameters)
    {
        Type extensionType = typeof(ScatterSourceDoubleArray).Assembly
            .GetType("ScottPlot.DataSources.ReadOnlyListExtensions")!;

        MethodInfo method = extensionType
            .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(x => x.Name == methodName);

        return (IReadOnlyList<T>)method
            .MakeGenericMethod(genericTypes)
            .Invoke(null, parameters)!;
    }
}
