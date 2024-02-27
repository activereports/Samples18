using GrapeCity.ActiveReports.Design;
using GrapeCity.ActiveReports.Viewer.Win;

namespace ActiveReports.Samples.CreateReport
{
	partial class ReportsForm
	{
		/// <summary>
		/// Necessary designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		/// <summary>
		/// Clean resources.
		/// </summary>
		/// <param name="disposing">True if the resource is cleaned, false is not cleaned.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region  Code generated by Windows Form Designer


		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportsForm));
			this.viewer1 = new GrapeCity.ActiveReports.Viewer.Win.Viewer();
			this.SuspendLayout();
			// 
			// viewer1
			// 
			this.viewer1.CurrentPage = 0;
			resources.ApplyResources(this.viewer1, "viewer1");
			this.viewer1.Name = "viewer1";
			this.viewer1.PreviewPages = 0;
			// 
			// viewer1.Sidebar.ParametersPanel
			//
			resources.ApplyResources(this.viewer1.Sidebar.ParametersPanel, "viewer1.Sidebar.ParametersPanel");
			this.viewer1.Sidebar.ParametersPanel.ContextMenu = null;
			this.viewer1.Sidebar.ParametersPanel.Width = 200;
			// 
			// viewer1.Sidebar.SearchPanel
			// 
			resources.ApplyResources(this.viewer1.Sidebar.SearchPanel, "viewer1.Sidebar.SearchPanel");
			this.viewer1.Sidebar.SearchPanel.ContextMenu = null;
			this.viewer1.Sidebar.SearchPanel.Width = 200;
			// 
			// viewer1.Sidebar.ThumbnailsPanel
			// 
			resources.ApplyResources(this.viewer1.Sidebar.ThumbnailsPanel, "viewer1.Sidebar.ThumbnailsPanel");
			this.viewer1.Sidebar.ThumbnailsPanel.ContextMenu = null;
			this.viewer1.Sidebar.ThumbnailsPanel.Width = 200;
			// 
			// viewer1.Sidebar.TocPanel
			// 
			resources.ApplyResources(this.viewer1.Sidebar.TocPanel, "viewer1.Sidebar.TocPanel");
			this.viewer1.Sidebar.TocPanel.ContextMenu = null;
			this.viewer1.Sidebar.TocPanel.Expanded = true;
			this.viewer1.Sidebar.TocPanel.Width = 200;
			this.viewer1.Sidebar.Width = 200;
			// 
			// ReportsForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.viewer1);
			this.Name = "ReportsForm";
			this.Load += new System.EventHandler(this.ReportsForm_Load);
			this.ResumeLayout(false);

		}
		#endregion
		private Viewer viewer1;
		
	}
}
