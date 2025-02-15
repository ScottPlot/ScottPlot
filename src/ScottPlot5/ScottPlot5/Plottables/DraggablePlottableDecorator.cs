namespace ScottPlot.Plottables
{
    public class DraggablePlottableDecorator : HittablePlottableDecorator
    {
        private Coordinates DragFrom;
        private Coordinates BeforeDragOffset;
        public DraggablePlottableDecorator(IPlottable plottable) : base(plottable)
        {
            try
            {
                _ = GetOffset();
            }
            catch (Exception)
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
            return Source switch
            {
                Signal s => new Coordinates(s.Data.XOffset, s.Data.YOffset),
                SignalXY sxy => new Coordinates(sxy.Data.XOffset, sxy.Data.YOffset),
                Arrow arrow => new Coordinates(arrow.Base.X, arrow.Base.Y),
                DataLogger dataLogger => new Coordinates(dataLogger.Data.XOffset, dataLogger.Data.YOffset),
                DataStreamer dataStreamer => new Coordinates(dataStreamer.Data.OffsetX, dataStreamer.Data.OffsetY),
                Ellipse ellipse => new Coordinates(ellipse.Center.X, ellipse.Center.Y),
                ImageMarker imageMarker => new Coordinates(imageMarker.Location.X, imageMarker.Location.Y),
                Marker marker => new Coordinates(marker.X, marker.Y),
                Scatter scatter => new Coordinates(scatter.OffsetX, scatter.OffsetY),
                Text text => text.Location,
                _ => throw new Exception($"Unsupported Source plotttable {Source}")
            };
        }

        private void SetOffset(double offsetX, double offsetY)
        {
            switch (Source)
            {
                case Signal s:
                    s.Data.XOffset = offsetX;
                    s.Data.YOffset = offsetY;
                    return;
                case SignalXY sxy:
                    sxy.Data.XOffset = offsetX;
                    sxy.Data.YOffset = offsetY;
                    return;
                case Arrow arrow:
                    double tipDeltaX = arrow.Tip.X - arrow.Base.X;
                    double tipDeltaY = arrow.Tip.Y - arrow.Base.Y;
                    arrow.Base = new Coordinates(offsetX, offsetY);
                    arrow.Tip = new Coordinates(offsetX + tipDeltaX, offsetY + tipDeltaY);
                    return;
                case DataLogger dataLogger:
                    dataLogger.Data.XOffset = offsetX;
                    dataLogger.Data.YOffset = offsetY;
                    return;
                case DataStreamer dataStreamer:
                    dataStreamer.Data.OffsetX = offsetX;
                    dataStreamer.Data.OffsetY = offsetY;
                    return;
                case Ellipse ellipse:
                    ellipse.Center.X = offsetX;
                    ellipse.Center.Y = offsetY;
                    return;
                case ImageMarker imageMarker:
                    imageMarker.Location = new Coordinates(offsetX, offsetY);
                    return;
                case Marker marker:
                    marker.X = offsetX;
                    marker.Y = offsetY;
                    return;
                case Scatter scatter:
                    scatter.OffsetX = offsetX;
                    scatter.OffsetY = offsetY;
                    return;
                case Text text:
                    text.Location = new Coordinates(offsetX, offsetY);
                    return;
                default:
                    throw new Exception($"Unsupported Source plotttable {Source}");
            }
        }
    }
}
