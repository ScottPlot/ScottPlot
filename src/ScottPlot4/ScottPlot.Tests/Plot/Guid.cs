using System.Collections.Generic;
using NUnit.Framework;

namespace ScottPlotTests.Plot
{
    class Guid
    {
        [Test]
        public void Test_Guids_AreUnique()
        {
            int reps = 100;
            var guids = new HashSet<string>();

            for (int i = 0; i < reps; i++)
            {
                var plt = new ScottPlot.Plot();
                guids.Add(plt.GetGuid());
            }

            int uniqueGuids = guids.Count;

            Assert.AreEqual(reps, uniqueGuids);
        }
    }
}
