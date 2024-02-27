using GrapeCity.ActiveReports.Core.Rendering;
using GrapeCity.ActiveReports.Design.DdrDesigner.Tools;
using GrapeCity.ActiveReports.PageReportModel;
using GrapeCity.Enterprise.Data.Expressions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace ActiveReports.Calendar.Design.Converters
{

    /// <devdoc>Made public for a CRI's designer.</devdoc>
    internal class LengthExpressionInfoConverter : ExpressionInfoConverterBase
	{
		/// <summary>
		/// The default unit
		/// </summary>
		protected string _defaultUnit;
		/// <summary>
		/// Initializes a new instance of <see cref="LengthExpressionInfoConverter" /> with the specified default length unit.
		/// </summary>
		/// <param name="defaultUnit">The default lenght unit.</param>
		public static LengthExpressionInfoConverter CreateForEnum(string defaultUnit)
		{
			return new LengthExpressionInfoConverter(defaultUnit);
		}

		/// <summary>
		/// Initializes new instance of the <see cref="LengthExpressionInfoConverter"/>
		/// </summary>
		public LengthExpressionInfoConverter()
			: this(string.Empty)
		{
		}

		private LengthExpressionInfoConverter(string defaultUnit) // DO NOT MAKE THIS PUBLIC! PROPERTYGRID WILL CALL IT
		{
			_defaultUnit = defaultUnit;
		}

		/// <summary>
		/// Converts the given object to the ExpressionInfo, using the specified context and culture information. Overrides the ConvertFrom method of TypeConverter.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="Object"/> to convert.</param>
		/// <returns>An <see cref="Object"/> that represents the converted value.</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return ConvertFrom(context, culture, value, false);
		}

		/// <summary>
		/// Overrides the ConvertFrom method of TypeConverter
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="isNullable">Parameter to distinguish if this parameter can accept null values.</param>
		internal object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, bool isNullable)
		{
			if (value is string)
			{
				ExpressionInfo expressionInfo = ExpressionInfo.FromString(value as string);
				if (context == null)
					return expressionInfo;
				if (Util.IsEmptyExpression(expressionInfo) && isNullable)
					return expressionInfo;
				IExpressionEvaluatorService service = Util.GetServiceFromTypeDescriptorContext(typeof(IExpressionEvaluatorService), context) as IExpressionEvaluatorService;
				if (service != null)
				{
					if (!service.IsValid(expressionInfo))
						throw new ArgumentException(Resources.InvalidExpressionFormat);
					if (!expressionInfo.IsConstant)
						return expressionInfo;
					if (expressionInfo.ResultType != ExpressionResultType.String)
						throw new ArgumentException(Resources.InvalidExpressionLength);
					if (Util.IsEmptyExpression(expressionInfo))  // isNullable is false, as earlier condition prevents empty and isNullable
					{
						// Replace empty strings on non-"acceptTransparent" properties with the default property.  
						if (context.PropertyDescriptor != null)
						{
							if (context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)] != null)
							{
								if (((DefaultValueAttribute)context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)]).Value is ExpressionInfo)
									return ((ExpressionInfo)((DefaultValueAttribute)context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)]).Value);
								else
									Debug.Fail("Unrecognized default value type.");
							}
							else
							{
								Debug.Fail("Expected default value not available for this property.  Please add in the PropertyAttributeFactory.");
								throw new ArgumentException(Resources.InvalidExpressionLength);
							}
						}
						throw new ArgumentException(Resources.InvalidExpressionLength);
					}
					//Converting to lower since the Length class cannot handle mixed case ie. 12Pt is invalid, 12pt and 12PT are valid

					string lengthString = Convert.ToString(service.Evaluate(expressionInfo)).ToLower(culture);

					var currentDecimalSeparator = culture.NumberFormat.NumberDecimalSeparator;
					var invariantDecimalSeparator = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
					if (currentDecimalSeparator != invariantDecimalSeparator)
						lengthString = lengthString.Replace(currentDecimalSeparator, invariantDecimalSeparator);

					Length length = new Length(lengthString);
					expressionInfo = ExpressionInfo.Parse(length.ToString(CultureInfo.InvariantCulture), ExpressionResultType.String);
				}
				return expressionInfo;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
