using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ActiveReports.Viewer.Helper;
using GrapeCity.ActiveReports.Viewer.Win.Internal.Export;
using ActiveReports.Win.Export;
using GrapeCity.ActiveReports.PageReportModel;
using GrapeCity.ActiveReports.Data;
using LCEArgs = GrapeCity.ActiveReports.Chart.Designer.LocateCredentialsEventArgs;
using GrapeCity.ActiveReports;
using System.Xml;

namespace ActiveReports.Samples.Viewer
{
	using Rom = GrapeCity.ActiveReports.PageReportModel;
	internal partial class ViewerForm : Form
	{
		private ExportForm _exportForm;
		private ExportForm.ReportType? _reportType;
		private string _openFileName;

		public ViewerForm()
		{
			InitializeComponent();
			Icon = Properties.Resource.App;
			SetReportName(null);
		}

		/// <summary>
		/// Loads the document.
		/// </summary>
		/// <param name="file">The file.</param>
		public void LoadDocument(FileInfo file)
		{
			try
			{
				_reportType = ViewerHelper.DetermineReportType(file);
				bool isRdf = ViewerHelper.IsRdf((file));
				var reportServerUri = !isRdf ? ViewerHelper.GetReportServerUri(file) : string.Empty;

				if (!string.IsNullOrEmpty(reportServerUri))
				{

					throw new NotSupportedException("Server report is not supported");
				}
				else
				{
					if (isRdf)
						viewer.LoadDocument(file.FullName);
					else
						switch (_reportType)
						{
							case ExportForm.ReportType.Dashboard:
							case ExportForm.ReportType.PageFpl:
							case ExportForm.ReportType.PageCpl:
								PageReport pageReport = new PageReport(file);
								pageReport.Document.LocateCredentials += OnLocateCredentials;
								viewer.LoadDocument(pageReport.Document);
								break;
							case ExportForm.ReportType.Section:
								SectionReport sectionReport = new SectionReport();
								using (XmlTextReader reader = new XmlTextReader(file.FullName))
								{
									sectionReport.LoadLayout(reader, out System.Collections.ArrayList error);
								}
								sectionReport.Document.Printer.PrinterName = String.Empty;
								sectionReport.LocateCredentials += OnLocateCredentialsSection;
								sectionReport.ResourceLocator = new DefaultResourceLocator { BaseUri = new Uri(file.FullName) };
								viewer.LoadDocument(sectionReport);
								break;
							default:
								throw new ArgumentOutOfRangeException($"Unknown report type to load: \"{_reportType}\"");
						}
				}

				_openFileName = file.FullName;
				exportToolStripMenuItem.Enabled = true;
				SetReportName(openFileDialog.FileName);
			}
			catch (Exception ex)
			{
				viewer.HandleError(ex);
			}
		}

		private void OpenMenuItemHandler(object sender, EventArgs e)
		{
			openFileDialog.FileName = string.Empty;
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				LoadDocument(new FileInfo(openFileDialog.FileName));
			}
		}

		private void CloseMenuItemHandler(object sender, EventArgs e)
		{
			Close();
		}

		private void SetReportName(string reportName)
		{
			Text = string.IsNullOrEmpty(reportName) ? string.Format( Properties.Resource.DefaultTitleFormat, Properties.Resource.ViewerFormTitle) : string.Format(Properties.Resource.TitleFormat, Properties.Resource.ViewerFormTitle, Path.GetFileName(reportName));
		}

