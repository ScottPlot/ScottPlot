using System.Collections.Generic;

namespace ScottPlot.Diagnostic
{
    public class DiagnosticDataChecker
    {
        private FieldsCheckerBase[] Checks;
        public DiagnosticDataChecker()
        {
            Checks = new FieldsCheckerBase[]
            {
                new InfinityChecker(),
                new NANChecker(),
                new AccendingChecker(),
                new EqualLengthChecker(),
            };
        }

        public void CheckPlottable(Plottable plottable)
        {
            var validatable = plottable as IValidatableData;
            if (validatable != null)
            {
                validatable.ValidateData();
            }
            else
            {
                foreach (var check in Checks)
                {
                    check.Check(plottable);
                }
            }
        }

        public void CheckPlottables(List<Plottable> plottables)
        {
            foreach (var plottable in plottables)
            {
                CheckPlottable(plottable);
            }
        }
    }
}

