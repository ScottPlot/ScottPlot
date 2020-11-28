using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    partial class Plot
    {
        /* Obsolete methods should go in the files most relevant to their function.
         * Methods with no current relevant function go here. 
         */

        [Obsolete("use ValidatePlottableData() to validate data in plottables")]
        public bool DiagnosticMode;
    }
}
