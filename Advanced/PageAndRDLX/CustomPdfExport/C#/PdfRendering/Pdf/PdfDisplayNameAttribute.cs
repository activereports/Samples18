using System.ComponentModel;

namespace ActiveReports.Samples.Export.Rendering.Pdf
{
	internal class PdfDisplayNameAttribute : DisplayNameAttribute
	{
		public PdfDisplayNameAttribute(string displayName) : base()
		{
			DisplayNameValue = Resources.ResourceManager.GetString(displayName);
		}
	}
}
