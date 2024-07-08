Imports System.Text

Public Class CredentialsDialog
	Public ReadOnly Property UserName As String
		Get
			Return txtUser.Text
		End Get
	End Property

	Public ReadOnly Property Password As String
		Get
			Return txtPassword.Text
		End Get
	End Property

	Public Sub New(dataSourceName As String, prompt As String)
		InitializeComponent()
		Text = String.Format(Text, dataSourceName)
		lblPrompt.Text = prompt
		AddHandler Me.txtUser.KeyDown, AddressOf AskCredentialsDialog_KeyDown
		AddHandler Me.txtPassword.KeyDown, AddressOf AskCredentialsDialog_KeyDown
		AddHandler Me.KeyDown, AddressOf AskCredentialsDialog_KeyDown
	End Sub

	Private Sub AskCredentialsDialog_KeyDown(sender As Object, e As KeyEventArgs)
		Select Case e.KeyCode
			Case Keys.Enter
				Me.DialogResult = DialogResult.OK
			Case Keys.Escape
				Me.DialogResult = DialogResult.Cancel
		End Select
	End Sub
End Class