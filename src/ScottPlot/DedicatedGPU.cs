using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ScottPlot
{
	public class DedicatedGPU
	{
		[DllImport("nvapi64.dll", EntryPoint = "fake")] // We do not need to enter, only load the dll
		public static extern int LoadNvApi64();
		[System.Runtime.InteropServices.DllImport("nvapi.dll", EntryPoint = "fake")] // Ditto
		static extern int LoadNvApi32();

		/// <summary>
		///		Attempts to force the high-performance graphics profile on nvidia systems
		/// </summary>
		/// <remarks>
		///		This code is uniquely sketchy
		///		Borrowed from: https://stackoverflow.com/a/46749111
		/// </remarks>
		public static void RunWithDedicatedGraphics() {
			try
			{
				if (Environment.Is64BitProcess)
				{
					LoadNvApi64();  //This will always throw as the entry point does not exist
									//On machines with non-nvidia GPUs it should cause no side effects
									//On machines with nvidia GPUs it should use the nvidia GPU for accelleration
				}
				else {
					LoadNvApi32();	//Ditto
				}
			}
			catch { 

			}
		}
	}
}
