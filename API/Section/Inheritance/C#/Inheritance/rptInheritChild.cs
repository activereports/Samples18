
namespace ActiveReports.Samples.Inheritance
{
	/// <summary>
	/// A description of the overview of the ChildReport。
	/// </summary>
	public partial class rptInheritChild : Inheritance.rptInheritBase
	{
		public rptInheritChild()
		{
			// This call is required by the ActiveReports designer
			InitializeComponent();

			Document.Printer.PrinterName = string.Empty;

			// Sets the path of the csv file.		   
			CsvPath = "../../../../Customers.csv";
		}
	}
}
