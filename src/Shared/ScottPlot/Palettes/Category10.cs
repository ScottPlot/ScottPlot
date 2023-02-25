// These colors were originally developed by Tableau:
//   https://www.tableau.com/about/blog/2016/7/colors-upgrade-tableau-10-56782
//
// Vega obtained permission to release this color set under a BSD license:
//   https://github.com/d3/d3-scale-chromatic/pull/16
//
// Vega placed these color values here under a BSD (3-clause) license:
//   https://github.com/vega/vega/blob/af5cc1df42eb5aaf2f478d0bda69313643fe0532/docs/releases/v1.2.1/vega.js#L170-L205
//
// ScottPlot obtained this list of colors from Vega (not Tableau) and includes them here along with a copy
//   of the Vega's BSD 3-clause license.
// 
// D3 also uses this default color set under a BSD (3-clause) license:
//   https://github.com/d3/d3-3.x-api-reference/blob/master/Ordinal-Scales.md#category10
//
// Matplotlib also uses this color set:
//   https://github.com/matplotlib/matplotlib/blob/b34895dbfaba1293ea2ab8c35d1b2df5a246054c/lib/matplotlib/_color_data.py#L16-L28
// 

/*
Copyright (c) 2015-2018, University of Washington Interactive Data Lab
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors
may be used to endorse or promote products derived from this software
without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace ScottPlot.Palettes;

public class Category10 : IPalette
{
    public string Name { get; } = "Category 10";

    public string Description { get; } = "A set of 10 unque colors used in " +
        "many data visualization libraries such as Matplotlib, Vega, and Tableau";

    public SharedColor[] Colors { get; } = SharedColor.FromHex(HexColors);

    private static readonly string[] HexColors =
    {
        "#1f77b4", "#ff7f0e", "#2ca02c", "#d62728", "#9467bd",
        "#8c564b", "#e377c2", "#7f7f7f", "#bcbd22", "#17becf",
    };
}
