using NUnit.Framework;
using System;
using System.Linq;

namespace ScottPlotTests
{
    internal class NumericConversionTests
    {
        private static void AssertConversionPreservesOriginalValue<T>(double originalValue, double within = 0)
        {
            ScottPlot.NumericConversion.DoubleToGeneric(originalValue, out T genericValue);
            double finalValue = ScottPlot.NumericConversion.GenericToDouble(ref genericValue);
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

        [Test]
        public void Test_Signal_Types()
        {
            // see discussion in https://github.com/ScottPlot/ScottPlot/pull/1927

            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotConst<double>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotConst<float>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotConst<int>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotConst<byte>());

            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotXYGeneric<double, double>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotXYGeneric<float, float>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotXYGeneric<int, int>());
            Assert.DoesNotThrow(() => new ScottPlot.Plottable.SignalPlotXYGeneric<byte, byte>());
        }

        [Test]
        public void Test_GenericSignal_ByteArray()
        {
            double[] doubles = ScottPlot.DataGen.Sin(51, offset: 100, mult: 100);
            byte[] bytes = doubles.Select(x => (byte)(x + 5)).ToArray();

            ScottPlot.Plot plt = new();
            plt.AddSignalConst(doubles, label: "doubles");
            plt.AddSignalConst(bytes, label: "bytes");
            plt.Legend();
            TestTools.SaveFig(plt);
        }
    }
}
