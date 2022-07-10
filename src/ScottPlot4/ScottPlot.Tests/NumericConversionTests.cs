using NUnit.Framework;
using System;

namespace ScottPlot.Tests
{
    internal class NumericConversionTests
    {
        private static void AssertConversionPreservesOriginalValue<T>(double originalValue, double within = 0)
        {
            NumericConversion.DoubleToGeneric(originalValue, out T genericValue);
            double finalValue = NumericConversion.GenericToDouble(ref genericValue);
            Assert.That(finalValue, Is.EqualTo(originalValue).Within(within), $"Type: {typeof(T)}");
        }

        [Test]
        public void Test_NoAllocationConversion_PreservesOriginalValue()
        {
            AssertConversionPreservesOriginalValue<double>(42.69);
            AssertConversionPreservesOriginalValue<float>(42.69, 1e-4);
            AssertConversionPreservesOriginalValue<int>(42);
            AssertConversionPreservesOriginalValue<byte>(42);
            AssertConversionPreservesOriginalValue<decimal>(42.69);

            AssertConversionPreservesOriginalValue<Int16>(42);
            AssertConversionPreservesOriginalValue<Int32>(42);
            AssertConversionPreservesOriginalValue<Int64>(42);

            AssertConversionPreservesOriginalValue<UInt16>(42);
            AssertConversionPreservesOriginalValue<UInt32>(42);
            AssertConversionPreservesOriginalValue<UInt64>(42);
        }
    }
}
