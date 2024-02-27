using System.Data.SQLite;

namespace ActiveReports.Samples.IListBinding.DataLayer
{
	internal class DataProvider
	{
		/// <summary>
		/// Returns a new connection object for reading the data in the ProductCollection
		/// </summary>
		internal static SQLiteConnection NewConnection
			=> new SQLiteConnection(ActiveReports.Samples.IListBinding.DataLayer.Properties.Resources.ConnectionString);
	}
}
