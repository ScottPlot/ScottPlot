using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables.Interactive
{
    public class InteractivePolygon : Polygon, IHasInteractiveHandles
    {
        private Coordinates DragStartPoint;
        private Coordinates[]? DragBaseVertices;

        public InteractivePolygon(Coordinates[] coords) : base(coords)
        {
        }

        enum Handles { Body }

        public InteractiveHandle? GetHandle(CoordinateRect rect)
        {
            for (int i = 0; i < Coordinates.Length; i++)
            {
                var coord = Coordinates[i];
                if (rect.Contains(coord))
                {
                    return new InteractiveHandle(this, Cursor.Hand, i + 1);
                }
            }

            if (IsPointInPolygon(rect.Center))
            {
                return new InteractiveHandle(this, Cursor.SizeAll);
            }

            return null;
        }

        public void MoveHandle(InteractiveHandle handle, Coordinates point)
        {
            if (handle.Index == (int)Handles.Body && DragBaseVertices != null)
            {
                var deltaX = point.X - DragStartPoint.X;
                var deltaY = point.Y - DragStartPoint.Y;

                for (int i = 0; i < Coordinates.Length; i++)
                {
                    Coordinates[i].X = DragBaseVertices[i].X + deltaX;
                    Coordinates[i].Y = DragBaseVertices[i].Y + deltaY;
                }
            }
            else
            {
                var vertexIndex = handle.Index - 1;
                Coordinates[vertexIndex].X = point.X;
                Coordinates[vertexIndex].Y = point.Y;
            }
        }

        public void PressHandle(InteractiveHandle handle, Coordinates point)
        {
            if (handle.Index == (int)Handles.Body)
            {
                DragStartPoint = point;
                DragBaseVertices = Coordinates.ToArray(); // deep clone
            }
        }

        public void ReleaseHandle(InteractiveHandle handle)
        {
            if (handle.Index == (int)Handles.Body)
            {
                DragBaseVertices = null;
            }
        }

        private bool IsPointInPolygon(Coordinates point)
        {
            if (Coordinates.Length < 3)
            {
                return false;
            }

            var n = Coordinates.Length;
            var x = point.X;
            var y = point.Y;
            var polyX = Coordinates.Select(c => c.X).ToArray();
            var polyY = Coordinates.Select(c => c.Y).ToArray();

            var inside = false;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (polyY[i] > y != polyY[j] > y &&
                    x < (polyX[j] - polyX[i]) * (y - polyY[i]) / (polyY[j] - polyY[i]) + polyX[i])
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
}
