using ScottPlot.Diagnostic.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace ScottPlot.Diagnostic
{
    public class EqualLengthChecker : FieldsCheckerBase
    {
        public EqualLengthChecker()
        {
            AttributesToCheck = new Attribute[] { new EqualLengthAttribute() };
        }

        public override bool Check(Plottable plottable)
        {
            var fieldsToCheck = GetFieldsToCheck(plottable);
            (FieldInfo fi, int? Length)[] fieldsWithLength = fieldsToCheck.Select(fi =>
            {
                if (fi.GetValue(plottable) == null)
                {
                    return (fi, null);
                }
                int? Length = 1;
                if (fi.FieldType.IsArray)
                {
                    var array = (Array)fi.GetValue(plottable);
                    Length = array.Length;
                }
                return (fi, Length);
            }).ToArray();
            var NotEqualFields = fieldsWithLength
                                  .SelectMany(f => fieldsWithLength, (f1, f2) => (f1, f2))
                                  .Where(fPair =>
                                  {
                                      if (fPair.f1.Length == null || fPair.f2.Length == null)
                                          return false;

                                      return fPair.f1.Length != fPair.f2.Length;
                                  });

            foreach (var fp in NotEqualFields)
            {
                throw new ArrayTypeMismatchException($"{fp.f1.fi.Name}.Length({fp.f1.Length}) != {fp.f2.fi.Name}.Length({fp.f2.Length})");
            }

            return true;
        }
    }
}
