using System;

namespace ScottPlot.Control
{
    /// <summary>
    /// This class detects and stores display scale.
    /// The scale ratio stored here is used to translate mouse coordinates to real pixel coordinates 
    /// in user controls on systems that use display scaling.
    /// </summary>
    public class DisplayScale
    {
        private float _scaleRatio;

        /// <summary>
        /// Current display scale ratio. 
        /// 100% scaling (ratio of 1.0) means no scaling.
        /// </summary>
        public float ScaleRatio
        {
            get
            {
                return _scaleRatio;
            }
            private set
            {
                _scaleRatio = value;
                ScaleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// This event is invoked whenever the display scale is changed.
        /// </summary>
        public event EventHandler ScaleChanged = delegate { };

        public DisplayScale()
        {
            Enable();
        }

        /// <summary>
        /// Enable display scaling using the measured scale ratio.
        /// Call this method if you suspect display scaling has changed.
        /// </summary>
        public void Enable()
        {
            ScaleRatio = MeasureScaleRatio();
        }

        /// <summary>
        /// Enable display scaling using a defined scale ratio (e.g., 150% scaling is ratio 1.5)
        /// </summary>
        public void Enable(double ratio)
        {
            ScaleRatio = (float)ratio;
        }

        /// <summary>
        /// Disable display scaling (use 100% scaling regardless of display setting)
        /// </summary>
        public void Disable()
        {
            ScaleRatio = 1;
        }

        /// <summary>
        /// Return the display scale ratio being used.
        /// A scaling ratio of 1.0 means scaling is not active.
        /// </summary>
        private static float MeasureScaleRatio()
        {
            const int DEFAULT_DPI = 96;
            using System.Drawing.Bitmap bmp = new(1, 1);
            using System.Drawing.Graphics gfx = System.Drawing.Graphics.FromImage(bmp);
            return gfx.DpiX / DEFAULT_DPI;
        }
    }
}
