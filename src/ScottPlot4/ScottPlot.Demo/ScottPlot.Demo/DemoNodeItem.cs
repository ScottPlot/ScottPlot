using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public class DemoNodeItem
    {
        public string Header { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }
        public string Tag { get; set; }
        public List<DemoNodeItem> Items { get; set; }
    }
}
