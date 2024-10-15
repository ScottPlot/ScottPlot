using Windows.System;
using ScottPlot.Interactivity;

namespace ScottPlot.Maui;

// All the code in this file is only included on Windows.
public class PlatformClass1
//internal static partial class MauiPlotExtensions
{
//    static partial void ProcessKeyModifiersPressed(UserInputProcessor processor, MauiPlot plot, PointerEventArgs e)
//    {
//        var args = e.PlatformArgs?.PointerRoutedEventArgs;
//        if (args is null) return;
//        var keyModifiers = args.KeyModifiers;


//        if (keyModifiers is null) return;

//        bool control = ((uint)keyModifiers & (uint)VirtualKeyModifiers.Control) > 0;
//        bool shift = ((int)keyModifiers & (int)VirtualKeyModifiers.Shift) > 0;
//        bool alt = ((int)keyModifiers & (int)VirtualKeyModifiers.Menu) > 0;


//        IUserAction actionShift = shift
//            ? new Interactivity.UserActions.KeyDown(StandardKeys.Shift)
//            : new Interactivity.UserActions.KeyUp(StandardKeys.Shift);
//        processor.Process(actionShift);

//        IUserAction actionControl = control
//            ? new Interactivity.UserActions.KeyDown(StandardKeys.Control)
//            : new Interactivity.UserActions.KeyUp(StandardKeys.Control);
//        processor.Process(actionControl);

//        IUserAction actionAlt = alt
//            ? new Interactivity.UserActions.KeyDown(StandardKeys.Alt)
//            : new Interactivity.UserActions.KeyUp(StandardKeys.Alt);
//        processor.Process(actionAlt);

//        Trace.WriteLine($"dsa{args}");
//    }
}
