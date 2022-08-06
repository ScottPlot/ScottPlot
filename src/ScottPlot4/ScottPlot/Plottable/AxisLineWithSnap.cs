using System;

namespace ScottPlot.Plottable
{
    public abstract class AxisLineWithSnap : AxisLine
    {
        /// <summary>
        /// Location of the axis positions to snap to (Y position if horizontal line, X position if vertical line)
        /// </summary>
        protected double[] Positions;

        public int PointCount => Positions.Length;

        public int CurrentIndex { get; set; } = 0;

        protected AxisLineWithSnap(bool isHorizontal) : base(isHorizontal) { }

        public override void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled)
                return;

            var distancessq = new double[PointCount];
            if (IsHorizontal)
            {
                if (coordinateY < DragLimitMin) coordinateY = DragLimitMin;
                if (coordinateY > DragLimitMax) coordinateY = DragLimitMax;

                for (int i = 0; i < PointCount; i++)
                    distancessq[i] = Math.Abs(Positions[i] - coordinateY);
            }
            else
            {
                if (coordinateX < DragLimitMin) coordinateX = DragLimitMin;
                if (coordinateX > DragLimitMax) coordinateX = DragLimitMax;

                for (int i = 0; i < PointCount; i++)
                    distancessq[i] = Math.Abs(Positions[i] - coordinateX);
            }

            for (int i = 0; i < PointCount; i++)
                if (distancessq[i] < distancessq[CurrentIndex])
                    CurrentIndex = i;

            Position = Positions[CurrentIndex];

            OnDragged();
        }
    }
}
