using GrapeCity.ActiveReports.Design.DdrDesigner.Tools;
using GrapeCity.ActiveReports.PageReportModel;
using GrapeCity.ActiveReports.Rdl.Themes;
using GrapeCity.Enterprise.Data.Expressions;
using System;
using System.ComponentModel;

namespace ActiveReports.Calendar.Design.Converters
{

    /// <summary>
    /// Defines a class for converting MimeType expressions.
    /// </summary>
    internal sealed class MimeTypeExpressionInfoConverter : StringExpressionInfoConverter
	{
		/// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
		/// </summary>
		/// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		string[] _mimeTypes = new[] {
			"image/bmp",
			"image/jpeg",
			"image/gif",
			"image/png",
			"image/emf",
			"image/wmf",
			"image/x-emf",
			"image/x-wmf",
			"image/svg+xml"
		};
		/// <summary>
		/// Returns a collection of standard values for the MimeType when provided with a format context.
		/// </summary>
		/// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			string[] imageTypes = null;
			int index = 0;
			if (IsImageSourceDatabase(context))
			{
				ITheme theme = Util.GetServiceFromTypeDescriptorContext(typeof(ITheme), context) as ITheme;
				if (theme != null && theme.Theme != null && theme.Theme.Images != null)
				{
					imageTypes = new string[theme.Theme.Images.Count];
					foreach (var image in theme.Theme.Images)
					{
						imageTypes[index++] = String.Format("={0}.MIMEType", Util.GenerateThemeImageExpression(image.Name));
					}
				}
			}
			if (imageTypes == null)
				imageTypes = new string[0];
			index = 0;
			ExpressionInfo[] mimeTypes = new ExpressionInfo[_mimeTypes.Length + imageTypes.Length];
			foreach (string mimeType in imageTypes)
			{
				mimeTypes[index++] = ExpressionInfo.Parse(mimeType, ExpressionResultType.String);
			}
			foreach (string mimeType in _mimeTypes)
			{
				mimeTypes[index++] = ExpressionInfo.Parse(mimeType, ExpressionResultType.String);
			}
			return new StandardValuesCollection(mimeTypes);
		}

		private static bool IsImageSourceDatabase(ITypeDescriptorContext context)
		{
			if (context == null)
				return false;
			if (context.Instance == null)
				return false;
            GrapeCity.ActiveReports.PageReportModel.Image image = context.Instance as GrapeCity.ActiveReports.PageReportModel.Image;
			if (image != null)
				return image.Source == ImageSource.Database;
			return false;
		}

		/// <summary>
		///  Returns whether the collection of standard values returned is an exclusive list of possible values, using the specified context.
		/// </summary>
		/// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
		/// <returns> true if the standard values collection is an exhaustive list of possible values; false if other values are possible.</returns>
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}
	}

}
