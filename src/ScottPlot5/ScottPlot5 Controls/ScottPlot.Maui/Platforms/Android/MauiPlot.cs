using View = Android.Views.View;
using MauiView = ScottPlot.Maui.MauiPlot;
using Android.Content;
using MotionEventActions = Android.Views.MotionEventActions;
using Android.Views;
using Android.Print;

namespace ScottPlot.Maui.Android;
internal class MauiPlot : View
{
    private readonly MauiView mauiView;
    public MauiPlot(Context context, MauiView mauiView) : base(context)
    {
        this.mauiView = mauiView;
        //this.Touch += MauiPlot_Touch;
    }
}
