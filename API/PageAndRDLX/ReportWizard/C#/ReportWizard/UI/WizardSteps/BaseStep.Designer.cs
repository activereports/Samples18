namespace ActiveReports.Samples.ReportWizard.UI.WizardSteps 
{
	partial class BaseStep
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
		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseStep));
			this.SuspendLayout();
			// 
			// BaseStep
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.MaximumSize = new System.Drawing.Size(638, 270);
			this.MinimumSize = new System.Drawing.Size(638, 270);
			this.Name = "BaseStep";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
