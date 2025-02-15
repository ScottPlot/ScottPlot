using SkiaSharp.Views.Maui.Controls.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Maui
{
    public static class Register
    {
        public static MauiAppBuilder UseScottPlot(this MauiAppBuilder builder)
        {
            builder.UseSkiaSharp();
            return builder;
        }
    }
}
