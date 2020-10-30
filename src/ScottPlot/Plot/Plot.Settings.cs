/* Code here customizes Plot behavior (not styling) */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot
{
    partial class Plot
    {
        private bool ValidateEveryPoint = false;
        private ErrorAction ValidationErrorAction = ErrorAction.ShowErrorOnPlot;

        public void Validate(bool everyDataPoint, ErrorAction action = ErrorAction.ShowErrorOnPlot)
        {
            ValidateEveryPoint = everyDataPoint;
            ValidationErrorAction = action;
            if (everyDataPoint)
                Debug.WriteLine("WARNING: every data point will be validated on each render, reducing performance");
        }

        public void AntiAlias(bool figure = true, bool data = false, bool legend = false)
        {
            settings.misc.antiAliasFigure = figure;
            settings.misc.antiAliasData = data;
        }

        public void SetCulture(System.Globalization.CultureInfo culture)
        {
            settings.culture = culture;
        }

        /// <summary>
        /// Updates the used culture to match your requirements.
        /// </summary>
        /// <param name="shortDatePattern">
        /// https://docs.microsoft.com/dotnet/standard/base-types/custom-date-and-time-format-strings
        /// </param>
        /// <param name="decimalSeparator">
        /// Separates the decimal digits.
        /// </param>
        /// <param name="numberGroupSeparator">
        /// Separates large numbers ito groups of digits for readability.
        /// </param>
        /// <param name="decimalDigits">
        /// Number of digits after the numberDecimalSeparator.
        /// </param>
        /// <param name="numberNegativePattern">
        /// https://docs.microsoft.com/dotnet/api/system.globalization.numberformatinfo.numbernegativepattern
        /// </param>
        /// <param name="numberGroupSizes">
        /// Sizes of decimal groups which are separated by the numberGroupSeparator.
        /// https://docs.microsoft.com/dotnet/api/system.globalization.numberformatinfo.numbergroupsizes
        /// </param>
        public void SetCulture(
            string shortDatePattern = null,
            string decimalSeparator = null,
            string numberGroupSeparator = null,
            int? decimalDigits = null,
            int? numberNegativePattern = null,
            int[] numberGroupSizes = null)
        {

            // settings.culture may be null if the thread culture is the same is the system culture.
            // If it is null, assigning it to a clone of the current culture solves this and also makes it mutable.
            if (settings.culture is null)
                settings.culture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();

            if (shortDatePattern != null)
                settings.culture.DateTimeFormat.ShortDatePattern = shortDatePattern;

            if (decimalDigits != null)
                settings.culture.NumberFormat.NumberDecimalDigits = decimalDigits.Value;

            if (decimalSeparator != null)
                settings.culture.NumberFormat.NumberDecimalSeparator = decimalSeparator;

            if (numberGroupSeparator != null)
                settings.culture.NumberFormat.NumberGroupSeparator = numberGroupSeparator;

            if (numberGroupSizes != null)
                settings.culture.NumberFormat.NumberGroupSizes = numberGroupSizes;

            if (numberNegativePattern != null)
                settings.culture.NumberFormat.NumberNegativePattern = numberNegativePattern.Value;
        }
    }
}