		private void AboutMenuItemHandler(object sender, EventArgs e)
		{
			const string showAboutBoxMethodName = "ShowAboutBox";
			var attributes = viewer.GetType().GetCustomAttributes(typeof(LicenseProviderAttribute), false);
			if (attributes.Length != 1) return;
			var attr = (LicenseProviderAttribute)attributes[0];
			var provider = ((LicenseProvider)Activator.CreateInstance(attr.LicenseProvider));

			var methodInfo = provider.GetType().GetMethod(showAboutBoxMethodName, BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
				methodInfo.Invoke(provider, new object[] { viewer.GetType() });
		}

		private void ExportMenuItemHandler(object sender, EventArgs e)
		{
			if (_exportForm == null)
			{
				_exportForm = new ExportForm();
			}

			var reportType = _reportType ?? DetermineOpenedReportType();

			_exportForm.Show(this, new ExportViewer(viewer), reportType);
		}

		private ExportForm.ReportType DetermineOpenedReportType()
		{
			if (viewer.Document != null)
				return ExportForm.ReportType.Section;
			return viewer.OpenedReport == GrapeCity.Viewer.Common.ViewModel.OpenedReport.Fpl
				? ExportForm.ReportType.PageFpl
				: ExportForm.ReportType.PageCpl;
		}

		internal void OnLocateCredentialsSection(object sender, LCEArgs args)
		{
			var pageArgs = ToLocateCredentialsEventArgs(args);

			OnLocateCredentials(sender, pageArgs);

			args.UserName = pageArgs.UserName;
			args.Password = pageArgs.Password;
		}

		private LocateCredentialsEventArgs ToLocateCredentialsEventArgs(LCEArgs args)
		{
			var dataSource = args.DataSource;
			string connectionString = null;
			string dataProvider = null;
			if (dataSource is DbDataSource)
			{
				connectionString = ((DbDataSource)dataSource).ConnectionString;
				dataProvider = ((DbDataSource)dataSource).ProviderName;
			}
			var pageDataSource = new DataSource()
			{
				Name = args.Name,
				ConnectionProperties = new ConnectionProperties
				{
					Prompt = args.PromptText,
					ConnectString = connectionString,
					DataProvider = dataProvider
				},
			};

			return new LocateCredentialsEventArgs(pageDataSource, args.ReportPath, args.PromptText);
		}

		internal void OnLocateCredentials(object sender, LocateCredentialsEventArgs args)
		{
			if (LocateCredentials != null)
				LocateCredentials(sender, args);
			else
			{
				ConnectionProperties connectionProperties = args.DataSource.ConnectionProperties;
				if (connectionProperties == null)
					return;
				var dataSourceName = args.DataSource != null ? args.DataSource.Name : string.Empty;
				string prompt = connectionProperties.Prompt;

				var credential = GetCredentials(new GrapeCity.ActiveReports.PageReportModel.DataSource
				{
					Name = dataSourceName,
					ConnectionProperties = new ConnectionProperties
					{
						Prompt = prompt,
						ConnectString = connectionProperties.ConnectString,
						DataProvider = connectionProperties.DataProvider,
						IntegratedSecurity = connectionProperties.IntegratedSecurity
					},
				});

				args.UserName = credential.UserName;
				args.Password = credential.Password;
			}
		}

		/// <summary>
		/// Raises LocateCredentials event. Occurs when credentials is required for data source loading.
		/// </summary>
		internal event LocateCredentialsEventHandler LocateCredentials;

		internal struct Credentials
		{
			/// <summary>
			/// Gets the user name used to connect to data source.
			/// </summary>
			public string UserName { get; private set; }

			/// <summary>
			/// Gets the password used to connect to data source.
			/// </summary>
			public string Password { get; private set; }

			/// <summary>
			/// Initializes new instance of <see cref="Credentials"/> class.
			/// </summary>
			/// <param name="name">The user name used to connect to data source.</param>
			/// <param name="password">The password used to connect to data source.</param>
			public Credentials(string name, string password) : this()
			{
				UserName = name;
				Password = password;
			}
		}

		private Credentials GetCredentials(Rom.DataSource dataSource)
		{
			Credentials? credentials;
			credentials = new Credentials();

			// Prohibit locate credential if locating credential from designer and credential request's count is more than one.
			// It can be happen on locating credential in subreport.
			string prompt = dataSource.ConnectionProperties.Prompt;
			if (string.IsNullOrEmpty(prompt))
				return credentials ?? new Credentials(string.Empty, string.Empty);

			// Prohibit locate credentials if report is blank or data provider is null
			string dataProvider = dataSource.ConnectionProperties.DataProvider;
			if (string.IsNullOrEmpty(dataProvider))
				return credentials ?? new Credentials(string.Empty, string.Empty);

			credentials = RequestCredentials(dataSource, prompt);

			return credentials.Value;
		}

		private Credentials? RequestCredentials(Rom.DataSource dataSource,
			string prompt)
		{
			var credentialsDialog = new CredentialsDialog(dataSource.Name, prompt);
			if (credentialsDialog.ShowDialog() == DialogResult.OK)
			{
				return new Credentials(credentialsDialog.UserName, credentialsDialog.Password);
			}
			else
				return new Credentials(string.Empty, string.Empty);
		}
	}
}
