﻿Imports System.Text

Public Class ViewerForm

	Protected Overrides Sub OnLoad(e As EventArgs)
		MyBase.OnLoad(e)
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
		viewer1.LoadDocument("../../../../DemoReport.rdlx")
	End Sub
End Class
