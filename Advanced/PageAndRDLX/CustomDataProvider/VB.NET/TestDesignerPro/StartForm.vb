Imports System.IO
Imports GrapeCity.ActiveReports.Design.Advanced

Public Class StartForm
	Dim _reportName As String = "../../../../DemoReport.rdlx"
	Private Sub ReportsForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim node = CreateFileNode(_reportName)
		treeView.Nodes.Add(node)
	End Sub

	Private Shared Function CreateFileNode(ByVal fileName As String) As TreeNode
		Dim file As FileInfo = New FileInfo(fileName)

		Dim reportFileNode As New TreeNode(file.Name)
		reportFileNode.ImageIndex = 2
		reportFileNode.SelectedImageIndex = 2
		reportFileNode.ForeColor = Color.Brown
		reportFileNode.Tag = file.FullName

		Return reportFileNode
	End Function

	Private Sub TreeView_AfterCollapse(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeView.AfterCollapse
		e.Node.ImageIndex = 0
		e.Node.SelectedImageIndex = 0
	End Sub

	Private Sub TreeView_AfterExpand(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treeView.AfterExpand
		e.Node.ImageIndex = 1
		e.Node.ImageIndex = 1
		e.Node.SelectedImageIndex = 1
	End Sub

	'Handle click on Tree Node
	Private Sub TreeView_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles treeView.NodeMouseClick
		e.Node.ImageIndex = 2
		treeView.SelectedNode = e.Node
		Dim reportFile As New FileInfo(e.Node.Tag.ToString())
		_reportName = reportFile.FullName
		designerForm = New DesignerForm()
		designerForm.ExportViewerFactory = New ExportViewerFactory()
		designerForm.SessionSettingsStorage = New SessionSettingsStorage()
		designerForm.LoadReport(_reportName)
		designerForm.Show()
	End Sub

	Private Sub Df_Loaded(ByVal sender As Object, ByVal e As EventArgs) Handles designerForm.Load
		Dim helper = New HelperForm()
		helper.Show()
	End Sub
End Class
