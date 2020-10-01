using ScottPlot.Diagnostic.Attributes;
using System;

namespace ScottPlot.Diagnostic
{
    public class InfinityChecker : FieldsCheckerBase
    {
        public InfinityChecker()
        {
            AttributesToCheck = new Attribute[] { new NotInfinityAttribute(), new FiniteNumbersAttribute() };
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
                            if (double.IsInfinity(dArray[i]))
                                throw new ArithmeticException($"{plottable}: {fi.Name}[{i}] = {dArray[i]}");
                        }
                    }
                    if (elementType == typeof(float))
                    {
                        var dArray = (float[])fi.GetValue(plottable);
                        for (int i = 0; i < dArray.Length; i++)
                        {
                            if (float.IsInfinity(dArray[i]))
                                throw new ArithmeticException($"{plottable}: {fi.Name}[{i}] = {dArray[i]}");
                        }
                    }
                }
                else
                {
                    if (fi.FieldType == typeof(double))
                    {
                        var value = (double)fi.GetValue(plottable);
                        if (double.IsNaN(value))
                            throw new ArithmeticException($"{plottable}: {fi.Name} = {value}");
                    }
                    if (fi.FieldType == typeof(float))
                    {
                        var value = (float)fi.GetValue(plottable);
                        if (float.IsNaN(value))
                            throw new ArithmeticException($"{plottable}: {fi.Name} = {value}");
                    }
                }
            }

            return true;
        }
    }
}
