using ScottPlot.Diagnostic.Attributes;
using System;

namespace ScottPlot.Diagnostic
{
    public class NANChecker : FieldsCheckerBase
    {
        public NANChecker()
        {
            AttributesToCheck = new Attribute[] { new NotNANAttribute(), new FiniteNumbersAttribute() };
        }

        public override bool Check(Plottable plottable)
        {
            var fieldsToCheck = GetFieldsToCheck(plottable);
            foreach (var fi in fieldsToCheck)
            {
                if (fi.GetValue(plottable) == null)
                    continue;
                if (fi.FieldType.IsArray)
                {
                    var elementType = fi.FieldType.GetElementType();
                    if (elementType == typeof(double))
                    {
                        var dArray = (double[])fi.GetValue(plottable);
                        for (int i = 0; i < dArray.Length; i++)
                        {
                            if (Double.IsNaN(dArray[i]))
                                throw new ArithmeticException($"{plottable}: {fi.Name}[{i}] is NAN");
                        }
                    }
                    if (elementType == typeof(float))
                    {
                        var dArray = (float[])fi.GetValue(plottable);
                        for (int i = 0; i < dArray.Length; i++)
                        {
                            if (float.IsNaN(dArray[i]))
                                throw new ArithmeticException($"{plottable}: {fi.Name}[{i}] is NAN");
                        }
                    }
                }
                else
                {
                    if (fi.FieldType == typeof(double))
                    {
                        var value = (double)fi.GetValue(plottable);
                        if (Double.IsNaN(value))
                            throw new ArithmeticException($"{plottable}: {fi.Name} is NAN");
                    }
                    if (fi.FieldType == typeof(float))
                    {
                        var value = (float)fi.GetValue(plottable);
                        if (float.IsNaN(value))
                            throw new ArithmeticException($"{plottable}: {fi.Name} is NAN");
                    }
                }
            }

            return true;
        }
    }
}
