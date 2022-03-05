namespace ScottPlot
{
    public enum RadialGaugeMode
    {
        /// <summary>
        /// Successive gauges start outward from the center but start at the same angle
        /// </summary>
        Stacked,

        /// <summary>
        /// Successive gauges start outward from the center and start at sequential angles
        /// </summary>
        Sequential,

        /// <summary>
        /// Gauges are all the same distance from the center but start at sequential angles
        /// </summary>
        SingleGauge
    }
}
