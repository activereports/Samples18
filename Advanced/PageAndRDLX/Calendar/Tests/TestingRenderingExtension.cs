using ActiveReports.Calendar.Components;
using GrapeCity.ActiveReports.Extensibility.Rendering;
using GrapeCity.ActiveReports.Extensibility.Rendering.Components;
using GrapeCity.ActiveReports.Extensibility.Rendering.IO;
using GrapeCity.ActiveReports.Rendering.Components.Interfaces;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;

namespace ActiveReports.Calendar.Tests
{
    /// <summary>
    /// A RenderingExtension used to test the raptor custom ReportItems.
    /// </summary>
    public class TestingRenderingExtension : IRenderingExtension
	{
		private readonly Hashtable _reportItems = new Hashtable();
		private IReport _report;

		/// <summary>
		/// The report that was last rendered.
		/// </summary>
		public IReport Report
		{
			get { return _report; }
		}

		/// <summary>
		/// Render report to source C# code
		/// </summary>
		/// <param name="report"></param>
		/// <param name="streams"></param>
		public void Render(IReport report, StreamProvider streams)
		{
			Render(report, streams, null, CancellationToken.None);
		}

		/// <summary>
		/// Render report to source C# code with given settings
		/// </summary>
		/// <param name="report"></param>
		/// <param name="streams"></param>
		/// <param name="settings"></param>
		public void Render(IReport report, StreamProvider streams, NameValueCollection settings)
		{
			Render(report, streams, settings, CancellationToken.None);
		}

		/// <summary>
		/// Render report to source C# code with given settings
		/// </summary>
		/// <param name="report"></param>
		/// <param name="streams"></param>
		/// <param name="settings"></param>
		/// <param name="cancel"></param>
		public void Render(IReport report, StreamProvider streams, NameValueCollection settings, CancellationToken cancel)
		{
			if (report == null)
				throw new ArgumentNullException("report");

			var reportBase = (ReportBase)report;

			_report = report;
			foreach (IReportItem ri in 
				reportBase.ReportSections.SelectMany(reportSection => reportSection.Body.ReportItems))
			{
				ReportItemBase dataItem;

				var cri = ri as ICustomReportItemAdapter;
				if (cri != null)
				{
					dataItem = cri.OriginalReportItem as ReportItemBase;
				}
				else
				{
					dataItem = ri as ReportItemBase;
				}

				if (dataItem != null)
				{
					RenderDataItem(dataItem);
				}
			}
		}

		private void RenderDataItem(ReportItemBase dataItem)
		{
			// add report item to the collection
			_reportItems[dataItem.Name] = dataItem;
		}

		/// <summary>
		/// Returns <see cref="IReportItem"/> instance that was rendered.
		/// </summary>
		/// <param name="reportItemName">the report item name to find</param>
		public IReportItem GetReportItem(string reportItemName)
		{
			return (IReportItem)_reportItems[reportItemName];
		}
	}
}
