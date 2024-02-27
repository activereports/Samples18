
namespace ActiveReports.Samples.Inheritance
{
	/// <summary>
	/// A description of the overview of the ChildReport。
	/// </summary>
	public partial class rptInheritChild : Inheritance.rptInheritBase
	{
		public rptInheritChild()
		{
			//
			// 
			//
			InitializeComponent();
			//
			// ActiveReports Designer support is required.
			//
			// Sets the path of the csv file.		   
			CsvPath = "../../../../Customers.csv";
		}
	}
}
