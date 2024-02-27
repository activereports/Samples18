using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using GrapeCity.ActiveReports.Design.DdrDesigner.Tools;

namespace ActiveReports.Calendar.Design.Converters
{
    /// <summary>
	/// Handles FontFamily property by loading all font names available.
	/// </summary>
	/// <devdoc>Made public for a CRI's designer.</devdoc>
	internal class FontFamilyExpressionInfoConverter : StringExpressionInfoConverter
	{
		StandardValuesCollection _standardValues;

		/// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
		/// <returns>true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> should be called to find a common set of values the object supports; otherwise, false.</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
		
		/// <summary>
		/// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
		/// <returns>A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> that holds a standard set of valid values, or null if the data type does not support a standard set of values.</returns>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if(_standardValues == null)
			{
				var nameList = FontFamily.Families.Select(f => f.Name).ToList();
				object[] fontFamilies = new object[nameList.Count+2];
				fontFamilies[0] = ConvertFromString("=Theme.Fonts!MajorFont.Family");
				fontFamilies[1] = ConvertFromString("=Theme.Fonts!MinorFont.Family");
				int index = 2;
				foreach(string fontName in nameList)
				{
					fontFamilies[index++] = ConvertFromString(fontName);
				}
				_standardValues = new StandardValuesCollection(fontFamilies);
			}
			return _standardValues;
		}
	}
}
