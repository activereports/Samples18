using GrapeCity.ActiveReports.Core.Rendering;
using GrapeCity.Enterprise.Data.Expressions;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Drawing;
using System.Globalization;
using System.Linq;
using GrapeCity.ActiveReports.Design.DdrDesigner.Tools;
using GrapeCity.ActiveReports.Design.DdrDesigner.Editors.ColorEditor;
using System.Reflection;

namespace ActiveReports.Calendar.Design.Converters
{

	internal class ColorExpressionInfoConverter : ExpressionInfoConverterBase
	{
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
			var cte = context.PropertyDescriptor.GetEditor(typeof(UITypeEditor)) as ColorTypeEditor;
			if (cte != null)
			{
				bool supportTransparent = cte is ColorTypeEditorWithTransparent;
				var items = ConvertToExpressionInfo(GetColors(supportTransparent));
				return new StandardValuesCollection(items);
			}
			return base.GetStandardValues(context);
		}

		static Color[] GetColors(bool supportsTransparent)
		{
			var colors = ColorsFromStaticMembers(typeof(Color), supportsTransparent);
			Array.Sort(colors, ColorHelper.HsbColorComparer.Singleton);
			return colors;
		}
		/// <summary>
		/// Returns the Colors from all public static properties of the specified type who's return <see cref="System.Type"/> is <see cref="System.Drawing.Color"/>
		/// </summary>
		static Color[] ColorsFromStaticMembers(Type type, bool hasTransparent)
		{
			if (type == null)
				return new Color[0];
			PropertyInfo[] members = type.GetProperties(BindingFlags.Public
				| BindingFlags.GetProperty
				| BindingFlags.Static);

			ArrayList colorList = new ArrayList(members.Length);

			foreach (PropertyInfo prop in members)
			{
				MethodInfo method = prop.GetGetMethod();
				if (method != null && method.ReturnType == typeof(Color))
				{
					Color colorVal = (Color)prop.GetValue(null, null);
					if (!hasTransparent && colorVal == Color.Transparent)
						continue;
					colorList.Add(colorVal);
				}
			}
			return (Color[])colorList.ToArray(typeof(Color));
		}

		/// <summary>
		/// Converts specified colors to expression information.
		/// </summary>
		protected static ICollection ConvertToExpressionInfo(ICollection colors)
		{
			return colors.OfType<Color>().Select(item => ExpressionInfo.FromString((item).Name)).ToArray();
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

		internal object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, bool acceptTransparent)
		{
			return ConvertFrom(context, culture, value, acceptTransparent, false);
		}

		/// <summary>
		/// Overrides the ConvertFrom method of TypeConverter
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="acceptTransparent">Parameter to distinguish if this color property accepts null/transparent values.</param>
		/// <param name="preserveAlpha">Parameter to distinguish if this color property accepts semi-transparent values.</param>
		internal object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, bool acceptTransparent, bool preserveAlpha)
		{
			if (value is string)
			{
				string actualExpression = Util.ExpandShortenedText(value as string);
				ExpressionInfo expressionInfo = ExpressionInfo.FromString(actualExpression);
				if (context == null)
					return expressionInfo;
				IExpressionEvaluatorService service = Util.GetServiceFromTypeDescriptorContext(typeof(IExpressionEvaluatorService), context) as IExpressionEvaluatorService;
				if (service != null)
				{
					if (!service.IsValid(expressionInfo))
						throw new ArgumentException(Resources.InvalidExpressionFormat);
					if (Util.IsEmptyExpression(expressionInfo) || ((string)value).ToLower(CultureInfo.InvariantCulture) == "transparent")
					{
						if (acceptTransparent)
							return ExpressionInfo.FromString("");

						// Replace empty strings on non-"acceptTransparent" properties with the default property.  
						if (context.PropertyDescriptor != null)
						{
							if (context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)] != null)
							{
								if (((DefaultValueAttribute)context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)]).Value is ExpressionInfo)
									return (((DefaultValueAttribute)context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)]).Value);
								else
									Debug.Fail("Unrecognized default value type.");
							}
							else
							{
								Debug.Fail("Expected default value not available for this property.  Please add in the PropertyAttributeFactory.");
								throw new ArgumentException(Resources.InvalidExpressionColor);
							}
						}
						throw new ArgumentException(Resources.InvalidExpressionColor);
					}
					if (!expressionInfo.IsConstant)
						return expressionInfo;
					if (expressionInfo.ResultType != ExpressionResultType.String
						&& expressionInfo.ResultType != ExpressionResultType.Variant)
						throw new ArgumentException(Resources.InvalidExpressionColor);
					string colorString = (string)service.Evaluate(expressionInfo);
					int dummyResult;
					if (Int32.TryParse(colorString, out dummyResult))
						colorString = "#" + colorString;
					Color colorFromHtml = Color.Empty;
					bool invalidColor = false;
					try
					{
						colorFromHtml = ColorTranslator.FromHtml(colorString);
					}
					catch (Exception)
					{
						invalidColor = true;
					}
					if (invalidColor)
						try
						{
							colorString = "#" + colorString;
							colorFromHtml = ColorTranslator.FromHtml(colorString);
							invalidColor = false;
						}
						catch (Exception)
						{
							invalidColor = true;
						}
					// Transparent scenarios are caught earlier.  0 here means that the color could not be converted.
					if (invalidColor || colorFromHtml.ToArgb() == 0)
						throw new ArgumentException(Resources.InvalidExpressionColor);
					colorFromHtml = Util.CheckKnownColors(colorFromHtml);
					if (colorFromHtml.IsNamedColor && colorFromHtml == Color.LightGray)
						// .NET changes LightGray to LightGrey - avoid this change
						colorString = "LightGray";
					else
						colorString = ColorTranslator.ToHtml(colorFromHtml);
					if (!colorFromHtml.IsKnownColor)
					{
						if (preserveAlpha && colorFromHtml.A != 0 && colorFromHtml.A != 0xFF)
							colorString = colorString.Insert(1, colorFromHtml.A.ToString("X2"));
						colorString = colorString.ToLower(CultureInfo.InvariantCulture);
					}
					return ExpressionInfo.FromString(colorString);
				}
				return expressionInfo;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// This will only be called by Transparent and Sub Color types.
		internal object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType, bool displayTransparent)
		{
			if (destinationType == typeof(string))
			{
				if (value is ExpressionInfo)
				{
					ExpressionInfo expressionInfo = (ExpressionInfo)value;
					if (Util.IsEmptyExpression(expressionInfo))
					{
						if (displayTransparent)
							return "Transparent";
						else
							return "";
					}
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
