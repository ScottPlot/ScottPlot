using ScottPlot.Diagnostic.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace ScottPlot.Diagnostic
{
    public class CheckFiledsOrderedAccendingDecorator : PlottableDecoratorBase
    {
        private FieldInfo[] fieldsToCheck;

        public CheckFiledsOrderedAccendingDecorator(Plottable plottable) : base(plottable)
        {
            fieldsToCheck = sourcePlottable.GetType().GetFields().Where(f => f.IsDefined(typeof(AccendingAttribute))).ToArray();
        }

        protected override void BeforeRenderCheck()
        {
            foreach (var fi in fieldsToCheck)
            {
                if (fi.FieldType.IsArray)
                {
                    var elementType = fi.FieldType.GetElementType();
                    if (typeof(IComparable).IsAssignableFrom(elementType))
                    {
                        var dArray = (Array)fi.GetValue(sourcePlottable);
                        for (int i = 1; i < dArray.Length; i++)
                        {
                            if (((IComparable)dArray.GetValue(i)).CompareTo(dArray.GetValue(i - 1)) < 0)
                            {
                                throw new ArrayTypeMismatchException($"{sourcePlottable}: {fi.Name} contain not accending values,"
                                    + $" {fi.Name}[{i}] = {dArray.GetValue(i)} < {fi.Name}[{i - 1}] = {dArray.GetValue(i - 1)}");
                            }
                        }
                    }
                }
            }
        }
    }
}
