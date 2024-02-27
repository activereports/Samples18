Imports System.Globalization
Imports GrapeCity.ActiveReports.Export.Pdf.Section
Imports GrapeCity.ActiveReports.Export.Pdf.Section.Signing
Imports System.IO
Imports System.Resources
Imports System.Text
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports

Public Class PDFDigitalSignature
    Private _pageDocument As PageDocument

    Public Sub New()
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        InitializeComponent()
    End Sub

    Private Sub PDFDigitalSignature_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Set the default state of the combo "format of the signature"
        cmbVisibilityType.SelectedIndex = 3
        Dim PageReport = New PageReport()
        _pageDocument = PageReport.Document
        PageReport.Load(New FileInfo("..//..//..//..//..//..//Report//Catalog.rdlx"))
        arvMain.LoadDocument(_pageDocument)
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnExport.Click

        Dim pdfRE = New Export.Pdf.Page.PdfRenderingExtension()
        Dim settings = New Export.Pdf.Page.Settings()

        Dim sfd As New SaveFileDialog()
        Dim tmpCursor As Cursor = Nothing
        Dim tempPath As String = String.Empty

        sfd.Title = Me.Text ' Title
        sfd.FileName = "DigitalSignature.pdf" ' Name of the file for initial display
        sfd.Filter = "PDF|*.pdf" ' Filter
        If sfd.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

        Try
            ' Change the cursor.
            Cursor = Cursors.WaitCursor
            Application.DoEvents()

            ' PAdES format
            settings.SignatureFormat = SignatureFormat.ETSI_CAdES_detached
            settings.SignatureDigestAlgorithm = SignatureDigestAlgorithm.SHA256

            ' Sets the type of signature.
            settings.SignatureVisibilityType = CType(cmbVisibilityType.SelectedIndex, VisibilityType)

            ' Set the signature display area.
            settings.SignatureStampBounds = New RectangleF(0.05F, 0.05F, 5.0F, 0.93F)

            ' Sets the character position of the signature text
            settings.SignatureStampTextAlignment = Alignment.Left
            settings.SignatureStampFontName = "Courier New"
            settings.SignatureStampFontSize = 10

            ' Set the rectangle in which the text is placed in the area that displays the signature.
            ' The coordinate specified in this property starts with the top left point, relative to the rectangular signature.
            settings.SignatureStampTextRectangle = New RectangleF(1.2F, 0.0F, 3.8F, 0.93F)

            ' Set the display position of the signature image.
            settings.SignatureStampImageAlignment = Alignment.Center

            ' Set the rectangle image so that it is placed in the area that displays the signature.
            ' The coordinate specified in this property starts with the top left point, relative to the rectangular signature.
            settings.SignatureStampImageRectangle = New RectangleF(0F, 0F, 1.0F, 0.93F)
            settings.SignatureStampImageFileName = Path.GetFullPath("..//..//..//..//Image//northwind.bmp")
            ' Sets the password for the certificate and digital signature.
            ' For X509Certificate2 class, etc. Please refer to the site of Microsoft.
            settings.SignatureCertificateFileName = Path.GetFullPath("..//..//..//..//certificate.pfx")
            settings.SignatureCertificatePassword = "test"
            
            If chkTimeStamp.Checked Then
                settings.SignatureTimeStamp = New TimeStamp("https://tsa.wotrus.com", "", "")
            End If

            ' Sets the time stamp.
            settings.SignatureSignDate = DateTime.Now.ToString(CultureInfo.InvariantCulture)
            ' Signing time
            settings.SignatureDistinguishedNameVisible = False
            ' Contact
            settings.SignatureContact = "support@example.com"
            ' Location
            settings.SignatureLocation = "Pittsburgh"
            
            Dim outputDirectory = New DirectoryInfo(Path.GetDirectoryName(sfd.FileName))
            Dim outputProvider = New Rendering.IO.FileStreamProvider(outputDirectory, sfd.FileName)
            outputProvider.OverwriteOutputFile = True

            ' Export the file.
            _pageDocument.Render(pdfRE, outputProvider, settings)

            'Start the output file (Open)
            Dim ProcessProperties As New ProcessStartInfo
            ProcessProperties.CreateNoWindow = True
            ProcessProperties.UseShellExecute = True
            ProcessProperties.Verb = "open"
            ProcessProperties.FileName = sfd.FileName
            Process.Start(ProcessProperties)

            ' Display the notification message.
            MessageBox.Show(My.Resources.Resource.FinishExportMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As PdfSigningException
            File.Delete(tempPath)
            MessageBox.Show(My.Resources.Resource.LimitMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Replace the cursor
            Cursor = tmpCursor
            Application.DoEvents()

            ' End processing
            sfd.Dispose()
        End Try
    End Sub

End Class
