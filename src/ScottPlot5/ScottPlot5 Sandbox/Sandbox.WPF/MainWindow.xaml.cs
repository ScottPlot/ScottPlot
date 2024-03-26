using System;
using System.Linq;
using System.Windows;
using ScottPlot;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    private ScottPlot.AxisRules.LockedHorizontal lockedHorizontalRule;
    private ScottPlot.AxisRules.LockedVertical lockedVerticalRuleLeft;
    private ScottPlot.AxisRules.LockedVertical lockedVerticalRuleRight;
    private ScottPlot.AxisRules.LockedCenterX lockedCenterXRule;
    private ScottPlot.AxisRules.LockedCenterY lockedCenterYRuleLeft;
    private ScottPlot.AxisRules.LockedCenterY lockedCenterYRuleRight;
    private ScottPlot.AxisRules.LockedLeft lockedLeftRule;
    private ScottPlot.AxisRules.LockedRight lockedRightRule;
    private ScottPlot.AxisRules.LockedTop lockedTopRuleLeft;
    private ScottPlot.AxisRules.LockedBottom lockedBottomRuleLeft;
    private ScottPlot.AxisRules.LockedTop lockedTopRuleRight;
    private ScottPlot.AxisRules.LockedBottom lockedBottomRuleRight;
    private ScottPlot.AxisRules.SnapToTicksX snapXRule;
    private ScottPlot.AxisRules.SnapToTicksY snapYRuleLeft;
    private ScottPlot.AxisRules.SnapToTicksY snapYRuleRight;

    private bool _isLockHorizontalChecked = false;
    public bool IsLockHorizontalChecked
    {
        get
        {
            return _isLockHorizontalChecked;
        }
        set
        {
            _isLockHorizontalChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedHorizontalRule);
            } else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedHorizontalRule);
            }
        }
    }

    private bool _isLockVerticalLeftChecked = false;
    public bool IsLockVerticalLeftChecked
    {
        get
        {
            return _isLockVerticalLeftChecked;
        }
        set
        {
            _isLockVerticalLeftChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedVerticalRuleLeft);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedVerticalRuleLeft);
            }
        }
    }

    private bool _isLockVerticalRightChecked = false;
    public bool IsLockVerticalRightChecked
    {
        get
        {
            return _isLockVerticalRightChecked;
        }
        set
        {
            _isLockVerticalRightChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedVerticalRuleRight);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedVerticalRuleRight);
            }
        }
    }

    private bool _isLockCenterXChecked = false;
    public bool IsLockCenterXChecked
    {
        get
        {
            return _isLockCenterXChecked;
        }
        set
        {
            _isLockCenterXChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedCenterXRule);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedCenterXRule);
            }
        }
    }

    private bool _isLockCenterLeftChecked = false;
    public bool IsLockCenterLeftChecked
    {
        get
        {
            return _isLockCenterLeftChecked;
        }
        set
        {
            _isLockCenterLeftChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedCenterYRuleLeft);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedCenterYRuleLeft);
            }
        }
    }

    private bool _isLockCenterRightChecked = false;
    public bool IsLockCenterRightChecked
    {
        get
        {
            return _isLockCenterRightChecked;
        }
        set
        {
            _isLockCenterRightChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedCenterYRuleRight);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedCenterYRuleRight);
            }
        }
    }

    private bool _isLockLeftChecked = false;
    public bool IsLockLeftChecked
    {
        get
        {
            return _isLockLeftChecked;
        }
        set
        {
            _isLockLeftChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedLeftRule);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedLeftRule);
            }
        }
    }

    private bool _isLockRightChecked = false;
    public bool IsLockRightChecked
    {
        get
        {
            return _isLockRightChecked;
        }
        set
        {
            _isLockRightChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedRightRule);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedRightRule);
            }
        }
    }

    private bool _isLockTopLeftChecked = false;
    public bool IsLockTopLeftChecked
    {
        get
        {
            return _isLockTopLeftChecked;
        }
        set
        {
            _isLockTopLeftChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedTopRuleLeft);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedTopRuleLeft);
            }
        }
    }

    private bool _isLockBottomLeftChecked = false;
    public bool IsLockBottomLeftChecked
    {
        get
        {
            return _isLockBottomLeftChecked;
        }
        set
        {
            _isLockBottomLeftChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedBottomRuleLeft);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedBottomRuleLeft);
            }
        }
    }

    private bool _isLockTopRightChecked = false;
    public bool IsLockTopRightChecked
    {
        get
        {
            return _isLockTopRightChecked;
        }
        set
        {
            _isLockTopRightChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedTopRuleRight);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedTopRuleRight);
            }
        }
    }

    private bool _isLockBottomRightChecked = false;
    public bool IsLockBottomRightChecked
    {
        get
        {
            return _isLockBottomRightChecked;
        }
        set
        {
            _isLockBottomRightChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(lockedBottomRuleRight);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(lockedBottomRuleRight);
            }
        }
    }

    private bool _isSnapTicksXChecked = false;
    public bool IsSnapTicksXChecked
    {
        get
        {
            return _isSnapTicksXChecked;
        }
        set
        {
            _isSnapTicksXChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(snapXRule);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(snapXRule);
            }
        }
    }

    private bool _isSnapTicksYLeftChecked = false;
    public bool IsSnapTicksYLeftChecked
    {
        get
        {
            return _isSnapTicksYLeftChecked;
        }
        set
        {
            _isSnapTicksYLeftChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(snapYRuleLeft);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(snapYRuleLeft);
            }
        }
    }

    private bool _isSnapTicksYRightChecked = false;
    public bool IsSnapTicksYRightChecked
    {
        get
        {
            return _isSnapTicksYRightChecked;
        }
        set
        {
            _isSnapTicksYRightChecked = value;
            if (value)
            {
                WpfPlot1.Plot.Axes.Rules.Add(snapYRuleRight);
            }
            else
            {
                WpfPlot1.Plot.Axes.Rules.Remove(snapYRuleRight);
            }
        }
    }

    private bool _isInvertXChecked = false;
    public bool IsInvertXChecked
    {
        get
        {
            return _isInvertXChecked;
        }
        set
        {
            var oldLimits = WpfPlot1.Plot.Axes.GetLimits(WpfPlot1.Plot.Axes.Bottom, WpfPlot1.Plot.Axes.Left);
            if (_isInvertXChecked != value)
            {
                WpfPlot1.Plot.Axes.SetLimitsX(left: oldLimits.Right, right: oldLimits.Left, WpfPlot1.Plot.Axes.Bottom);
            }
            _isInvertXChecked = value;
            WpfPlot1.Refresh();
        }
    }

    private bool _isInvertYLeftChecked = false;
    public bool IsInvertYLeftChecked
    {
        get
        {
            return _isInvertYLeftChecked;
        }
        set
        {
            var oldLimits = WpfPlot1.Plot.Axes.GetLimits(WpfPlot1.Plot.Axes.Bottom,WpfPlot1.Plot.Axes.Left);
            if (_isInvertYLeftChecked != value)
            {
                WpfPlot1.Plot.Axes.SetLimitsY(bottom: oldLimits.Top,top: oldLimits.Bottom, WpfPlot1.Plot.Axes.Left);
            }
            _isInvertYLeftChecked = value;
            WpfPlot1.Refresh();
        }
    }

    private bool _isInvertYRightChecked = false;
    public bool IsInvertYRightChecked
    {
        get
        {
            return _isInvertYRightChecked;
        }
        set
        {
            var oldLimits = WpfPlot1.Plot.Axes.GetLimits(WpfPlot1.Plot.Axes.Bottom, WpfPlot1.Plot.Axes.Right);
            if (_isInvertYRightChecked != value)
            {
                WpfPlot1.Plot.Axes.SetLimitsY(bottom: oldLimits.Top, top: oldLimits.Bottom, WpfPlot1.Plot.Axes.Right);
            }
            _isInvertYRightChecked = value;
            WpfPlot1.Refresh();
        }
    }

    public MainWindow()
    {
        InitializeComponent();
        lockedHorizontalRule = new(xAxis: WpfPlot1.Plot.Axes.Bottom);
        lockedVerticalRuleLeft = new(yAxis: WpfPlot1.Plot.Axes.Left);
        lockedVerticalRuleRight = new(yAxis: WpfPlot1.Plot.Axes.Right);
        lockedCenterXRule = new(xAxis: WpfPlot1.Plot.Axes.Bottom);
        lockedCenterYRuleLeft = new(yAxis: WpfPlot1.Plot.Axes.Left);
        lockedCenterYRuleRight = new(yAxis: WpfPlot1.Plot.Axes.Right);
        lockedLeftRule = new(xAxis: WpfPlot1.Plot.Axes.Bottom);
        lockedRightRule = new(xAxis: WpfPlot1.Plot.Axes.Bottom);
        lockedTopRuleLeft = new(yAxis: WpfPlot1.Plot.Axes.Left);
        lockedBottomRuleLeft = new(yAxis: WpfPlot1.Plot.Axes.Left);
        lockedTopRuleRight = new(yAxis: WpfPlot1.Plot.Axes.Right);
        lockedBottomRuleRight = new(yAxis: WpfPlot1.Plot.Axes.Right);
        snapXRule = new(xAxis: WpfPlot1.Plot.Axes.Bottom);
        snapYRuleLeft = new(yAxis: WpfPlot1.Plot.Axes.Left);
        snapYRuleRight = new(yAxis: WpfPlot1.Plot.Axes.Right);

        WpfPlot1.Plot.Axes.SetLimits(left:-1,right:1,top:-1,bottom:1);

        var sig1 = WpfPlot1.Plot.Add.Signal(Generate.Sin(51));
        var sig2 = WpfPlot1.Plot.Add.Signal(Generate.Cos(51).Select(x => x *0.5).ToArray());
        // tell each signal plot to use a different axis
        sig1.Axes.YAxis = WpfPlot1.Plot.Axes.Left;
        sig2.Axes.YAxis = WpfPlot1.Plot.Axes.Right;
        WpfPlot1.Plot.Axes.AutoScale();

    }

}
