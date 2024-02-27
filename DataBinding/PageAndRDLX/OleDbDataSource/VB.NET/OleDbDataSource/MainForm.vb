﻿Imports System.IO
Imports System.Text
Imports GrapeCity.ActiveReports

Public Class MainForm

    Sub New()
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        InitializeComponent()
    End Sub

    ' Loads and shows the report.
    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim rptPath As New FileInfo("..\..\..\..\OleDBReport.rdlx")
        Dim pageReport As New PageReport(rptPath)
        reportPreview.LoadDocument(pageReport.Document)
    End Sub

End Class
