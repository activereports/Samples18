using GrapeCity.ActiveReports.Design.Advanced;
using GrapeCity.ActiveReports.Viewer.Win;
using GrapeCity.ActiveReports.Viewer.Win.Internal.Export;
using ActiveReports.Win.Export;

namespace ActiveReports.Samples.TableOfContents
{
	internal class ExportViewerFactory : IExportViewerFactory
	{
		public IExportViewer CreateExportViewer(Viewer viewer)
		{
			return new ExportViewer(viewer);
		}
	}
}
