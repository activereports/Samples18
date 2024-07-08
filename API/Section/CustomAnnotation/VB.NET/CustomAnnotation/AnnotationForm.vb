Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document.Section.Annotations
Imports System.IO
Imports System.Resources
Imports System.Text
Imports System.Xml

Public Class AnnotationForm
	Private resource As New ResourceManager(Me.GetType)
	Private WithEvents _tsbAnnotation As New ToolStripButton(resource.GetString("CustomAnnotation"))

	Sub New()
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
		InitializeComponent()
	End Sub

	Private Sub AnnotationForm_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
		' Add custom button for annotation.
		Dim ts As ToolStrip = arvMain.Toolbar.ToolStrip
		ts.Items.Add(_tsbAnnotation)
		'Load report layout and run report
		Dim report As New SectionReport
		report.LoadLayout(XmlReader.Create("..//..//..//..//Report//Annotation.rpx"))
		report.Document.Printer.PrinterName = String.Empty
		arvMain.LoadDocument(DirectCast(report, SectionReport))
	End Sub

	Private Sub tsbExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles _tsbAnnotation.Click
		'Depending on the presence or absence of annotation, to display the confirmation message. 
		If arvMain.Document.Pages(arvMain.ReportViewer.CurrentPage - 1).Annotations.Count > 0 Then
			MessageBox.Show(My.Resources.StampMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
			Return
		End If
		' Gets the image from a resource seal.
		Dim thisExe As Reflection.Assembly
		thisExe = Reflection.Assembly.GetExecutingAssembly()
		Dim file As FileStream = New FileStream("..//..//..//..//Resources//stamp.png", FileMode.Open, FileAccess.Read, FileShare.Read)
#If NETFRAMEWORK Then
        Dim imgStamp = New Bitmap(file)
#Else
		Dim imgStamp = GrapeCity.ActiveReports.Document.Drawing.Image.FromStream(file)
#End If

		' Create an annotation, you can assign the value of the property.
		Dim annoImg As New AnnotationImage()
		annoImg.BackgroundImage = imgStamp ' Image
		annoImg.Color = Color.Transparent  'Background color
		annoImg.BackgroundLayout = ImageLayout.Zoom  ' Display format
		annoImg.ShowBorder = False  'Display border (hidden)  
		' Add a comment.
		' (Specify the additional position)
		annoImg.Attach(6.09F, 1.19F)
		arvMain.Document.Pages(arvMain.ReportViewer.CurrentPage - 1).Annotations.Add(annoImg)
		' (Set the size)
		annoImg.Height = 0.7F
		annoImg.Width = 0.7F

		'To update the Viewer. 
		arvMain.Refresh()
	End Sub

End Class
