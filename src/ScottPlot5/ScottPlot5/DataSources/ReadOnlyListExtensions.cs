
namespace ScottPlot.DataSources
{
    internal static class ReadOnlyListExtensions
    {
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
        /// List view which zips two lists together by applying a mapping function.
        /// A view-only equivalent of Enumerable#Zip(...).
        /// </summary>
        /// <typeparam name="TFirst">type of the first input list</typeparam>
        /// <typeparam name="TSecond">type of the second input list</typeparam>
        /// <typeparam name="TOutput">type of the combined output list</typeparam>
        private class ZipListView<TFirst, TSecond, TOutput> : IReadOnlyList<TOutput>
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

            public TOutput this[int index] => _zippingFunction.Invoke(_firstInput[index], _secondInput[index]);

            public int Count => Math.Min(_firstInput.Count, _secondInput.Count);

            public IEnumerator<TOutput> GetEnumerator()
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

        /// <summary>
        /// List view which skips N elements from the start of the list.
        /// A view-only equivalent of Enumerable#Skip(N).
        /// </summary>
        /// <typeparam name="T">type of the original list</typeparam>
        private class SkipListView<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> _originalList;
            private readonly int _toSkip;

            public SkipListView(IReadOnlyList<T> originalList, int toSkip)
            {
                _originalList = originalList;
                _toSkip = toSkip;
            }

            public T this[int index]
            {
                get
                {
                    int mappedIndex = index + _toSkip;
                    return _originalList[mappedIndex];
                }
            }

            public int Count => Math.Max(_originalList.Count - _toSkip, 0);

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

        /// <summary>
        /// List view which takes N elements from the start of the list.
        /// A view-only equivalent of Enumerable#Take(N).
        /// </summary>
        /// <typeparam name="T">type of the original list</typeparam>
        private class TakeListView<T> : IReadOnlyList<T>
        {
            private readonly IReadOnlyList<T> _originalList;
            private readonly int _toTake;

            public TakeListView(IReadOnlyList<T> originalList, int toTake)
            {
                _originalList = originalList;
                _toTake = toTake;
            }

            public T this[int index]
            {
                get
                {
                    if (index > _toTake) throw new ArgumentOutOfRangeException(nameof(index));
                    return _originalList[index];
                }
            }

            public int Count => Math.Min(_originalList.Count, _toTake);

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
