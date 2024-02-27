using GrapeCity.ActiveReports.Design.Advanced;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ActiveReports.Samples.TestDesignerPro
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
			string reportName = "../../../../DemoReport.rdlx";
			DesignerForm df = new DesignerForm();
			df.Load += Df_Load;
			df.LoadReport(reportName);
			df.ExportViewerFactory = new ExportViewerFactory();
			df.SessionSettingsStorage = new SessionSettingsStorage();
			Application.Run(df);
		}

		private static void Df_Load(object sender, EventArgs e)
		{
			HelperForm helper = new HelperForm();
			helper.Show();
		}
	}
}
