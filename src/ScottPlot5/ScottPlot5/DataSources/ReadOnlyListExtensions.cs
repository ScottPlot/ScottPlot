
namespace ScottPlot.DataSources
{
    internal static class ReadOnlyListExtensions
    {
        internal static IReadOnlyList<TOutput> SelectView<TInput, TOutput>(this IReadOnlyList<TInput> inputList,
            Func<TInput, TOutput> mappingFunction)
        {
            return new SelectListView<TInput, TOutput>(inputList, mappingFunction);
        }

        internal static IReadOnlyList<T> GrowView<T>(this IReadOnlyList<T> inputList, uint growthFactor,
            Func<int, int, IReadOnlyList<T>, T> growthFunction)
        {
            return new GrowListView<T>(inputList, growthFactor, growthFunction);
        }

        internal static IReadOnlyList<TOutput> ZipView<TFirst, TSecond, TOutput>(this IReadOnlyList<TFirst> inputList,
            IReadOnlyList<TSecond> toZipWith, Func<TFirst, TSecond, TOutput> zippingFunction)
        {
            return new ZipListView<TFirst, TSecond, TOutput>(inputList, toZipWith, zippingFunction);
        }

        internal static IReadOnlyList<T> SkipView<T>(this IReadOnlyList<T> inputList, int toSkip)
        {
            return new SkipListView<T>(inputList, toSkip);
        }

        internal static IReadOnlyList<T> TakeView<T>(this IReadOnlyList<T> inputList, int toTake)
        {
            return new TakeListView<T>(inputList, toTake);
        }

        /// <summary>
        /// List view which projects elements from the original list onto a new form.
        /// A view-only equivalent of Enumerable#Select(...).
        /// </summary>
        /// <typeparam name="TInput">type of the original list</typeparam>
        /// <typeparam name="TOutput">type of the output list</typeparam>
        private class SelectListView<TInput, TOutput> : ListViewDecorator<TOutput>
        {
            private readonly IReadOnlyList<TInput> _inputList;
            private readonly Func<TInput, TOutput> _mappingFunction;

            public SelectListView(IReadOnlyList<TInput> inputList, Func<TInput, TOutput> mappingFunction)
            {
                _inputList = inputList;
                _mappingFunction = mappingFunction;
            }

            public override TOutput this[int index] => _mappingFunction.Invoke(_inputList[index]);

            public override int Count => _inputList.Count;
        }

        /// <summary>
        /// List view which grows the number of elements by a positive integer factor.
        /// A growth function is applied to each element such that element 0 could expand into
        /// element 0, 1, 2, ..., (_growthFactor - 1).
        /// </summary>
        /// <example>
        /// The below example illustrates using this view to double the size of a list of strings,
        /// adding the number of each duplicate to the end of each string.
        /// <code>
        /// <![CDATA[
        /// IReadOnlyList<string> original = new string[] {"A", "B", "C"};
        /// print(original); //"A", "B", "C"
        /// IReadOnlyList<string> grown = new GrowListView<string>(
        ///     original, 3, (step, originalIndex, originalList) => $"{originalList[originalIndex]}-{step}");
        /// print(grown); //"A-0", "A-1", "A-2", "B-0", "B-1", "B-2", "C-0", "C-1", "C-2"
        /// ]]>
        /// </code>
        /// </example>
        /// <typeparam name="T">type of the input list</typeparam>
        private class GrowListView<T> : ListViewDecorator<T>
        {
            private readonly IReadOnlyList<T> _inputList;
            private readonly uint _growthFactor;
            private readonly Func<int, int, IReadOnlyList<T>, T> _growthFunction;

            public GrowListView(IReadOnlyList<T> inputList, uint growthFactor,
                Func<int, int, IReadOnlyList<T>, T> growthFunction)
            {
                _inputList = inputList;
                _growthFactor = growthFactor;
                _growthFunction = growthFunction;
            }

            public override T this[int index] => _growthFunction.Invoke(
                (int)(index % _growthFactor), //current step (starts at zero)
                (int)Math.Floor((double)index / _growthFactor), //original index
                _inputList); //original list

            public override int Count => (int)(_inputList.Count * _growthFactor);
        }

        /// <summary>
        /// List view which zips two lists together by applying a mapping function.
        /// A view-only equivalent of Enumerable#Zip(...).
        /// </summary>
        /// <typeparam name="TFirst">type of the first input list</typeparam>
        /// <typeparam name="TSecond">type of the second input list</typeparam>
        /// <typeparam name="TOutput">type of the combined output list</typeparam>
        private class ZipListView<TFirst, TSecond, TOutput> : ListViewDecorator<TOutput>
        {
            private readonly IReadOnlyList<TFirst> _firstInput;
            private readonly IReadOnlyList<TSecond> _secondInput;
            private readonly Func<TFirst, TSecond, TOutput> _zippingFunction;

            public ZipListView(IReadOnlyList<TFirst> firstInput, IReadOnlyList<TSecond> secondInput,
                Func<TFirst, TSecond, TOutput> zippingFunction)
            {
                _firstInput = firstInput;
                _secondInput = secondInput;
                _zippingFunction = zippingFunction;
            }

            public override TOutput this[int index] =>
                _zippingFunction.Invoke(_firstInput[index], _secondInput[index]);

            public override int Count => Math.Min(_firstInput.Count, _secondInput.Count);
        }

        /// <summary>
        /// List view which skips N elements from the start of the list.
        /// A view-only equivalent of Enumerable#Skip(N).
        /// </summary>
        /// <typeparam name="T">type of the original list</typeparam>
        private class SkipListView<T> : ListViewDecorator<T>
        {
            private readonly IReadOnlyList<T> _originalList;
            private readonly int _toSkip;

            public SkipListView(IReadOnlyList<T> originalList, int toSkip)
            {
                _originalList = originalList;
                _toSkip = toSkip;
            }

            public override T this[int index]
            {
                get
                {
                    int mappedIndex = index + _toSkip;
                    return _originalList[mappedIndex];
                }
            }

            public override int Count => Math.Max(_originalList.Count - _toSkip, 0);
        }

        /// <summary>
        /// List view which takes N elements from the start of the list.
        /// A view-only equivalent of Enumerable#Take(N).
        /// </summary>
        /// <typeparam name="T">type of the original list</typeparam>
        private class TakeListView<T> : ListViewDecorator<T>
        {
            private readonly IReadOnlyList<T> _originalList;
            private readonly int _toTake;

            public TakeListView(IReadOnlyList<T> originalList, int toTake)
            {
                _originalList = originalList;
                _toTake = toTake;
            }

            public override T this[int index]
            {
                get
                {
                    if (index > _toTake) throw new ArgumentOutOfRangeException(nameof(index));
                    return _originalList[index];
                }
            }

            public override int Count => Math.Min(_originalList.Count, _toTake);
        }

        private abstract class ListViewDecorator<T> : IReadOnlyList<T>
        {
            public abstract T this[int index] { get; }

            public abstract int Count { get; }

            public IEnumerator<T> GetEnumerator()
            {
                for (int i = 0; i < Count; i++)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
