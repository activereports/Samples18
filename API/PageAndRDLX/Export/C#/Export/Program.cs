﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActiveReports.Samples.Export
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
#if NET6_0_OR_GREATER
			Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);
#endif
			Application.Run(new ExportForm());
		}
	}
}
