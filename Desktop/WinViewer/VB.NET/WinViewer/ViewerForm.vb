Imports GrapeCity.ActiveReports.Viewer.Win.Internal.Export
Imports System.Reflection
Imports System.IO
Imports System.ComponentModel
Imports System.Text
Imports GrapeCity.Viewer.Common.ViewModel
Imports ActiveReports.Samples.Viewer.ActiveReports.Win.Export
Imports ActiveReports.Samples.Viewer.ActiveReports.Viewer.Helper
Imports LCEArgs = GrapeCity.ActiveReports.Chart.Designer.LocateCredentialsEventArgs
Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Data
Imports GrapeCity.ActiveReports.PageReportModel
Imports Rom = GrapeCity.ActiveReports.PageReportModel
Imports System.Xml

Partial Friend Class ViewerForm
	Inherits Form
	Private _exportForm As ExportForm
	Private _reportType As System.Nullable(Of ExportForm.ReportType)
	Private _openFileName As String

	Public Sub New()
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
		InitializeComponent()
		Icon = My.Resources.App
		SetReportName(Nothing)
	End Sub

	''' <summary>
	''' Loads the document.
	''' </summary>
	''' <param name="file">The file.</param>
	Public Sub LoadDocument(file As FileInfo)
		Try
			_reportType = ViewerHelper.DetermineReportType(file)
			Dim isRdf As Boolean = ViewerHelper.IsRdf((file))
			Dim reportServerUri = If(Not isRdf, ViewerHelper.GetReportServerUri(file), String.Empty)

			If Not String.IsNullOrEmpty(reportServerUri) Then
				Throw New NotSupportedException("Server report is not supported")
			Else
				''viewer.LoadDocument(file.FullName)
				Select Case _reportType
					Case ExportForm.ReportType.Dashboard, ExportForm.ReportType.PageFpl, ExportForm.ReportType.PageCpl
						Dim pageReport As New PageReport(file)
						AddHandler pageReport.Document.LocateCredentials, AddressOf OnLocateCredentials
						viewer.LoadDocument(pageReport.Document)

					Case ExportForm.ReportType.Section
						Dim sectionReport As New SectionReport()
						Using reader As New XmlTextReader(file.FullName)
							sectionReport.LoadLayout(reader, New System.Collections.ArrayList())
						End Using
						sectionReport.Document.Printer.PrinterName = String.Empty
						AddHandler sectionReport.LocateCredentials, AddressOf OnLocateCredentialsSection
						sectionReport.ResourceLocator = New DefaultResourceLocator() With {
							.BaseUri = New Uri(file.FullName)
						}
						viewer.LoadDocument(sectionReport)
					Case Else
						Throw New ArgumentOutOfRangeException($"Unknown report type to load: ""{_reportType}""")
				End Select

			End If
			_openFileName = file.FullName

			ExportToolStripMenuItem.Enabled = True
			SetReportName(openFileDialog.FileName)
		Catch ex As Exception
			viewer.HandleError(ex)
		End Try
	End Sub

	Private Sub OpenMenuItemHandler(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
		openFileDialog.FileName = String.Empty
		openFileDialog.Filter = My.Resources.Filter

		If openFileDialog.ShowDialog(Me) = DialogResult.OK Then
			LoadDocument(New FileInfo(openFileDialog.FileName))
		End If
	End Sub

	Private Sub CloseMenuItemHandler(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
		Close()
	End Sub

	Private Sub SetReportName(reportName As String)
		Text = If(String.IsNullOrEmpty(reportName), String.Format(My.Resources.DefaultTitleFormat, My.Resources.ViewerFormTitle), String.Format(My.Resources.TitleFormat, My.Resources.ViewerFormTitle, Path.GetFileName(reportName)))
	End Sub

	Private Sub AboutMenuItemHandler(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
		Const showAboutBoxMethodName As String = "ShowAboutBox"
		Dim attributes = viewer.[GetType]().GetCustomAttributes(GetType(LicenseProviderAttribute), False)
		If attributes.Length <> 1 Then
			Return
		End If
		Dim attr As LicenseProviderAttribute = CType(attributes(0), LicenseProviderAttribute)
		Dim provider As Object = (CType(Activator.CreateInstance(attr.LicenseProvider), LicenseProvider))

		Dim methodInfo As Object = provider.[GetType]().GetMethod(showAboutBoxMethodName, BindingFlags.NonPublic Or BindingFlags.Instance)

		If methodInfo IsNot Nothing Then
			methodInfo.Invoke(provider, New Object() {viewer.[GetType]()})
		End If
	End Sub

	Private Sub ExportMenuItemHandler(sender As Object, e As EventArgs) Handles ExportToolStripMenuItem.Click
		If _exportForm Is Nothing Then
			_exportForm = New ExportForm()
		End If

		Dim reportType = If(_reportType, DetermineOpenedReportType())

		_exportForm.Show(Me, New ExportViewer(viewer), reportType)
	End Sub

	Private Function DetermineOpenedReportType() As ExportForm.ReportType
		If viewer.Document IsNot Nothing Then
			Return ExportForm.ReportType.Section
		End If
		Return If(viewer.OpenedReport = OpenedReport.Fpl,
			ExportForm.ReportType.PageFpl,
			ExportForm.ReportType.PageCpl)
	End Function

	Private Sub OnLocateCredentialsSection(sender As Object, args As LCEArgs)
		Dim pageArgs = ToLocateCredentialsEventArgs(args)

		OnLocateCredentials(sender, pageArgs)

		args.UserName = pageArgs.UserName
		args.Password = pageArgs.Password
	End Sub

	Private Function ToLocateCredentialsEventArgs(args As LCEArgs) As LocateCredentialsEventArgs
		Dim dataSource = args.DataSource
		Dim connectionString As String = Nothing
		Dim dataProvider As String = Nothing

		If TypeOf dataSource Is DbDataSource Then
			connectionString = DirectCast(dataSource, DbDataSource).ConnectionString
			dataProvider = DirectCast(dataSource, DbDataSource).ProviderName
		End If

		Dim pageDataSource = New DataSource() With {
		.Name = args.Name,
		.ConnectionProperties = New ConnectionProperties With {
			.Prompt = args.PromptText,
			.ConnectString = connectionString,
			.DataProvider = dataProvider
		}
	}

		Return New LocateCredentialsEventArgs(pageDataSource, args.ReportPath, args.PromptText)
	End Function

	Friend Sub OnLocateCredentials(ByVal sender As Object, ByVal args As LocateCredentialsEventArgs)
		If LocateCredentialsEvent IsNot Nothing Then
			RaiseEvent LocateCredentials(sender, args)
		Else
			Dim connectionProperties As ConnectionProperties = args.DataSource.ConnectionProperties
			If connectionProperties Is Nothing Then
				Return
			End If
			Dim dataSourceName = If(args.DataSource IsNot Nothing, args.DataSource.Name, String.Empty)
			Dim prompt As String = connectionProperties.Prompt

			Dim credential = GetCredentials(New GrapeCity.ActiveReports.PageReportModel.DataSource With {
			.Name = dataSourceName,
			.ConnectionProperties = New ConnectionProperties With {
				.Prompt = prompt,
				.ConnectString = connectionProperties.ConnectString,
				.DataProvider = connectionProperties.DataProvider,
				.IntegratedSecurity = connectionProperties.IntegratedSecurity
			}
		})

			args.UserName = credential.UserName
			args.Password = credential.Password
		End If
	End Sub

	Friend Event LocateCredentials As LocateCredentialsEventHandler

	Friend Structure Credentials
		Public Property UserName As String

		Public Property Password As String

		Public Sub New(name As String, password As String)
			Me.UserName = name
			Me.Password = password
		End Sub
	End Structure

	Private Function GetCredentials(dataSource As Rom.DataSource) As Credentials
		Dim credentials As Credentials? = New Credentials()

		Dim prompt As String = dataSource.ConnectionProperties.Prompt
		If String.IsNullOrEmpty(prompt) Then
			Return If(credentials, New Credentials(String.Empty, String.Empty))
		End If

		Dim dataProvider As String = dataSource.ConnectionProperties.DataProvider
		If String.IsNullOrEmpty(dataProvider) Then
			Return If(credentials, New Credentials(String.Empty, String.Empty))
		End If

		credentials = RequestCredentials(dataSource, prompt)
		Return credentials.Value
	End Function

	Private Function RequestCredentials(dataSource As Rom.DataSource, prompt As String) As Credentials?
		Dim credentialsDialog As New CredentialsDialog(dataSource.Name, prompt)

		If credentialsDialog.ShowDialog() = DialogResult.OK Then
			Return New Credentials(credentialsDialog.UserName, credentialsDialog.Password)
		Else
			Return New Credentials(String.Empty, String.Empty)
		End If
	End Function

End Class
