﻿/* Code here tests what happens if you modify the list of plottables in one thread while
 * simultaneously rendering in another thread.
 * 
 * An InvalidOperationException is usually (but not always) thrown unless you
 * wait for IsRendering to become false before modifying the plot.
 * 
 * See discussion in https://github.com/swharden/ScottPlot/issues/609
 * 
 */
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading;

namespace ScottPlotTests.Concurrency
{
    class ConcurrencyTests
    {
        public static bool ContinueModifyingPlottables = true;
        public static void ContinuouslyModifyPlottables(object data)
        {
            ScottPlot.Plot plt = (ScottPlot.Plot)data;
            while (ContinueModifyingPlottables)
            {
                plt.RenderLock();
                plt.Clear();
                plt.AddSignal(new double[] { 1, 2, 3 });
                plt.RenderUnlock();
            }
            Debug.WriteLine("Modification thread shutting down...");
            Thread.Sleep(100);
        }

        // This test takes 5 seconds to complete so it is not run routinely
        //[Test]
        public void Test_Concurrent_ModifyPlottablesWhileRendering()
        {
            var plt = new ScottPlot.Plot();
            var bmp = new System.Drawing.Bitmap(300, 200);

            // update plottables continuously in the background
            Thread updateThread = new Thread(new ParameterizedThreadStart(ContinuouslyModifyPlottables));
            updateThread.Start(plt);

            // render repeatedly while the plottalbles are being modified
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 5000)
            {
                plt.Render(bmp);
            }

            ContinueModifyingPlottables = false;
            updateThread.Join();

            Console.WriteLine("Test complete");
            Assert.Pass();
        }
    }
}
