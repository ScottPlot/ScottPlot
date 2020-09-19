using ScottPlot.Diagnostic.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace ScottPlot.Diagnostic
{
    public class CheckFieldsForInfinityDecorator : PlottableDecoratorBase
    {
        private FieldInfo[] fieldsToCheck;

        public CheckFieldsForInfinityDecorator(Plottable plottable) : base(plottable)
        {
            fieldsToCheck = sourcePlottable.GetType().GetFields().Where(f => f.IsDefined(typeof(NotInfinityAttribute))).ToArray();
        }

        protected override void BeforeRenderCheck()
        {
            foreach (var fi in fieldsToCheck)
            {
                if (fi.FieldType.IsArray)
                {
                    var elementType = fi.FieldType.GetElementType();
                    if (elementType == typeof(double))
                    {
                        var dArray = (double[])fi.GetValue(sourcePlottable);
                        for (int i = 0; i < dArray.Length; i++)
                        {
                            if (double.IsInfinity(dArray[i]))
                                throw new ArithmeticException($"{sourcePlottable}: {fi.Name}[{i}] = {dArray[i]}");
                        }
                    }
                    if (elementType == typeof(float))
                    {
                        var dArray = (float[])fi.GetValue(sourcePlottable);
                        for (int i = 0; i < dArray.Length; i++)
                        {
                            if (float.IsInfinity(dArray[i]))
                                throw new ArithmeticException($"{sourcePlottable}: {fi.Name}[{i}] = {dArray[i]}");
                        }
                    }
                }
                else
                {
                    if (fi.FieldType == typeof(double))
                    {
                        var value = (double)fi.GetValue(sourcePlottable);
                        if (double.IsNaN(value))
                            throw new ArithmeticException($"{sourcePlottable}: {fi.Name} = {value}");
                    }
                    if (fi.FieldType == typeof(float))
                    {
                        var value = (float)fi.GetValue(sourcePlottable);
                        if (float.IsNaN(value))
                            throw new ArithmeticException($"{sourcePlottable}: {fi.Name} = {value}");
                    }
                }
            }
        }
    }
}
