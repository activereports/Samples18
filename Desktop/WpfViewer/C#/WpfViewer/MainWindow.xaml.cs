using DocumentFormat.OpenXml.Wordprocessing;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Extensibility.Rendering.Components;
using GrapeCity.ActiveReports.Viewer.Wpf.View;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ActiveReports.Samples.WPFViewer
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MainWindow : Window
	{
		static readonly string CurrentFileLocation = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\..\..\..\Reports\";

		public MainWindow()
		{
			InitializeComponent();
			Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("..\\..\\..\\..\\resources\\app.ico", UriKind.Relative));
		}

		private void InitializeCustomCommand(ResourceDictionary resources)
		{
			if(resources == null) throw new ArgumentNullException(nameof(resources));

			resources["MyCommand"] = new MyCommand();
        }

		/// <summary>
		/// >Preview Button Click- Load selected report on click.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPreview_Click(object sender, RoutedEventArgs e)
		{
			ComboBoxItem _report = (ComboBoxItem)CmbListBox.SelectedItem;
			//If the 'Add Custom Button' checkbox is checked load custom menu of Wpf Viewer 
			if (chkCustomButton.IsChecked == true)
			{
				string langDictPath = "DefaultWPFViewerTemplates.xaml";
				Uri langDictUri = new Uri(langDictPath, UriKind.Relative);

                ResourceTheme.Source = langDictUri;
				InitializeCustomCommand(ResourceTheme);
            }
			//If the 'Add Custom Button' checkbox is not checked, clear the resource dictionary values.
			if (chkCustomButton.IsChecked == false)
			{
				ResourceTheme.Clear();
			}

			//Load Selected Report
			LoadReport(_report.Content.ToString());
		}

		private void LoadReport(string reportName)
		{
			string extension = Path.GetExtension(reportName);
			switch(extension)
			{
				case ".rpx":
					SectionReport rpt = new SectionReport();
					rpt.LoadLayout(XmlReader.Create(CurrentFileLocation + reportName));
					rpt.Document.Printer.PrinterName = String.Empty;
					ReportViewer.LoadDocument(rpt);
					break;
				case ".rdlx":
					ReportViewer.LoadDocument(CurrentFileLocation + reportName);
					break;
			}
		}

		/// <summary>
		///In the SelectionChanged event of the combo box, disable 'Add Custom Button' checkbox and 'Preview' button, when no report is selected.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CmbListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (CmbListBox.SelectedIndex == 0)
			{
				if (chkCustomButton != null)
				{
					chkCustomButton.IsEnabled = false;
				}
				btnPreview.IsEnabled = false;
			}
			else
			{
				chkCustomButton.IsEnabled = true;

				btnPreview.IsEnabled = true;
			}
		}
	}
}
