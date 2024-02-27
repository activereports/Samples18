using ActiveReports.Samples.ReportWizard.UI;
using System.Resources;

namespace ActiveReports.Samples.ReportWizard.UI.WizardSteps
{
	partial class SelectMasterReport
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectMasterReport));
			label1 = new System.Windows.Forms.Label();
			reportList = new System.Windows.Forms.ListBox();
			tipControl1 = new TipControl();
			SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(label1, "label1");
			label1.Name = "label1";
			// 
			// reportList
			// 
			reportList.DisplayMember = "Title";
			reportList.FormattingEnabled = true;
			resources.ApplyResources(reportList, "reportList");
			reportList.Name = "reportList";
			reportList.SelectedIndexChanged += reportList_SelectedIndexChanged;
			// 
			// tipControl1
			// 
			resources.ApplyResources(tipControl1, "tipControl1");
			tipControl1.Name = "tipControl1";
			// 
			// SelectMasterReport
			// 
			resources.ApplyResources(this, "$this");
			Controls.Add(tipControl1);
			Controls.Add(reportList);
			Controls.Add(label1);
			Description = "Choose the type of data you would like to analyze.";
			Name = "SelectMasterReport";
			Title = "Select Report Type";
			ResumeLayout(false);
			PerformLayout();
		}
		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox reportList;
		private TipControl tipControl1;
	}
}
