namespace ScottPlot.Plottable
{
    public interface IHasPixelOffset
    {
        /// <summary>
        /// Render the object this number of pixels right of its coordinate location
        /// </summary>
        public float PixelOffsetX { get; set; }

        /// <summary>
        /// Render the object this number of pixels above its coordinate location
        /// </summary>
        public float PixelOffsetY { get; set; }
    }
}
