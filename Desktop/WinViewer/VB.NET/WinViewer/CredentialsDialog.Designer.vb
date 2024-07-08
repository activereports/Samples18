<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CredentialsDialog
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
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
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CredentialsDialog))
		Me.btnCancel = New System.Windows.Forms.Button()
		Me.btnOk = New System.Windows.Forms.Button()
		Me.txtPassword = New System.Windows.Forms.TextBox()
		Me.txtUser = New System.Windows.Forms.TextBox()
		Me.lblPrompt = New System.Windows.Forms.Label()
		Me.lblPassword = New System.Windows.Forms.Label()
		Me.lblUser = New System.Windows.Forms.Label()
		Me.SuspendLayout()
		'
		'btnCancel
		'
		Me.btnCancel.CausesValidation = False
		Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		resources.ApplyResources(Me.btnCancel, "btnCancel")
		Me.btnCancel.Name = "btnCancel"
		Me.btnCancel.UseVisualStyleBackColor = True
		'
		'btnOk
		'
		resources.ApplyResources(Me.btnOk, "btnOk")
		Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.btnOk.Name = "btnOk"
		Me.btnOk.UseVisualStyleBackColor = True
		'
		'txtPassword
		'
		resources.ApplyResources(Me.txtPassword, "txtPassword")
		Me.txtPassword.Name = "txtPassword"
		'
		'txtUser
		'
		resources.ApplyResources(Me.txtUser, "txtUser")
		Me.txtUser.Name = "txtUser"
		'
		'lblPrompt
		'
		resources.ApplyResources(Me.lblPrompt, "lblPrompt")
		Me.lblPrompt.Name = "lblPrompt"
		'
		'lblPassword
		'
		resources.ApplyResources(Me.lblPassword, "lblPassword")
		Me.lblPassword.Name = "lblPassword"
		'
		'lblUser
		'
		resources.ApplyResources(Me.lblUser, "lblUser")
		Me.lblUser.Name = "lblUser"
		'
		'CredentialsDialog
		'
		resources.ApplyResources(Me, "$this")
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.Controls.Add(Me.btnCancel)
		Me.Controls.Add(Me.btnOk)
		Me.Controls.Add(Me.txtPassword)
		Me.Controls.Add(Me.txtUser)
		Me.Controls.Add(Me.lblPrompt)
		Me.Controls.Add(Me.lblPassword)
		Me.Controls.Add(Me.lblUser)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.Name = "CredentialsDialog"
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	Private WithEvents btnCancel As Button
	Private WithEvents btnOk As Button
	Private WithEvents txtPassword As TextBox
	Private WithEvents txtUser As TextBox
	Private WithEvents lblPrompt As Label
	Private WithEvents lblPassword As Label
	Private WithEvents lblUser As Label
End Class
