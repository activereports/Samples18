using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReports.Calendar.Design.Converters
{
	internal sealed class NullableEnumExpressionInfoConverter : EnumExpressionInfoConverter
	{
		/// <summary>
		/// Converts the given object to the ExpressionInfo, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="Object"/> to convert.</param>
		/// <returns>An <see cref="Object"/> that represents the converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return base.ConvertFrom(context, culture, value, true);
		}

		public NullableEnumExpressionInfoConverter()
			: this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="EnumExpressionInfoConverter" /> with the specified enum <see cref="System.Type" /> as the base type.
		/// </summary>
		/// <param name="enumBaseType">The base type of the enum or null if the base type should be determined dynamically from the <see cref="PropertyDescriptor" />. If it is non-null the specified <see cref="Type" /> must represent an enum, or an <see cref="ArgumentException" /> will be thrown.</param>
		/// <exception cref="ArgumentException">Thrown when the specified type is non-null and does not represent an enum.</exception>
		new public static NullableEnumExpressionInfoConverter CreateForEnum(System.Type enumBaseType)
		{
			return new NullableEnumExpressionInfoConverter(enumBaseType);
		}

		private NullableEnumExpressionInfoConverter(System.Type enumBaseType) // DO NOT MAKE THIS PUBLIC! PROPERTYGRID WILL CALL IT
			: base(enumBaseType)
		{ }
	}

}
