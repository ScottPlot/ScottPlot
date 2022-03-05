using NUnit.Framework;
using ScottPlot.Drawing;
using System;
using System.Linq;

namespace ScottPlotTests.Colormap
{
    [TestFixture]
    public class ColormapFactoryTests
    {
        [TestCase("Solar")]
        [TestCase("Amp")]
        [TestCase("Balance")]
        [TestCase("Ice")]
        public void Create_AvailableName_NotThrows(string Name)
        {
            var factory = new ColormapFactory();
            ScottPlot.Drawing.Colormap result = factory.CreateOrDefault(Name);
        }

        [TestCase("Solar")]
        [TestCase("Amp")]
        [TestCase("Balance")]
        [TestCase("Ice")]
        public void Create_AvailableName_ColormapNameMatchRequested(string Name)
        {
            var factory = new ColormapFactory();
            ScottPlot.Drawing.Colormap result = factory.CreateOrDefault(Name);
            Assert.AreEqual(Name, result.Name);
        }

        [TestCase("NotExistedColormap")]
        [TestCase("a12")]
        [TestCase("BestColormapEver")]
        public void Create_NotAvailableName_ReturnDefaultAlgae(string Name)
        {
            var factory = new ColormapFactory();
            ScottPlot.Drawing.Colormap result = factory.CreateOrDefault(Name);
            Assert.AreEqual(factory.GetDefaultColormap().GetType().Name, result.Name);
        }

        [TestCase("Solar")]
        [TestCase("Amp")]
        [TestCase("Balance")]
        [TestCase("Ice")]
        public void CreateUnsafe_AvailableName_ColormapNameMatchrequested(string Name)
        {
            var factory = new ColormapFactory();
            ScottPlot.Drawing.Colormap result = factory.CreateOrThrow(Name);
            Assert.AreEqual(Name, result.Name);
        }

        [TestCase("NotExistedColormap")]
        [TestCase("a12")]
        [TestCase("BestColormapEver")]
        public void CreateUnsafe_NotAvailableName_Throws(string Name)
        {
            var factory = new ColormapFactory();
            Assert.Throws<ArgumentOutOfRangeException>(() =>
           {
               ScottPlot.Drawing.Colormap result = factory.CreateOrThrow(Name);
           });
        }

        [TestCase("Solar")]
        [TestCase("Amp")]
        [TestCase("Balance")]
        [TestCase("Ice")]
        public void GetAvailableNames_ContainAvailableName(string Name)
        {
            var factory = new ColormapFactory();
            var availableNames = factory.GetAvailableNames();
            CollectionAssert.Contains(availableNames, Name);
        }

        [TestCase("NotExistedColormap")]
        [TestCase("a12")]
        [TestCase("BestColormapEver")]
        public void GetAvailableNames_NotContainUnavailableName(string Name)
        {
            var factory = new ColormapFactory();
            var availableNames = factory.GetAvailableNames();
            CollectionAssert.DoesNotContain(availableNames, Name);
        }

        [TestCase("Solar")]
        [TestCase("Amp")]
        [TestCase("Balance")]
        [TestCase("Ice")]
        public void GetAvailableColormaps_ContainAvailableName(string Name)
        {
            var factory = new ColormapFactory();
            var availableColormaps = factory.GetAvailableColormaps();
            CollectionAssert.Contains(availableColormaps.Select(cm => cm.Name), Name);
        }

        [TestCase("NotExistedColormap")]
        [TestCase("a12")]
        [TestCase("BestColormapEver")]
        public void GetAvailableColormaps_NotContainUnavailableName(string Name)
        {
            var factory = new ColormapFactory();
            var availableColormaps = factory.GetAvailableColormaps();
            CollectionAssert.DoesNotContain(availableColormaps.Select(cm => cm.Name), Name);
        }

        [Test]
        public void GetAvailableColormaps_KeysMustMatchNames()
        {
            var factory = new ColormapFactory();
            foreach (string keyName in factory.GetAvailableNames())
            {
                var cmap = factory.CreateOrThrow(keyName);
                Assert.AreEqual(keyName, cmap.Name);
            }
        }
    }
}
