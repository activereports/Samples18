using GrapeCity.ActiveReports.Design.Advanced;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ActiveReports.Samples.Radar
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
			string _reportName = @"..\..\..\..\..\..\Report\Radar.rdlx";
			DesignerForm df = new DesignerForm();
			df.SessionSettingsStorage = new SessionSettingsStorage();
			df.ExportViewerFactory = new ExportViewerFactory();
			df.LoadReport(_reportName);
			Application.Run(df);
		}
	}
}
