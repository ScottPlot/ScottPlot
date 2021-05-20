namespace ScottPlot
{
    /// <summary>
    /// Describes how a render should be performed with respect to quality.
    /// High quality enables anti-aliasing but is slower.
    /// Some options describe multiple renders, with or without a delay between them.
    /// </summary>
    public enum RenderType

    {
        /// <summary>
        /// Only render using low quality (anti-aliasing off)
        /// </summary>
        LowQuality,

        /// <summary>
        /// Only render using high quality (anti-aliasing on)
        /// </summary>
        HighQuality,

        /// <summary>
        /// Perform a high quality render after a delay.
        /// This is the best render type to use when resizing windows.
        /// </summary>
        HighQualityDelayed,

        /// <summary>
        /// Render low quality and display it, then if no new render requests
        /// have been received immediately render a high quality version and display it.
        /// This is the best render option to use when requesting renders programmatically
        /// </summary>
        LowQualityThenHighQuality,

        /// <summary>
        /// Render low quality and display it, wait a small period of time for new render requests to arrive,
        /// and if no new requests have been received render a high quality version and display it.
        /// This is the best render option to use for mouse interaction.
        /// </summary>
        LowQualityThenHighQualityDelayed,

        /// <summary>
        /// Process mouse events only (pan, zoom, etc) and do not render graphics on a Bitmap,
        /// then if no new requests have been received render using the last-used render type.
        /// </summary>
        ProcessMouseEventsOnly,
    }
}
