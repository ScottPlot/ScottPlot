using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Tools
{
    class ChangingBase
    {
        [TestCase(255, 16, "FF")]
        [TestCase(112, 16, "70")]
        [TestCase(127, 16, "7F")]
        [TestCase(255, 2, "11111111")]
        [TestCase(15, 2, "1111")]
        [TestCase(0, 2, "0")]
        [TestCase(-0, 2, "0")]
        [TestCase(0x10000000, 16, "10000000")]
        [TestCase(0x20000000, 16, "20000000")]
        [TestCase(0x21, 16, "21")]

        [TestCase(-255, 16, "-FF")]
        [TestCase(-112, 16, "-70")]
        [TestCase(-127, 16, "-7F")]
        [TestCase(-255, 2, "-11111111")]
        [TestCase(-15, 2, "-1111")]
        [TestCase(-0x10000000, 16, "-10000000")]
        [TestCase(-0x20000000, 16, "-20000000")]
        [TestCase(-0x21, 16, "-21")]

        public void TestIntegers(double number, int radix, string expectedOutput)
        {
            Assert.AreEqual(expectedOutput, ScottPlot.Tools.ToDifferentBase(number, radix));
        }

        [TestCase(200.1, 16, 3, true, "C8.19A")]
        [TestCase(200.1, 16, 9, true, "C8.19999999A")]
        [TestCase(200.1, 2, 5, true, "11001000.00011")]
        [TestCase(200.1, 2, 3, true, "11001000")]
        [TestCase(255.3, 16, 3, true, "FF.4CD")]
        [TestCase(255.3, 16, 9, true, "FF.4CCCCCCCD")]
        [TestCase(255.3, 2, 3, true, "11111111")]
        [TestCase(255.3, 2, 9, true, "11111111.01001101")]
        [TestCase(255.3, 2, 3, false, "11111111.000")]
        [TestCase(255.3, 2, 9, false, "11111111.010011010")]
        [TestCase(255.3, 16, 1, true, "FF.5")]
        [TestCase(255.3, 2, 1, true, "11111111")]
        [TestCase(268435456.008056640625, 16, 5, true, "10000000.021")]
        [TestCase(0.008056640625, 16, 5, true, "0.021")]


        [TestCase(-200.1, 16, 3, true, "-C8.19A")]
        [TestCase(-200.1, 16, 9, true, "-C8.19999999A")]
        [TestCase(-200.1, 2, 5, true, "-11001000.00011")]
        [TestCase(-200.1, 2, 3, true, "-11001000")]
        [TestCase(-255.3, 16, 3, true, "-FF.4CD")]
        [TestCase(-255.3, 16, 9, true, "-FF.4CCCCCCCD")]
        [TestCase(-255.3, 2, 3, true, "-11111111")]
        [TestCase(-255.3, 2, 9, true, "-11111111.01001101")]
        [TestCase(-255.3, 2, 3, false, "-11111111.000")]
        [TestCase(-255.3, 2, 9, false, "-11111111.010011010")]
        [TestCase(-255.3, 16, 1, true, "-FF.5")]
        [TestCase(-255.3, 2, 1, true, "-11111111")]
        [TestCase(-268435456.008056640625, 16, 5, true, "-10000000.021")]
        [TestCase(-0.008056640625, 16, 5, true, "-0.021")]
        public void TestDecimals(double number, int radix, int decimalPlaces, bool dropTrailingZeroes, string expectedOutput)
        {
            Assert.AreEqual(expectedOutput, ScottPlot.Tools.ToDifferentBase(number, radix, decimalPlaces, dropTrailingZeroes: dropTrailingZeroes));
        }

        [TestCase(100, 17)] //If we add extra symbols (i.e. base64) this will no longer throw
        [TestCase(100, -1)]
        [TestCase(100, 0)]
        [TestCase(100, 1)]
        public void TestOutOfRange(double number, int radix)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ScottPlot.Tools.ToDifferentBase(number, radix));
        }
    }
}
