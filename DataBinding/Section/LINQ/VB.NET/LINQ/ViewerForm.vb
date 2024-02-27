﻿Imports System.Text
Imports System.Xml
Imports GrapeCity.ActiveReports

Public Class ViewerForm

    Dim _manager As System.Resources.ResourceManager

    Sub New()
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        InitializeComponent()
    End Sub

    '  LINQ to Object
    Private Sub ViewerForm_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        _manager = New System.Resources.ResourceManager(Me.GetType)

        ' To generate a report.
        Dim rpt As New SectionReport
        rpt = New SectionReport
        rpt.LoadLayout(XmlReader.Create("..\\..\\..\\..\\rptLINQtoObject.rpx"))

        arvMain.LoadDocument(rpt)
    End Sub

End Class
