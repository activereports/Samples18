﻿Imports GrapeCity.ActiveReports.Design.Advanced

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StartForm
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Me.treeView = New System.Windows.Forms.TreeView()
		Me.SuspendLayout()
		'
		'treeView
		'
		Me.treeView.Dock = System.Windows.Forms.DockStyle.Fill
		Me.treeView.Location = New System.Drawing.Point(0, 0)
		Me.treeView.Name = "treeView"
		Me.treeView.Size = New System.Drawing.Size(800, 450)
		Me.treeView.TabIndex = 3
		'
		'StartForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0F, 96.0F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
		Me.ClientSize = New System.Drawing.Size(800, 450)
		Me.Controls.Add(Me.treeView)
		Me.Name = "StartForm"
		Me.Text = "DemoReport"
		Me.ResumeLayout(False)

	End Sub

	Private WithEvents treeView As TreeView
	Private WithEvents designerForm As DesignerForm
End Class
