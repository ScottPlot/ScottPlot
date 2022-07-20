# ScottPlot: The 'Common' Project

Code in this folder serves multiple purposes:

* It can be compiled as a .NET Standard library and tested with the adjacent NUnit test suite

* Source files can be used in both ScottPlot4 and ScottPlot5, preventing duplication and re-writing logic.
  * ScottPlot5 imports all files (`**/*.cs`)
  * ScottPlot4 tacically imports individual files (so conflicting types are not imported)