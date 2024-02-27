﻿Imports System.IO
Imports System.Text
Imports GrapeCity.ActiveReports

Public Class ReportsForm

    Public Sub New()
        'Necessary designer variable.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        InitializeComponent()
    End Sub

    Private Sub ReportsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load a layout into a page report object.
        Dim report As PageReport = LayoutBuilder.BuildReportLayout()
        ' Add a data source to a page report object.
        report = LayoutBuilder.AddDataSetDataSource(report)
        ' Load a page report object into a stream.
        Dim reportStream As MemoryStream = LayoutBuilder.LoadReportToStream(report)

        arvMain.LoadDocument(reportStream, GrapeCity.Viewer.Common.DocumentFormat.Rdlx)
        report.Dispose()
        reportStream.Dispose()
    End Sub

End Class
