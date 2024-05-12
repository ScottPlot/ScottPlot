using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScottPlot;
using ScottPlot.AxisRules;

namespace Sandbox.WPF;

public partial class MainWindow : Window, INotifyPropertyChanged
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

    private AxisLimits CurrentLimitsLeft
    {
        get => WpfPlot1.Plot.Axes.GetLimits(xAxis: WpfPlot1.Plot.Axes.Bottom, yAxis: WpfPlot1.Plot.Axes.Left);
    }
    private AxisLimits CurrentLimitsRight
    {
        get => WpfPlot1.Plot.Axes.GetLimits(xAxis: WpfPlot1.Plot.Axes.Bottom, yAxis: WpfPlot1.Plot.Axes.Right);
    }

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
            OnPropertyChanged(nameof(IsLockHorizontalChecked));
            if (value)
            {
                lockedHorizontalRule = new LockedHorizontal(xAxis: WpfPlot1.Plot.Axes.Bottom, CurrentLimitsLeft.Left, CurrentLimitsLeft.Right);
                WpfPlot1.Plot.Axes.Rules.Add(lockedHorizontalRule);
            }
            else
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
            OnPropertyChanged(nameof(IsLockVerticalLeftChecked));
            if (value)
            {
                lockedVerticalRuleLeft = new LockedVertical(yAxis: WpfPlot1.Plot.Axes.Left, CurrentLimitsLeft.Bottom, CurrentLimitsLeft.Top);
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
            OnPropertyChanged(nameof(IsLockVerticalRightChecked));
            if (value)
            {
                lockedVerticalRuleRight = new LockedVertical(yAxis: WpfPlot1.Plot.Axes.Left, CurrentLimitsRight.Bottom, CurrentLimitsRight.Top);
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
            OnPropertyChanged(nameof(IsLockCenterXChecked));
            if (value)
            {
                lockedCenterXRule = new LockedCenterX(xAxis: WpfPlot1.Plot.Axes.Bottom, CurrentLimitsLeft.Left/2 + CurrentLimitsLeft.Right/2);
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
            OnPropertyChanged(nameof(IsLockCenterLeftChecked));
            if (value)
            {
                lockedCenterYRuleLeft = new LockedCenterY(yAxis: WpfPlot1.Plot.Axes.Left, CurrentLimitsLeft.Bottom / 2 + CurrentLimitsLeft.Top / 2);
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
            OnPropertyChanged(nameof(IsLockCenterRightChecked));
            if (value)
            {
                lockedCenterYRuleRight = new LockedCenterY(yAxis: WpfPlot1.Plot.Axes.Right, CurrentLimitsRight.Bottom / 2 + CurrentLimitsRight.Top / 2);
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
            OnPropertyChanged(nameof(IsLockLeftChecked));
            if (value)
            {
                lockedLeftRule = new LockedLeft(xAxis: WpfPlot1.Plot.Axes.Bottom, CurrentLimitsLeft.Left);
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
            OnPropertyChanged(nameof(IsLockRightChecked));
            if (value)
            {
                lockedRightRule = new LockedRight(xAxis: WpfPlot1.Plot.Axes.Bottom, CurrentLimitsLeft.Right);
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
            OnPropertyChanged(nameof(IsLockTopLeftChecked));
            if (value)
            {
                lockedTopRuleLeft = new LockedTop(yAxis: WpfPlot1.Plot.Axes.Left, CurrentLimitsLeft.Top);
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
            OnPropertyChanged(nameof(IsLockBottomLeftChecked));
            if (value)
            {
                lockedBottomRuleLeft = new LockedBottom(yAxis: WpfPlot1.Plot.Axes.Left, CurrentLimitsLeft.Bottom);
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
            OnPropertyChanged(nameof(IsLockTopRightChecked));
            if (value)
            {
                lockedTopRuleRight = new LockedTop(yAxis: WpfPlot1.Plot.Axes.Right, CurrentLimitsRight.Top);
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
            OnPropertyChanged(nameof(IsLockBottomRightChecked));
            if (value)
            {
                lockedBottomRuleRight = new LockedBottom(yAxis: WpfPlot1.Plot.Axes.Right, CurrentLimitsRight.Bottom);
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
            OnPropertyChanged(nameof(IsSnapTicksXChecked));
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
            OnPropertyChanged(nameof(IsSnapTicksYLeftChecked));
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
            OnPropertyChanged(nameof(IsSnapTicksYRightChecked));
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
                IsSnapTicksXChecked = false;
                IsLockLeftChecked = false;
                IsLockRightChecked = false;
                IsLockCenterXChecked = false;
                IsLockHorizontalChecked = false;
                WpfPlot1.Plot.Axes.SetLimitsX(left: oldLimits.Right, right: oldLimits.Left, WpfPlot1.Plot.Axes.Bottom);
            }
            _isInvertXChecked = value;
            OnPropertyChanged(nameof(IsInvertXChecked));
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
            var oldLimits = WpfPlot1.Plot.Axes.GetLimits(WpfPlot1.Plot.Axes.Bottom, WpfPlot1.Plot.Axes.Left);
            if (_isInvertYLeftChecked != value)
            {
                IsSnapTicksYLeftChecked = false;
                IsLockTopLeftChecked = false;
                IsLockBottomLeftChecked = false;
                IsLockLeftChecked = false;
                IsLockCenterLeftChecked = false;
                WpfPlot1.Plot.Axes.SetLimitsY(bottom: oldLimits.Top, top: oldLimits.Bottom, WpfPlot1.Plot.Axes.Left);
            }
            _isInvertYLeftChecked = value;
            OnPropertyChanged(nameof(IsInvertYLeftChecked));
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
                IsSnapTicksYRightChecked = false;
                IsLockTopRightChecked = false;
                IsLockBottomRightChecked = false;
                IsLockRightChecked = false;
                IsLockCenterRightChecked = false;
                WpfPlot1.Plot.Axes.SetLimitsY(bottom: oldLimits.Top, top: oldLimits.Bottom, WpfPlot1.Plot.Axes.Right);
            }
            _isInvertYRightChecked = value;
            OnPropertyChanged(nameof(IsInvertYRightChecked));
            WpfPlot1.Refresh();
        }
    }

    public MainWindow()
    {
        InitializeComponent();
        snapXRule = new(xAxis: WpfPlot1.Plot.Axes.Bottom);
        snapYRuleLeft = new(yAxis: WpfPlot1.Plot.Axes.Left);
        snapYRuleRight = new(yAxis: WpfPlot1.Plot.Axes.Right);

        WpfPlot1.Plot.Axes.SetLimits(left: -1, right: 1, top: -1, bottom: 1);
        WpfPlot1.Plot.ScaleFactor = WpfPlot1.DisplayScale;
        var sig1 = WpfPlot1.Plot.Add.Signal(Generate.Sin(51));
        var sig2 = WpfPlot1.Plot.Add.Signal(Generate.Cos(51).Select(x => x * 0.5).ToArray());
        // tell each signal plot to use a different axis
        sig1.Axes.YAxis = WpfPlot1.Plot.Axes.Left;
        sig2.Axes.YAxis = WpfPlot1.Plot.Axes.Right;
        WpfPlot1.Plot.Axes.AutoScale();

    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
