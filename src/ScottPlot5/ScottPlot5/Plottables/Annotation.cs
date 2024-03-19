using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables
{
    public class Annotation : IPlottable
    {
        public bool IsVisible { get; set; } = true;

        public IAxes Axes { get; set; } = new Axes();

        public IEnumerable<LegendItem> LegendItems => LegendItem.None;

        public Label Label { get; set; } = new();

        public Alignment Alignment { get; set; } = Alignment.UpperLeft;

        public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

        public void Render(RenderPack rp)
        {
            if (!IsVisible)
                return;

            //!TODO replace margin with PaddingX/Y
            float margin = 0;
            PixelSize textSize = Label.Measure();

            float _verticalOrigin = Alignment switch
            {
                Alignment.UpperLeft => rp.DataRect.Top + 0.5f * textSize.Height + margin,
                Alignment.UpperCenter => rp.DataRect.Top + 0.5f * textSize.Height + margin,
                Alignment.UpperRight => rp.DataRect.Top + 0.5f * textSize.Height + margin,
                Alignment.MiddleLeft => rp.DataRect.LeftCenter.Y - 0.5f * textSize.Height + margin,
                Alignment.MiddleCenter => rp.DataRect.LeftCenter.Y - 0.5f * textSize.Height + margin,
                Alignment.MiddleRight => rp.DataRect.LeftCenter.Y - 0.5f * textSize.Height + margin,
                Alignment.LowerLeft => rp.DataRect.Bottom - textSize.Height - 4 + margin,
                Alignment.LowerCenter => rp.DataRect.Bottom - textSize.Height - 4 + margin,
                Alignment.LowerRight => rp.DataRect.Bottom - textSize.Height - 4 + margin,
                _ => throw new NotImplementedException()
            };

            float _horizontalOrigin = Alignment switch
            {
                Alignment.UpperLeft => rp.DataRect.Left + 4 + margin,
                Alignment.UpperCenter => rp.DataRect.TopCenter.X - 0.5f * textSize.Width + margin,
                Alignment.UpperRight => rp.DataRect.Right - textSize.Width - 4 + margin,
                Alignment.MiddleLeft => rp.DataRect.Left + 4 + margin,
                Alignment.MiddleCenter => rp.DataRect.BottomCenter.X - 0.5f * textSize.Width + margin,
                Alignment.MiddleRight => rp.DataRect.Right - textSize.Width - 4 + margin,
                Alignment.LowerLeft => rp.DataRect.Left + 4 + margin,
                Alignment.LowerCenter => rp.DataRect.BottomCenter.X - 0.5f * textSize.Width + margin,
                Alignment.LowerRight => rp.DataRect.Right - textSize.Width - 4 + margin,
                _ => throw new NotImplementedException()
            };

            using SKPaint paint = new();
            Label.Render(
                canvas: rp.Canvas,
                x: _horizontalOrigin,
                y: _verticalOrigin,
                paint: paint);
        }
    }
}
