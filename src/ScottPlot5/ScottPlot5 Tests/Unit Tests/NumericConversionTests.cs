using System;

namespace SharedTests;

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

    [Test]
    public void Test_TypeSpecificFunction_Add()
    {
        Assert.Multiple(() =>
        {
            Assert.That(NumericConversion.CreateAddFunction<double>().Invoke(42, 69), Is.EqualTo(111));
            Assert.That(NumericConversion.CreateAddFunction<float>().Invoke(42, 69), Is.EqualTo(111));
            Assert.That(NumericConversion.CreateAddFunction<int>().Invoke(42, 69), Is.EqualTo(111));
            Assert.That(NumericConversion.CreateAddFunction<byte>().Invoke(42, 69), Is.EqualTo(111));
        });
    }

    [Test]
    public void Test_TypeSpecificFunction_Multiply()
    {
        Assert.Multiple(() =>
        {
            Assert.That(NumericConversion.CreateMultFunction<double>().Invoke(42, 69), Is.EqualTo(2898));
            Assert.That(NumericConversion.CreateMultFunction<float>().Invoke(42, 69), Is.EqualTo(2898));
            Assert.That(NumericConversion.CreateMultFunction<int>().Invoke(42, 69), Is.EqualTo(2898));
            Assert.That(NumericConversion.CreateMultFunction<byte>().Invoke(15, 2), Is.EqualTo(30));
        });
    }

    [Test]
    public void Test_TypeSpecificFunction_Subtract()
    {
        Assert.Multiple(() =>
        {
            Assert.That(NumericConversion.CreateSubtractFunction<double>().Invoke(111, 69), Is.EqualTo(42));
            Assert.That(NumericConversion.CreateSubtractFunction<float>().Invoke(111, 69), Is.EqualTo(42));
            Assert.That(NumericConversion.CreateSubtractFunction<int>().Invoke(111, 69), Is.EqualTo(42));
            Assert.That(NumericConversion.CreateSubtractFunction<byte>().Invoke(111, 69), Is.EqualTo(42));
        });
    }

    [Test]
    public void Test_TypeSpecificFunction_LessThanOrEqual()
    {
        Assert.Multiple(() =>
        {
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<double>().Invoke(111, 69), Is.False);
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<double>().Invoke(69, 69), Is.True);
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<double>().Invoke(42, 69), Is.True);

            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<float>().Invoke(111, 69), Is.False);
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<float>().Invoke(69, 69), Is.True);
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<float>().Invoke(42, 69), Is.True);

            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<int>().Invoke(111, 69), Is.False);
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<int>().Invoke(69, 69), Is.True);
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<int>().Invoke(42, 69), Is.True);

            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<byte>().Invoke(111, 69), Is.False);
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<byte>().Invoke(69, 69), Is.True);
            Assert.That(NumericConversion.CreateLessThanOrEqualFunction<byte>().Invoke(42, 69), Is.True);
        });
    }

    [Test]
    public void Test_Clamp()
    {
        NumericConversion.Clamp(3, 5, 10).Should().Be(5);
        NumericConversion.Clamp(5, 5, 10).Should().Be(5);
        NumericConversion.Clamp(7, 5, 10).Should().Be(7);
        NumericConversion.Clamp(10, 5, 10).Should().Be(10);
        NumericConversion.Clamp(13, 5, 10).Should().Be(10);
    }
}
