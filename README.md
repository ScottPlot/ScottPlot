# ScottPlot

[![CI](https://github.com/ScottPlot/ScottPlot/actions/workflows/ci.yaml/badge.svg)](https://github.com/ScottPlot/ScottPlot/actions/workflows/ci.yaml)
[![](https://img.shields.io/nuget/dt/scottplot?color=004880&label=Downloads&logo=NuGet)](https://www.nuget.org/packages/ScottPlot/)
[![Discord](https://badgen.net/discord/members/Dru6fnY2UX?icon=discord&color=5562ea&label=Discord)](https://scottplot.net/discord/)

**ScottPlot is a free and open-source plotting library for .NET** that makes it easy to interactively display large datasets. The [**ScottPlot Cookbook**](https://scottplot.net/cookbook/5.0/) demonstrates how to create line plots, bar charts, pie graphs, scatter plots, and more with just a few lines of code. The **[ScottPlot Demo](https://scottplot.net/demo/)** shows how to create plots in GUI environments with advanced interactive behaviors. ScottPlot supports 
    [Windows Forms](https://scottplot.net/quickstart/winforms/), 
    [WPF](https://scottplot.net/quickstart/wpf/),
    [Console](https://scottplot.net/quickstart/console/),
    [Uno Platform](https://scottplot.net/quickstart/unoplatform/),
    [Blazor](https://scottplot.net/quickstart/blazor/),
    [Avalonia](https://scottplot.net/quickstart/avalonia/),
    [Eto](https://scottplot.net/quickstart/eto/),
    [Notebooks](https://scottplot.net/quickstart/notebook/),
    and [more](https://scottplot.net/quickstart/)!

### Visit https://ScottPlot.NET for documentation and additional information

<div align='center'>

<a href='https://scottplot.net'><img src='dev/graphics/ScottPlot.gif'></a>

<a href='https://scottplot.net/cookbook/5.0/'><img src='dev/graphics/cookbook.jpg'></a>

</div>

<details>
  <summary>Click to expand the step-by-step guide (VS Code)</summary>

  ## Step-by-Step Instructions From Git to Running on your computer on VS Code! 
  1. **Clone the Repository**: 
     ```bash
     git clone https://github.com/your-repo/ScottPlot
     ```
  2. **Install Extension .NET Install Package on VS Code**: 
     ![Screenshot of .NET Extension](Images/Extension_.NET.png)
  3. **Install Polyglot Notebooks Install Package on VS Code**: 
     ![Screenshot of Polyglot Notebooks Extension](Images/Extension_Polyglot_Notebooks.png)
  4. **Install Dependencies in your Terminal**: 
     ```bash
     dotnet add package ScottPlot
     ```
  5. **Create a Temp folder and put bin and obj in there**: 
     ![Screenshot of Temp Folder Structure](Images/Temp_Folder_Creation.png)
  6. **Create a file named Program.cs (example)**: 
     Use the above documentation to create your code and polyglot (Example)
     ![Screenshot of Program.cs](Images/Example_Code.png)
    <details>
    <summary>Explanation of Code</summary>

  1. **Add Coordinates**
  2. **Create a new plot and add the points**
  3. **Play around with the color type!**
  4. **Remember to save your file using `.SavePng()`
  
    </details>
   

  7. **Run the Application in your Terminal**: 
     ```bash
     dotnet run Program.cs
     ```
  8.  **When run application, output would be a .png of your shape**
     ![Screenshot of Output](Images/Output_Polyglot.png)
</details>

**Contributing:** We welcome contributions from the community! We invite contributions from anyone, including developers who may be new to contributing to open-source projects. Visit https://ScottPlot.net/contributing/ to get started!

**License:** ScottPlot was created by [Scott W Harden](https://swharden.com/about/) and enhanced by [many contributions](https://scottplot.net/changelog/) from the [open-source community](https://scottplot.net/contributors/). It is provided under the permissive [MIT license](LICENSE) and is free to modify and use for any purpose.

If you enjoy ScottPlot ***give us a star!*** ⭐ 
