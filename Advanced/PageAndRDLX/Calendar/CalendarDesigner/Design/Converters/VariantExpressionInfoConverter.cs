using GrapeCity.ActiveReports.Core.Rendering;
using GrapeCity.ActiveReports.Design.DdrDesigner.Tools;
using GrapeCity.Enterprise.Data.Expressions;
using System;
using System.ComponentModel;
using System.Globalization;

namespace ActiveReports.Calendar.Design.Converters
{

    #region variant
    internal class VariantExpressionInfoConverter : ExpressionInfoConverterBase
	{
		/// <summary>
		/// Converts the given object to the ExpressionInfo, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="Object"/> to convert.</param>
		/// <returns>An <see cref="Object"/> that represents the converted value.</returns>
		/// <remarks>Overrides the ConvertFrom method of ExpressionInfoConverterBase.</remarks>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				value = Util.ExpandShortenedText(value as string);

				//CR 23136: Specify subtype of Variant.  If the subtype is not specified, the default is string.
				ExpressionInfo expressionInfo = ExpressionInfo.Parse(value as string, ExpressionResultType.Variant);
				if (context == null)
					return expressionInfo;
				IExpressionEvaluatorService service = Util.GetServiceFromTypeDescriptorContext(typeof(IExpressionEvaluatorService), context) as IExpressionEvaluatorService;
				if (service != null)
				{
					if (!service.IsValid(expressionInfo))
						throw new ArgumentException(Resources.InvalidExpressionFormat);
					if (expressionInfo.IsConstant)
					{
						if (expressionInfo.ResultType == ExpressionResultType.String && !IsValueExpression((string)value))
							return ExpressionInfo.Parse((string)value, ExpressionResultType.String);
						return ExpressionInfo.Parse(Convert.ToString(service.Evaluate(expressionInfo), CultureInfo.InvariantCulture), expressionInfo.ResultType);
					}
				}
				return expressionInfo;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
	#endregion
}
