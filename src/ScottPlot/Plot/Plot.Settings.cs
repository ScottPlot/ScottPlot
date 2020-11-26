/* Code here customizes Plot behavior (not styling) */

using ScottPlot.Ticks;
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

        public Settings GetSettings(bool showWarning = true)
        {
            if (showWarning)
                Debug.WriteLine("WARNING: GetSettings() is only for development and testing. " +
                                "Be aware its class structure changes frequently!");

            return settings;
        }

        /// <summary>
        /// Throw an exception of any plottable contains invalid data.
        /// </summary>
        /// <param name="deep">Deep validation is more thorough but slower.</param>
        public void Validate(bool deep = true)
        {
            foreach (var plottable in settings.Plottables)
                plottable.ValidateData(deep);
        }

        [Obsolete("Disable anti-aliasing using the lowQuality argument in Render() or SaveFig()", true)]
        public void AntiAlias(bool figure = true, bool data = false, bool legend = false) { }

        public void SetCulture(System.Globalization.CultureInfo culture)
        {
            foreach (var axis in settings.Axes)
                axis.Ticks.TickCollection.Culture = culture;
        }

        /// <summary>
        /// Updates the culture used for displaying numbers in tick labels
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
            foreach (var axis in settings.Axes)
                axis.Ticks.TickCollection.SetCulture(
                        shortDatePattern, decimalSeparator, numberGroupSeparator,
                        decimalDigits, numberNegativePattern, numberGroupSizes);
        }
    }
}
