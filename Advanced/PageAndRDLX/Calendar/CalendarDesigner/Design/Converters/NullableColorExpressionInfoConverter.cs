using System;
using System.ComponentModel;
using System.Globalization;

namespace ActiveReports.Calendar.Design.Converters
{

	/// <summary>
	/// Sub properties can accept null and Transparent values and display them as null.
	/// </summary>
	/// <devdoc>Made public for a CRI's designer.</devdoc>
	internal sealed class NullableColorExpressionInfoConverter : ColorExpressionInfoConverter
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
			return ConvertFrom(context, culture, value, true);
		}

		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="Object"/> to convert.</param>
		/// <param name="destinationType">The <see cref="Type"/>  to convert the <paramref name="value"/> parameter to.</param>
		/// <returns>An <see cref="Object"/> that represents the converted value.</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return ConvertTo(context, culture, value, destinationType, false);
		}
	}
}
