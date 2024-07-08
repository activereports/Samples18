using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document;
using System;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ActiveReports.Samples.UnboundData
{
	/// <summary>
	/// UnboundDataSample - Illustrates the use of unbound data in ActiveReports.
	/// </summary>
	public partial class MainForm : System.Windows.Forms.Form
	{
		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

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

		/// <summary>
		/// tabDataBinding_SelectedIndexChanged - clears the viewer out when switching
	   /// </summary>
		private void tabDataBinding_SelectedIndexChanged(object sender, EventArgs e)
		{
		  //Clear existing pages
		  //
			arvMain.Document = new SectionDocument();
		}

		//#region Unbound Data Code
		//

		/// <summary>
		/// btnDataSet_Click -  Illustrates using a DataSet as a data source 
		/// </summary>
		private void btnDataSet_Click(object sender, EventArgs e)
		{
			//Create the report.
			var rpt = new SectionReport();
			rpt.AddAssembly(typeof(SQLiteConnection).Assembly);
			rpt.LoadLayout(XmlReader.Create(Properties.Resources.UnboundDSInvoice));
			rpt.Document.Printer.PrinterName = String.Empty;
			//Run and view the report.
			arvMain.LoadDocument(rpt);
		}

		/// <summary>
		/// btnDataReader_Click - Illustrates using a DataReader as a data source 
		/// </summary>
		private void btnDataReader_Click(object sender, System.EventArgs e)
		{
			//Create the report
			var rpt = new SectionReport();
			rpt.AddAssembly(typeof(SQLiteConnection).Assembly);
			rpt.LoadLayout(XmlReader.Create(Properties.Resources.UnboundDRInvoice));
			rpt.Document.Printer.PrinterName = String.Empty;
			//Run and view the report
			arvMain.LoadDocument(rpt);
		}

		/// <summary>
		/// btnTextFile_Click - Illustrates using a TextFile as a data source 
		/// </summary>
		private void btnTextFile_Click(object sender, System.EventArgs e)
		{
			//Create the report
			//UnboundTFInvoice rpt = new UnboundTFInvoice();
			var rpt = new SectionReport();
			rpt.LoadLayout(XmlReader.Create(Properties.Resources.UnboundTFInvoice));
			rpt.Document.Printer.PrinterName = String.Empty;
			//Run and view the report
			arvMain.LoadDocument(rpt);		
		}

		/// <summary>
		/// btnArray_Click - Illustrates using a Array as a data source 
		/// </summary>
		private void btnArray_Click(object sender, System.EventArgs e)
		{
			//Create the report
			var rpt = new SectionReport();
			rpt.LoadLayout(XmlReader.Create(Properties.Resources.UnboundDAInvoice));
			rpt.Document.Printer.PrinterName = String.Empty;
			//Run and view the report
			arvMain.LoadDocument(rpt);
		}

		//#endregion
	}
}
