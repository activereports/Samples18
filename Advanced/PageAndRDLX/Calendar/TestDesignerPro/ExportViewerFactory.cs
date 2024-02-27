using ActiveReports.Win.Export;
using GrapeCity.ActiveReports.Design.Advanced;
using GrapeCity.ActiveReports.Viewer.Win;
using GrapeCity.ActiveReports.Viewer.Win.Internal.Export;

namespace ActiveReports.Calendar
{
    internal class ExportViewerFactory : IExportViewerFactory
	{
		public IExportViewer CreateExportViewer(Viewer viewer)
		{
			return new ExportViewer(viewer);
		}

    }
}
