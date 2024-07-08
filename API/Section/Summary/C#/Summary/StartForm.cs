using GrapeCity.ActiveReports;
using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ActiveReports.Samples.CalculatedFields
{
	/// <summary>
	/// CalculatedFields sample shows how to create a new Calculated Field in an ActiveReport and use it with the built in summary features.
	/// </summary>
	public partial class StartForm : System.Windows.Forms.Form
	{
		public StartForm()
		{
			InitializeComponent();
			comboBox1.Items.AddRange(new[] { Properties.Resources.OrdersReport, Properties.Resources.DataFieldExpressionsReport });
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#if NET6_0_OR_GREATER
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
			Application.Run(new StartForm());
		}

		private void Button_Click(object sender, EventArgs e)
		{
			var rpt = new SectionReport();
			var reportPath = "..\\..\\..\\..\\" + (string)comboBox1.SelectedItem;
			rpt.LoadLayout(XmlReader.Create(reportPath));
			rpt.Document.Printer.PrinterName = String.Empty;
			rpt.PrintWidth = 6.5F;
			arvMain.LoadDocument(rpt);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!button1.Enabled && comboBox1.SelectedItem != null)
			{
				button1.Enabled = true;
			}
		}

		private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}
	}
}
