Partial Class TipControl
    ' <summary>
    'Required designer variable.
    ' </summary>
    Private components As System.ComponentModel.IContainer = Nothing
    ' <summary>
    'Clean up any resources being used.
    ' </summary>
    '<param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso Not ((components) Is Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
    ' <summary>
    'Required method for Designer support - do not modify
    'the contents of this method with the code editor.
    ' </summary>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TipControl))
        Me.pictureBox1 = New System.Windows.Forms.PictureBox()
        Me.tipText = New System.Windows.Forms.Label()
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pictureBox1
        '
        resources.ApplyResources(Me.pictureBox1, "pictureBox1")
        Me.pictureBox1.Image = Global.ActiveReports.Samples.ReportWizard.Resources.Info_02
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.TabStop = False
        '
        'tipText
        '
        resources.ApplyResources(Me.tipText, "tipText")
        Me.tipText.Name = "tipText"
        '
        'TipControl
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.tipText)
        Me.Controls.Add(Me.pictureBox1)
        Me.Name = "TipControl"
        Me.Tag = ""
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private pictureBox1 As System.Windows.Forms.PictureBox
    Private tipText As System.Windows.Forms.Label
End Class
