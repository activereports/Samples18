Imports GrapeCity.ActiveReports.Export.Html.Section
Imports GrapeCity.ActiveReports.Export.Html
Imports GrapeCity.ActiveReports.Export
Imports GrapeCity.ActiveReports.Document
Imports DocumentFormat.OpenXml.Wordprocessing
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml
Imports HtmlToOpenXml
Imports HtmlToOpenXml.IO
Imports System.IO
Imports System.Threading

Public Class HtmlToOpenXmlExport
	Implements IDocumentExport
	Private Const _htmlPath As String = "..\..\htmlTemp\"
	Private Const _htmlFilePath As String = _htmlPath + "temp.html"

	Private Sub HtmlToOpenXml(html As String, generatedDocument As MemoryStream)
		Using buffer = ResourceHelper.GetStream("template.docx")
			buffer.CopyToAsync(generatedDocument)
		End Using

		generatedDocument.Position = 0L
		Using package = WordprocessingDocument.Open(generatedDocument, True)
			Dim mainPart As MainDocumentPart = package.MainDocumentPart
			If mainPart Is Nothing Then
				mainPart = package.AddMainDocumentPart()
				Dim doc = New Wordprocessing.Document(New Body())
				doc.Save(mainPart)
			End If

			Dim converter As New HtmlConverter(mainPart, New CustomWebRequest())
			converter.ParseHtml(html)
			mainPart.Document.Save()
		End Using
	End Sub

	Private Sub WriteAllBytes(stream As Stream, generatedDocument As MemoryStream)
		Dim bytes = generatedDocument.ToArray()
		stream.Write(bytes, 0, bytes.Length)
	End Sub

	Private Sub WriteAllBytes(destFilePath As String, generatedDocument As MemoryStream)
		File.WriteAllBytes(destFilePath, generatedDocument.ToArray())
	End Sub

	Private Function HtmlExport(document As SectionDocument, pageRange As String) As String
		If Not Directory.Exists(_htmlPath) Then
			Directory.CreateDirectory(_htmlPath)
		End If
		Using export = New HtmlExport
			export.Export(document, _htmlFilePath, pageRange)
		End Using
		Return GetHtmlString()
	End Function

	Private Function GetHtmlString() As String
		Using stream As New StreamReader(_htmlFilePath)
			Return stream.ReadToEnd()
		End Using
	End Function

	Private Sub ClearHtmlPath()
		For Each file In New DirectoryInfo(_htmlPath).GetFiles()
			file.Delete()
		Next
		Directory.Delete(_htmlPath)
	End Sub

	Sub Export(document As SectionDocument, filePath As String, pageRange As String) Implements IDocumentExport.Export
		Dim html = HtmlExport(document, pageRange)
		Using generatedDocument As New MemoryStream()
			HtmlToOpenXml(html, generatedDocument)
			WriteAllBytes(filePath, generatedDocument)
		End Using
		ClearHtmlPath()
	End Sub

	Sub Export(document As SectionDocument, filePath As String) Implements IDocumentExport.Export
		CType(Me, IDocumentExport).Export(document, filePath, String.Empty)
	End Sub

	Sub Export(document As SectionDocument, outputStream As Stream) Implements IDocumentExport.Export
		CType(Me, IDocumentExport).Export(document, outputStream, String.Empty)
	End Sub

	Sub Export(document As SectionDocument, outputStream As Stream, pageRange As String) Implements IDocumentExport.Export
		Dim html = HtmlExport(document, pageRange)
		Using generatedDocument As New MemoryStream()
			HtmlToOpenXml(html, generatedDocument)
			WriteAllBytes(outputStream, generatedDocument)
		End Using
		ClearHtmlPath()
	End Sub

	Sub Export(document As SectionDocument, outputHandler As IOutputHtml, pageRange As String) Implements IDocumentExport.Export
		Throw New NotSupportedException("It's not allowed to use IOutputHtml")
	End Sub

	Private Class CustomWebRequest
		Implements IWebRequest

		Public Function FetchAsync(requestUri As Uri, cancellationToken As CancellationToken) As Task(Of HtmlToOpenXml.IO.Resource) Implements IWebRequest.FetchAsync
			Dim fileName As String = Path.GetFileName(requestUri.OriginalString)
			Dim imagePath = Path.Combine(_htmlPath, fileName)
			Dim task = New Task(Of HtmlToOpenXml.IO.Resource)(Function()
																  Dim res = New HtmlToOpenXml.IO.Resource()
																  res.Content = File.OpenRead(imagePath)
																  Return res
															  End Function)
			task.Start()
			Return task
		End Function

		Public Sub Dispose() Implements IDisposable.Dispose
			Throw New NotImplementedException()
		End Sub

		Public Function SupportsProtocol(protocol As String) As Boolean Implements IWebRequest.SupportsProtocol
			Throw New NotImplementedException()
		End Function
	End Class
End Class
