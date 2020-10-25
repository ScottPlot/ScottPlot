using System;
using System.Linq;
using System.Reflection;
using ScottPlot.Plottable;

namespace ScottPlot.Diagnostic
{
    public class FieldsCheckerBase
    {
        protected Attribute[] AttributesToCheck;

        public FieldsCheckerBase()
        {
            AttributesToCheck = new Attribute[0];
        }

        protected FieldInfo[] GetFieldsToCheck(IPlottable plottable)
        {
            var fieldsToCheck = plottable.GetType().GetFields().Where(f => AttributesToCheck.Any(a => f.IsDefined(a.GetType(), false))).ToArray();
            return fieldsToCheck;
        }

        public virtual bool Check(IPlottable plottable)
        {
            return true;
        }
    }
}
