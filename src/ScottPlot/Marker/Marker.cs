using System;

namespace ScottPlot
{
    public static class Marker
    {
        public static MarkerShape None => MarkerShape.none;
        public static MarkerShape FilledCircle => MarkerShape.filledCircle;
        public static MarkerShape OpenCircle => MarkerShape.openCircle;
        public static MarkerShape FilledSquare => MarkerShape.filledSquare;
        public static MarkerShape OpenSquare => MarkerShape.openSquare;
        public static MarkerShape FilledDiamond => MarkerShape.filledDiamond;
        public static MarkerShape OpenDiamond => MarkerShape.openDiamond;
        public static MarkerShape Asterisk => MarkerShape.asterisk;
        public static MarkerShape HashTag => MarkerShape.hashTag;
        public static MarkerShape Cross => MarkerShape.cross;
        public static MarkerShape Eks => MarkerShape.eks;
        public static MarkerShape VerticalBar => MarkerShape.verticalBar;
        public static MarkerShape TriangleUp => MarkerShape.triUp;
        public static MarkerShape TriangleDown => MarkerShape.triDown;
        public static MarkerShape FilledTriangleUp => MarkerShape.filledTriangleUp;
        public static MarkerShape FilledTriangleDown => MarkerShape.filledTriangleDown;
        public static MarkerShape OpenTriangleUp => MarkerShape.openTriangleUp;
        public static MarkerShape OpenTriangleDown => MarkerShape.openTriangleDown;

        public static MarkerShape Random() => Random(new Random());

        public static MarkerShape Random(Random rand)
        {
            Array members = Enum.GetValues(typeof(MarkerShape));
            object randomMember = members.GetValue(rand.Next(members.Length));
            return (MarkerShape)randomMember;
        }
    }
}
