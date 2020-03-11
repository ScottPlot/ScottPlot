"""
This script analyzes source code and outputs an array of line lengths.
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
    print(os.path.basename(filePath), lengthsByLine);
    
if __name__=="__main__":
    analyzeLineLength(PATH_SRC+"/ScottPlot/Plot.cs")
    analyzeLineLength(PATH_SRC+"/ScottPlot.WinForms/FormsPlot.cs")
    analyzeLineLength(PATH_SRC+"/ScottPlot.WPF/WpfPlot.xaml.cs")
