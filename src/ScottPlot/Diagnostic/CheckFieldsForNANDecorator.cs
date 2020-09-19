using ScottPlot.Diagnostic.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace ScottPlot.Diagnostic
{
    public class CheckFieldsForNANDecorator : PlottableDecoratorBase
    {
        private FieldInfo[] fieldsToCheck;

        public CheckFieldsForNANDecorator(Plottable plottable) : base(plottable)
        {
            fieldsToCheck = sourcePlottable.GetType().GetFields().Where(f => f.IsDefined(typeof(NotNANAttribute))).ToArray();
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
                            if (Double.IsNaN(dArray[i]))
                                throw new ArithmeticException($"{sourcePlottable}: {fi.Name}[{i}] is NAN");
                        }
                    }
                    if (elementType == typeof(float))
                    {
                        var dArray = (float[])fi.GetValue(sourcePlottable);
                        for (int i = 0; i < dArray.Length; i++)
                        {
                            if (float.IsNaN(dArray[i]))
                                throw new ArithmeticException($"{sourcePlottable}: {fi.Name}[{i}] is NAN");
                        }
                    }
                }
                else
                {
                    if (fi.FieldType == typeof(double))
                    {
                        var value = (double)fi.GetValue(sourcePlottable);
                        if (Double.IsNaN(value))
                            throw new ArithmeticException($"{sourcePlottable}: {fi.Name} is NAN");
                    }
                    if (fi.FieldType == typeof(float))
                    {
                        var value = (float)fi.GetValue(sourcePlottable);
                        if (float.IsNaN(value))
                            throw new ArithmeticException($"{sourcePlottable}: {fi.Name} is NAN");
                    }
                }
            }
        }
    }
}
