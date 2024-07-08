using GrapeCity.ActiveReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace ActiveReports.Samples.LINQ
{
	public partial class ViewerForm : Form
	{
		public ViewerForm()
		{
			InitializeComponent();
		}
	  
		// Define a structure for LINQtoObject.
		private void ViewerForm_Load(object sender, EventArgs e)
		{
			// To generate a report.
			var rpt = new SectionReport();
			rpt.LoadLayout(XmlReader.Create("..\\..\\..\\..\\rptLINQtoObject.rpx"));
			rpt.Document.Printer.PrinterName = String.Empty;
			// To run the report.
			arvMain.LoadDocument(rpt);
		}
	}
}
