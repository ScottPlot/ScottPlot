"""
This script analyzes source code and outputs an array of line lengths.
The value at an index is the number of lines whose length is that index.
"""

import glob
import os
import sys

PATH_HERE = os.path.dirname(__file__)
PATH_SRC = os.path.abspath(PATH_HERE+"/../../../src")

def analyzeLineLength(filePath, minLength=3):
    with open(filePath) as f:
        lines = f.read().split("\n")
    lengthsByLine = [len(x.strip()) for x in lines]
    lengthsByLine = [x for x in lengthsByLine if x >= minLength]
    lengthCounts = [0 for x in range(max(lengthsByLine)+1)]
    for length in lengthsByLine:
        lengthCounts[length] += 1
    print(os.path.basename(filePath), lengthCounts);
    
if __name__=="__main__":
    analyzeLineLength(PATH_SRC+"/ScottPlot/Plot.cs")
    analyzeLineLength(PATH_SRC+"/ScottPlot.WinForms/FormsPlot.cs")
    analyzeLineLength(PATH_SRC+"/ScottPlot.WPF/WpfPlot.xaml.cs")
