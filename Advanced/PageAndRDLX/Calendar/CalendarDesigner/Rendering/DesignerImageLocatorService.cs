using ActiveReports.Calendar.Design.Designers;
using GrapeCity.ActiveReports;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace ActiveReports.Calendar.Rendering
{
    /// <summary>
    /// DesignerImageLocatorService
    /// </summary>
    public sealed class DesignerImageLocatorService : ImageLocatorService
	{
		public DesignerImageLocatorService(CalendarDesigner calendarDesigner)
		{
			IDesignerHost host = calendarDesigner.ReportItem.Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (host == null)
			{
				Debug.Fail("Can get IDesignerHost for calendar report item");
				return;
			}
			_parentPageReport = host.RootComponent as PageReport;

			InitializeServices();
		}
	}
}
