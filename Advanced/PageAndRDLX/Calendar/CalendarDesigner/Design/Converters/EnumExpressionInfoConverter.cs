using GrapeCity.ActiveReports.Core.Rendering;
using GrapeCity.ActiveReports.Design.DdrDesigner.Tools;
using GrapeCity.ActiveReports.Rdl;
using GrapeCity.Enterprise.Data.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace ActiveReports.Calendar.Design.Converters
{
    public class EnumExpressionInfoConverter : ExpressionInfoConverterBase
	{
		private System.Type _enumType;

		public EnumExpressionInfoConverter()
			: this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="EnumExpressionInfoConverter" /> with the specified enum <see cref="System.Type" /> as the base type.
		/// </summary>
		/// <param name="enumBaseType">The base type of the enum or null if the base type should be determined dynamically from the <see cref="PropertyDescriptor" />. If it is non-null the specified <see cref="Type" /> must represent an enum, or an <see cref="ArgumentException" /> will be thrown.</param>
		/// <exception cref="ArgumentException">Thrown when the specified type is non-null and does not represent an enum.</exception>
		public static EnumExpressionInfoConverter CreateForEnum(System.Type enumBaseType)
		{
			return new EnumExpressionInfoConverter(enumBaseType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumExpressionInfoConverter"/> class.
		/// </summary>
		/// <param name="enumBaseType">Type of the enum base.</param>
		/// <exception cref="System.ArgumentException">Expected enum;enumBaseType</exception>
		protected EnumExpressionInfoConverter(System.Type enumBaseType) 
		{
			if (enumBaseType != null && !enumBaseType.IsEnum)
				throw new ArgumentException("Expected enum", "enumBaseType");
			_enumType = enumBaseType;
		}

		/// <summary>
		/// Converts the given object to the ExpressionInfo, using the specified context and culture information.
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
		internal object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value, bool acceptsNullValues)
		{
			if (!(value is string))
				return base.ConvertFrom(context, culture, value);

			ExpressionInfo expressionInfo = ExpressionInfo.FromString(value as string);
			if (context == null)
				return expressionInfo;
			if (Util.IsEmptyExpression(expressionInfo))
			{
				if (acceptsNullValues)
					return expressionInfo;
				// Replace empty strings on non-"acceptsNullValues" properties with the default property.  
				if (context.PropertyDescriptor != null)
				{
					if (context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)] != null)
					{
						if (((DefaultValueAttribute)context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)]).Value is ExpressionInfo)
							return (((DefaultValueAttribute)context.PropertyDescriptor.Attributes[typeof(DefaultValueAttribute)]).Value);
						Debug.Fail("Unrecognized default value type.");
					}
					else
					{
						Debug.Fail("Expected default value not available for this property.  Please add in the PropertyAttributeFactory.");
						throw new ArgumentException(Resources.InvalidExpressionFormat);
					}
				}
				throw new ArgumentException(Resources.InvalidExpressionFormat);
			}
			if (culture == null)
				culture = CultureInfo.CurrentUICulture;

			IExpressionEvaluatorService evaluatorService = Util.GetServiceFromTypeDescriptorContext(typeof(IExpressionEvaluatorService), context) as IExpressionEvaluatorService;
			if (evaluatorService == null)
			{
				Debug.Fail(typeof(IExpressionEvaluatorService).Name + " is not available.");
				return expressionInfo;
			}
			if (!evaluatorService.IsValid(expressionInfo))
				throw new ArgumentException(Resources.InvalidExpressionFormat);
			if (!expressionInfo.IsConstant)
				return expressionInfo;
			if (expressionInfo.ResultType != ExpressionResultType.String)
				throw new ArgumentException(Resources.InvalidExpressionFormat);

			var evaluatedExpression = evaluatorService.Evaluate<string>(expressionInfo);

			Type type = GetEnumType(context);
			bool parseSucceeded = TryParseEnumString(context, evaluatedExpression, out expressionInfo);
			if (parseSucceeded)
			{
				return expressionInfo;
			}

			string separator = culture.TextInfo.ListSeparator + ' ';
			// Invalid expression: "Literal expressions on this property must be an enumerator of the following values:"
			string message = Resources.InvalidExpressionEnumerator;
			message += string.Join(separator, Enum.GetNames(type));
			throw new ArgumentException(message);
		}

		/// <summary>
		/// Parses the specified string as an enum. If the parsing fails, null is returned.
		/// </summary>
		/// <returns>True if the parsing succeeded, false otherwise.</returns>
		private bool TryParseEnumString(ITypeDescriptorContext context, string enumString, out ExpressionInfo expression)
		{
			Type type = GetEnumType(context);
			if (type == typeof(InvalidEnum))
			{
				expression = ExpressionInfo.FromString(enumString);
				return true;
			}
			object enumValue = Enum.Parse(type, enumString, true);
			//Re-assign the expression value to the proper case-sensitive enum.
			string actualName;
			try
			{
				actualName = Enum.GetName(type, enumValue);
			}
			catch (ArgumentNullException)
			{
				expression = ExpressionInfo.Parse("", ExpressionResultType.Variant);
				return false;
			}
			catch (ArgumentException)
			{
				expression = ExpressionInfo.Parse("", ExpressionResultType.Variant);
				return false;
			}
			if (string.IsNullOrEmpty(actualName))
			{
				expression = ExpressionInfo.Parse("", ExpressionResultType.Variant);
				return false;
			}
			expression = ExpressionInfo.FromString(actualName);
			return true;
		}

		/// <summary>
		/// Returns the type of enum that should be used by this converter.
		/// </summary>
		private Type GetEnumType(ITypeDescriptorContext context)
		{
			if (_enumType == null)
			{
				ExpressionBaseTypeAttribute baseTypeAttribute = context.PropertyDescriptor.Attributes[typeof(ExpressionBaseTypeAttribute)] as ExpressionBaseTypeAttribute;
				if (baseTypeAttribute != null)
				{
					_enumType = baseTypeAttribute.BaseType;
				}
				/*else
				{
					Debug.WriteLine("Should not get here.");

					Attribute[] attributes = PropertyAttributeFactory.CreateAttributes(context.Instance.GetType(), context.PropertyDescriptor.Name);
					foreach (Attribute attribute in attributes)
					{
						if ((baseTypeAttribute = attribute as ExpressionBaseTypeAttribute) != null)
						{
							_enumType = baseTypeAttribute.BaseType;
							break;
						}
					}
					if (baseTypeAttribute == null)
					{
						_enumType = typeof(InvalidEnum);
					}
				}*/
				Debug.Assert(_enumType != null && _enumType.IsEnum, "Expected a Type representing an Enum");
			}
			return _enumType;
		}

		/// <summary>
		/// When all else fails, we use this empty enum.
		/// </summary>
		private enum InvalidEnum
		{
		}

		/// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// Gets the standard values collection.
		/// </summary>
		/// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			var enumType = GetEnumType(context);
			var expressions = new List<ExpressionInfo>();
			foreach (var name in Enum.GetNames(enumType))
			{
				var browsableAttribute = enumType.GetMember(name)[0].GetCustomAttributes(typeof(BrowsableAttribute), false).Cast<BrowsableAttribute>().FirstOrDefault();
				if (browsableAttribute != null && !browsableAttribute.Browsable)
					continue;
				expressions.Add(name);
			}
			return new StandardValuesCollection(expressions);
		}
	}
}
