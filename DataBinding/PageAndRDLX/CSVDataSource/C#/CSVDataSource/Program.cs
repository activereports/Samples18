using System;
using System.Text;
using System.Windows.Forms;

namespace ActiveReports.Samples.CSVDataSource
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
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
			Application.Run(new MainForm());
		}
	}
}
