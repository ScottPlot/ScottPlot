using System;

namespace ScottPlot.Control
{
    /// <summary>
    /// This class detects and stores display scale.
    /// The scale ratio is used to calculate plot size and
    /// translate mouse coordinates to real pixel coordinates.
    /// </summary>
    public class DisplayScale
    {
        /// <summary>
        /// Scale ratio in use by the active display.
        /// This ratio is used when scaling is enabled.
        /// </summary>
        public float SystemScaleRatio { get; private set; } = 1.0f;

        /// <summary>
        /// Scale ratio to use if scaling is disabled.
        /// </summary>
        public float ManualScaleRatio { get; private set; } = 1.0f;

        private bool _enabled = true;

        /// <summary>
        /// Control whether the plot bitmap should be stretched if display scaling is active.
        /// When enabled text will be large but may be blurry.
        /// When disabled text will be sharp but may be too small to read on high-resolution displays.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    ScaleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Current display scale ratio. 
        /// </summary>
        public float ScaleRatio
        {
            get => Enabled ? SystemScaleRatio : ManualScaleRatio;
        }

        /// <summary>
        /// This event is invoked whenever the display scale is changed.
        /// </summary>
        public event EventHandler ScaleChanged = delegate { };

        public DisplayScale()
        {
            Measure();
        }

        /// <summary>
        /// Update the scale ratio using that of the active display.
        /// Call this method if you expect the display scale has changed.
        /// </summary>
        public void Measure()
        {
            double ratio = Drawing.GDI.GetScaleRatio();
            if (SystemScaleRatio != ratio)
            {
                SystemScaleRatio = (float)ratio;
                ScaleChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
