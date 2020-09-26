using ScottPlot.Diagnostic.Attributes;
using System;

namespace ScottPlot.Diagnostic
{
    public class AccendingChecker : FieldsCheckerBase
    {
        public AccendingChecker()
        {
            AttributesToCheck = new Attribute[] { new AccendingAttribute() };
        }

        public override bool Check(Plottable plottable)
        {
            var fieldsToCheck = GetFieldsToCheck(plottable);
            foreach (var fi in fieldsToCheck)
            {
                if (fi.FieldType.IsArray)
                {
                    var elementType = fi.FieldType.GetElementType();
                    if (typeof(IComparable).IsAssignableFrom(elementType))
                    {
                        var dArray = (Array)fi.GetValue(plottable);
                        for (int i = 1; i < dArray.Length; i++)
                        {
                            if (((IComparable)dArray.GetValue(i)).CompareTo(dArray.GetValue(i - 1)) < 0)
                            {
                                throw new ArrayTypeMismatchException($"{plottable}: {fi.Name} contain not accending values,"
                                    + $" {fi.Name}[{i}] = {dArray.GetValue(i)} < {fi.Name}[{i - 1}] = {dArray.GetValue(i - 1)}");
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
