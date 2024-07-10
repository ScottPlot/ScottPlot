using Microsoft.Maui.Handlers;
#if IOS || MACCATALYST
using PlatformView = ScottPlot.Maui.Apple.MauiPlot;
#elif ANDROID
using PlatformView = ScottPlot.Maui.Android.MauiPlot;
#elif WINDOWS
using PlatformView = ScottPlot.Maui.Windows.MauiPlot;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID)
using PlatformView = System.Object;
#endif
namespace ScottPlot.Maui
{
    internal class MauiPlotHandler : ContentViewHandler
    {
        public static IPropertyMapper<MauiPlot, MauiPlotHandler> PropertyMapper = new PropertyMapper<MauiPlot, MauiPlotHandler>(ViewMapper)
        {
        };
        public static CommandMapper<MauiPlot, MauiPlotHandler> CommandMapper = new(ViewCommandMapper)
        {
        };
        public MauiPlotHandler() : base(PropertyMapper, CommandMapper)
        {
        }
    }
}
