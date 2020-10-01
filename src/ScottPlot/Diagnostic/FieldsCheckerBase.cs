using System;
using System.Linq;
using System.Reflection;

namespace ScottPlot.Diagnostic
{
    public class FieldsCheckerBase
    {
        protected Attribute[] AttributesToCheck;

        public FieldsCheckerBase()
        {
            AttributesToCheck = new Attribute[0];
        }

        protected FieldInfo[] GetFieldsToCheck(Plottable plottable)
        {
            var fieldsToCheck = plottable.GetType().GetFields().Where(f => AttributesToCheck.Any(a => f.IsDefined(a.GetType(), false))).ToArray();
            return fieldsToCheck;
        }

        public virtual bool Check(Plottable plottable)
        {
            return true;
        }
    }
}
