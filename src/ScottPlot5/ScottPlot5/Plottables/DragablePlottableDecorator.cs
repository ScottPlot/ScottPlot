namespace ScottPlot.Plottables
{
    public class DragablePlottableDecorator : HitablePlottableDecorator
    {
        private Coordinates DragFrom;
        private Coordinates BeforeDragOffset;
        public DragablePlottableDecorator(IPlottable plottable) : base(plottable)
        {
            try
            {
                _ = GetOffset();
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException($"Unsupported plottable to drag: {plottable}");
            }
        }

        public void StartDrag(Coordinates start)
        {
            DragFrom = start;
            BeforeDragOffset = GetOffset();
        }

        public void DragTo(Coordinates to)
        {
            double offsetX = BeforeDragOffset.X + to.X - DragFrom.X;
            double offsetY = BeforeDragOffset.Y + to.Y - DragFrom.Y;
            SetOffset(offsetX, offsetY);
        }

        private Coordinates GetOffset()
        {
            if (Source is Signal s)
            {
                return new Coordinates(s.Data.XOffset, s.Data.YOffset);
            }
            if (Source is SignalXY sxy)
            {
                return new Coordinates(sxy.Data.XOffset, sxy.Data.YOffset);
            }
            if (Source is Arrow arrow)
            {
                return new Coordinates(arrow.Base.X, arrow.Base.Y);
            }
            if (Source is DataLogger dataLogger)
            {
                return new Coordinates(dataLogger.Data.XOffset, dataLogger.Data.YOffset);
            }
            if (Source is DataStreamer dataStreamer)
            {
                return new Coordinates(dataStreamer.Data.OffsetX, dataStreamer.Data.OffsetY);
            }
            if (Source is Ellipse ellipse)
            {
                return new Coordinates(ellipse.Center.X, ellipse.Center.Y);
            }
            if (Source is ImageMarker imageMarker)
            {
                return new Coordinates(imageMarker.Location.X, imageMarker.Location.Y);
            }
            if (Source is Marker marker)
            {
                return new Coordinates(marker.X, marker.Y);
            }
            if (Source is Scatter scatter)
            {
                return new Coordinates(scatter.OffsetX, scatter.OffsetY);
            }
            if (Source is Text text)
            {
                return text.Location;
            }

            throw new Exception($"Unsupported Source plotttable {Source}");
        }

        private void SetOffset(double offsetX, double offsetY)
        {
            if (Source is Signal s)
            {
                s.Data.XOffset = offsetX;
                s.Data.YOffset = offsetY;
                return;
            }
            if (Source is SignalXY sxy)
            {
                sxy.Data.XOffset = offsetX;
                sxy.Data.YOffset = offsetY;
                return;
            }
            if (Source is Arrow arrow)
            {
                arrow.Base = new Coordinates(offsetX, offsetY);
                return;
            }
            if (Source is DataLogger dataLogger)
            {
                dataLogger.Data.XOffset = offsetX;
                dataLogger.Data.YOffset = offsetY;
                return;
            }
            if (Source is DataStreamer dataStreamer)
            {
                dataStreamer.Data.OffsetX = offsetX;
                dataStreamer.Data.OffsetY = offsetY;
                return;
            }
            if (Source is Ellipse ellipse)
            {
                ellipse.Center.X = offsetX;
                ellipse.Center.Y = offsetY;
                return;
            }
            if (Source is ImageMarker imageMarker)
            {
                imageMarker.Location = new Coordinates(offsetX, offsetY);
                return;
            }
            if (Source is Marker marker)
            {
                marker.X = offsetX;
                marker.Y = offsetY;
                return;
            }
            if (Source is Scatter scatter)
            {
                scatter.OffsetX = offsetX;
                scatter.OffsetY = offsetY;
                return;
            }
            if (Source is Text text)
            {
                text.Location = new Coordinates(offsetX, offsetY);
                return;
            }
            throw new Exception($"Unsupported Source plotttable {Source}");
        }
    }
}
