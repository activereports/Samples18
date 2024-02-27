using System;
using System.ComponentModel;
using System.Globalization;

namespace ActiveReports.Calendar.Design.Converters
{
	internal sealed class NullableLengthExpressionInfoConverter : LengthExpressionInfoConverter
	{
		/// <summary>
		/// Converts the given object to the ExpressionInfo, using the specified context and culture information. Overrides the ConvertFrom method of TypeConverter.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="Object"/> to convert.</param>
		/// <returns>An <see cref="Object"/> that represents the converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return base.ConvertFrom(context, culture, value, true);
		}
	}
}
