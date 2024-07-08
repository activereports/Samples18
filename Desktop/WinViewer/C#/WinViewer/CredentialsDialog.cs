using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace ActiveReports.Samples.Viewer
{
	public partial class CredentialsDialog : Form
	{
		/// <summary>
		/// Shows credentials request dialog.
		/// </summary>
		[Obfuscation(Feature = "renaming", Exclude = true)]
		/// <summary>
		/// Allows internal callers to access to user name value entered.
		/// </summary>
		public string UserName => txtUser.Text;

		/// <summary>
		/// Allows internal callers to access to password value entered.
		/// </summary>
		public string Password => txtPassword.Text;
		public CredentialsDialog(string dataSourceName, string prompt)
		{
			InitializeComponent();
			this.MaximumSize = new Size(this.Size.Width * 2, this.Size.Height);
			this.MinimumSize = this.Size;
			Text = string.Format(Text, dataSourceName);
			this.lblPrompt.Text = prompt;
			this.txtUser.KeyDown += AskCredentialsDialog_KeyDown;
			this.txtPassword.KeyDown += AskCredentialsDialog_KeyDown;
			this.KeyDown += AskCredentialsDialog_KeyDown;
		}

		private void AskCredentialsDialog_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Enter:
					this.DialogResult = DialogResult.OK;
					break;
				case Keys.Escape:
					this.DialogResult = DialogResult.Cancel;
					break;
			}
		}
	}
}
