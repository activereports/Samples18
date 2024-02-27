using System.ComponentModel;
using System.Globalization;
using GrapeCity.ActiveReports.Design.DdrDesigner.Tools;
using GrapeCity.Enterprise.Data.Expressions;


namespace ActiveReports.Calendar.Design.Converters
{
	/// <summary>
	/// NullableFontFamilyExpressionInfoConverter
	/// </summary>
	////[DoNotObfuscateType]
	internal sealed class NullableFontFamilyExpressionInfoConverter : FontFamilyExpressionInfoConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				ExpressionInfo expression = ExpressionInfo.FromString((string)value);
				if (EvaluatorService.IsEmptyExpression(expression))
					return expression;
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
