using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot
{
    public class PlottableBoxAndWhisker : Plottable
    {
        public Statistics.BoxAndWhisker[] boxAndWhiskers;

        public PlottableBoxAndWhisker(Statistics.BoxAndWhisker[] boxAndWhiskers)
        {
            this.boxAndWhiskers = boxAndWhiskers;
        }

        public override AxisLimits2D GetLimits()
        {
            var limits = new AxisLimits2D();
            foreach (var box in boxAndWhiskers)
            {
                limits.ExpandX(box.xPosition - box.box.width / 2, box.xPosition + box.box.width / 2);
                limits.ExpandX(box.xPosition - box.whisker.width / 2, box.xPosition + box.whisker.width / 2);
                limits.ExpandX(box.xPosition - box.midline.width / 2, box.xPosition + box.midline.width / 2);
                limits.ExpandY(box.box.min, box.box.max);
                limits.ExpandY(box.whisker.min, box.whisker.max);
                limits.ExpandY(box.midline.position, box.midline.position);
            }
            return limits;
        }

        public override void Render(Settings settings)
        {
            foreach (Statistics.BoxAndWhisker baw in boxAndWhiskers)
            {
                float center = (float)settings.GetPixelX(baw.xPosition);

                if (baw.whisker.visible)
                {
                    float whiskerMax = (float)settings.GetPixelY(baw.whisker.max);
                    float whiskerMin = (float)settings.GetPixelY(baw.whisker.min);
                    float whiskerLeft = (float)settings.GetPixelX(baw.xPosition - baw.whisker.width / 2);
                    float whiskerRight = (float)settings.GetPixelX(baw.xPosition + baw.whisker.width / 2);

                    var whiskerPen = new Pen(baw.whisker.lineColor, baw.whisker.lineWidth);
                    settings.gfxData.DrawLine(whiskerPen, center, whiskerMin, center, whiskerMax);
                    settings.gfxData.DrawLine(whiskerPen, whiskerLeft, whiskerMax, whiskerRight, whiskerMax);
                    settings.gfxData.DrawLine(whiskerPen, whiskerLeft, whiskerMin, whiskerRight, whiskerMin);
                }

                if (baw.box.outline || baw.box.fill)
                {

                    float boxMax = (float)settings.GetPixelY(baw.box.max);
                    float boxMin = (float)settings.GetPixelY(baw.box.min);
                    float boxLeft = (float)settings.GetPixelX(baw.xPosition - baw.box.width / 2);
                    float boxRight = (float)settings.GetPixelX(baw.xPosition + baw.box.width / 2);

                    float boxHeight = boxMin - boxMax;
                    float boxWidth = boxRight - boxLeft;

                    PointF boxOrigin = new PointF(boxLeft, boxMax);
                    SizeF boxSize = new SizeF(boxWidth, boxHeight);
                    RectangleF boxRect = new RectangleF(boxOrigin, boxSize);


                    if (baw.box.fill)
                    {
                        var boxBrush = new SolidBrush(baw.box.fillColor);
                        settings.gfxData.FillRectangle(boxBrush, Rectangle.Round(boxRect));
                    }

                    if (baw.box.outline)
                    {
                        var boxPen = new Pen(baw.box.lineColor, baw.box.lineWidth);
                        settings.gfxData.DrawRectangle(boxPen, Rectangle.Round(boxRect));
                    }
                }

                if (baw.midline.visible)
                {
                    float boxLeft = (float)settings.GetPixelX(baw.xPosition - baw.box.width / 2);
                    float boxRight = (float)settings.GetPixelX(baw.xPosition + baw.box.width / 2);
                    float midlineY = (float)settings.GetPixelY(baw.midline.position);

                    var mindlinePen = new Pen(baw.midline.lineColor, baw.midline.lineWidth);
                    settings.gfxData.DrawLine(mindlinePen, boxLeft, midlineY, boxRight, midlineY);
                }
            }
        }

        public override string ToString()
        {
            return $"PlottableBox with {boxAndWhiskers.Length} boxes";
        }
    }
}
