using System;
using ScottPlot.Plottable;

namespace ScottPlot.SnapLogic
{
    public class NearestPosition : ISnap
    {
        private readonly double[] _snapPositions;

        public NearestPosition(double[] snapPositions)
        {
            _snapPositions = snapPositions;
        }

        public double Snap(double value)
        {
            var closestDistance = double.MaxValue;
            var closestPosition = double.MaxValue;

            foreach (var snapPosition in _snapPositions)
            {
                var distance = Math.Abs(value - snapPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPosition = snapPosition;
                }
            }

            return closestPosition;
        }
    }
}
