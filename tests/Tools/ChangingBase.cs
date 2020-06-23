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
		public void TestIntegers(double number, double radix, string expectedOutput)
		{
			Assert.AreEqual(expectedOutput, ScottPlot.Tools.ToDifferentBase(number, radix));
		}

		[TestCase(200.1, 16, 3, "C8.19A")]
		[TestCase(200.1, 16, 9, "C8.19999999A")]
		[TestCase(200.1, 2, 5, "11001000.00011")]
		[TestCase(200.1, 2, 3, "11001000")]
		[TestCase(255.3, 16, 3, "FF.4CD")]
		[TestCase(255.3, 16, 9, "FF.4CCCCCCCD")]
		[TestCase(255.3, 2, 3, "11111111.000")]
		[TestCase(255.3, 2, 9, "11111111.010011010")]
		[TestCase(255.3, 16, 1, "FF.5")]
		[TestCase(255.3, 2, 1, "11111111")]
		public void TestDecimals(double number, double radix, int decimalPlaces, string expectedOutput)
		{
			Assert.AreEqual(expectedOutput, ScottPlot.Tools.ToDifferentBase(number, radix, decimalPlaces));
		}
	}
}
