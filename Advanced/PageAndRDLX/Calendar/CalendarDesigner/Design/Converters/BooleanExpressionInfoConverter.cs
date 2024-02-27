using GrapeCity.ActiveReports.Core.Rendering;
using GrapeCity.ActiveReports.Design.DdrDesigner.Tools;
using GrapeCity.Enterprise.Data.Expressions;
using System;
using System.ComponentModel;
using System.Globalization;

namespace ActiveReports.Calendar.Design.Converters
{
    /// <summary>
	/// Defines a class for converting boolean expressions.
	/// </summary>
	internal class BooleanExpressionInfoConverter : ExpressionInfoConverterBase
	{
		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="object"/> to convert.</param>
		/// <returns>An <see cref="object"/> that represents the converted value.</returns>
		/// <remarks>Overrides the ConvertFrom method of TypeConverter.</remarks>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) 
		{
			if (value is string) 
			{
				string actualExpression = Util.ExpandShortenedText(value as string);
				ExpressionInfo expressionInfo = ExpressionInfo.FromString(actualExpression);
				if(context == null)
					return expressionInfo;
				IExpressionEvaluatorService service = Util.GetServiceFromTypeDescriptorContext(typeof(IExpressionEvaluatorService),context) as IExpressionEvaluatorService;
				if(service != null)
				{
					if(!service.IsValid(expressionInfo))
						throw new ArgumentException(Resources.InvalidExpressionFormat);
					if(!expressionInfo.IsConstant)
						return expressionInfo;
					string boolString = Convert.ToString(service.Evaluate(expressionInfo), CultureInfo.InvariantCulture);
					if(boolString == null)
						throw new ArgumentException(Resources.InvalidExpressionBoolean);

					try
					{
						bool returnValue = !string.IsNullOrEmpty(boolString) && Boolean.Parse(boolString);
						return ExpressionInfo.Parse(returnValue.ToString(CultureInfo.InvariantCulture), ExpressionResultType.String);
					}
					catch
					{
						throw new ArgumentException(Resources.InvalidExpressionBoolean);
					}
				}
				return expressionInfo;
			}
			return base.ConvertFrom(context, culture, value);
		}

        /// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
        /// </summary>
		/// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
   		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		private StandardValuesCollection _standardValues;
		private string[] _standardValuesStrings = new string[] {Boolean.TrueString, Boolean.FalseString};
        /// <summary>
		/// Returns a collection of standard values for the boolean type when provided with a format context.
        /// </summary>
		/// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if(_standardValues == null)
			{
				object[] bools = new object[_standardValuesStrings.Length];
				int index = 0;
				foreach(string boolValue in _standardValuesStrings)
				{
					bools[index++] = this.ConvertFromString(boolValue);
				}
				_standardValues = new StandardValuesCollection(bools);
			}
			return _standardValues;
		}

	}
}
