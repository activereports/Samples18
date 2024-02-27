using ActiveReports.Calendar.Design.Designers;
using ActiveReports.Calendar.Design.SmartPanels.Controls;

namespace ActiveReports.Calendar.Design.SmartPanels.Calendar
{
    /// <summary>
    /// Defines visibility page for calendar chart wizard.
    /// </summary>
    internal class CalendarVisibilityPage : CalendarPageBase
	{
		public CalendarVisibilityPage(CalendarDesigner designer)
			: base(designer)
		{
			InitializeControls();
		}

		private void InitializeControls()
		{
			using (new SuspendLayoutTransaction(this, true))
			{
				Controls.Add(new VisibilityEditor(ServiceProvider, Designer.ReportItem));
			}
		}
	}
}
