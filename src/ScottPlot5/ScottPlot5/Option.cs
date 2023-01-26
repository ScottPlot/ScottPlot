using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{

    internal struct Option<T1, T2>
    { 
        public int ActiveField = 0;

        private T1 _first;
        private T2 _second;

        public T1 First => ActiveField == 0 ? _first : throw new InvalidOperationException("Option is not active");
        public T2 Second => ActiveField == 1 ? _second : throw new InvalidOperationException("Option is not active");

        public Option(T1 first)
        {
            ActiveField = 0;
            _first = first;
        }

        public Option(T2 second)
        {
            ActiveField = 1;
            _second = second;
        }
    }
}
