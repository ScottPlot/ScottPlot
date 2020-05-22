using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.plottables
{
	interface IHighlightable
	{
		void HighlightPoint(int index);
		void HighlightPointNearest(double x);
		void HighlightPointNearest(double x, double y);
		void HighlightClear();
	}
}
