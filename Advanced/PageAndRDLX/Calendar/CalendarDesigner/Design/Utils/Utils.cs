using GrapeCity.ActiveReports.Core.Rendering;
using GrapeCity.ActiveReports.Design.DdrDesigner.Designers;
using GrapeCity.ActiveReports.PageReportModel;
using GrapeCity.Enterprise.Data.Expressions;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ActiveReports.Calendar.Design
{
    internal static class Util
	{

		private static readonly Regex _userRegularExpression = new Regex(@"(?<Expression>((?<ExpressionType>(User))[\W]+(?:Item[\W]+)?(?<Field>(UserID|Language))))+",
														 RegexOptions.IgnoreCase |
														 RegexOptions.CultureInvariant |
														 RegexOptions.ExplicitCapture |
														 RegexOptions.Compiled);


		private static readonly Regex _shortenedFieldsRegularExpression = new Regex(@"((?<!\\)\[(?<Field>(?![@&])([\\].|[^\]])+)\])+",
														 RegexOptions.IgnoreCase |
														 RegexOptions.CultureInvariant |
														 RegexOptions.ExplicitCapture |
														 RegexOptions.Compiled);

		private static readonly Regex _shortenedParametersRegularExpression = new Regex(@"(\[@(?<Field>([\\].|[^\]])+)\])+",
														 RegexOptions.IgnoreCase |
														 RegexOptions.CultureInvariant |
														 RegexOptions.ExplicitCapture |
														 RegexOptions.Compiled);

		private static readonly Regex _shortenedGlobalsRegularExpression = new Regex(@"(\[&(?<Field>[\w ]+)\])+",
														 RegexOptions.IgnoreCase |
														 RegexOptions.CultureInvariant |
														 RegexOptions.ExplicitCapture |
														 RegexOptions.Compiled);

		private const char EscapeSymbol = '\\';

		private const string _globalsPrefix = "&";

		private const string _parametersPrefix = "@";

		private const string _globalsCollectionName = "Globals";

		private const string _userCollectionName = "User";

		public const String EmptyExpression = "\"\"";

		/// <summary>
		/// Expands the shorter and more readable version of the text provided into a valid expression.
		/// </summary>
		internal static string ExpandShortenedText(string text)
		{
			if (text == null)
				return null;
			string expandedText = text;
			if (expandedText.StartsWith("="))
			{
				Match match = _shortenedFieldsRegularExpression.Match(text);
				if (match.Success)
					expandedText = ExpandShortenedText(expandedText, match, string.Empty, s => GenerateFieldValueExpression(UnescapeShortFieldOrParameterName(s)));

				match = _shortenedParametersRegularExpression.Match(expandedText);
				if (match.Success)
					expandedText = ExpandShortenedText(expandedText, match, _parametersPrefix, s => GenerateParameterValueExpression(UnescapeShortFieldOrParameterName(s)));

				match = _shortenedGlobalsRegularExpression.Match(expandedText);
				if (match.Success)
					expandedText = ExpandShortenedText(expandedText, match, _globalsPrefix, GenerateGlobalValueExpression);
			}
			return expandedText;
		}

		private delegate string GenerateExpressionDelegate(string expression);

		private static string ExpandShortenedText(string expandedText, Match match, string prefix, GenerateExpressionDelegate generateExpression)
		{
			while (match.Success)
			{
				string fieldName = match.Groups["Field"].Value;
				string expression = generateExpression(fieldName);
				int indexOfField = expandedText.IndexOf("[" + prefix + fieldName + "]");
				//Make sure there are an even number of " before the field.
				/* Case 35706 : Make sure matching text is found, otherwise substring could be out of range.
					 * This happens when the same field name is used multiple time, string.Replace replaces all
					 * occurrences.
					 */
				if (indexOfField >= 0 &&
					CharCountEven('"', expandedText.Substring(0, indexOfField)))
				{
					expandedText =
						expandedText.Replace("[" + prefix + fieldName + "]",
											 expression);
				}
				match = match.NextMatch();
			}
			return expandedText;
		}

		private static bool CharCountEven(char c, string text)
		{
			bool even = true;
			int index = text.IndexOf(c);
			while (index != -1)
			{
				even = !even;
				index = text.IndexOf(c, index + 1);
			}
			return even;
		}

		private enum ExpressionKind
		{
			Fields,
			Parameters
		}

		/// <summary>
		/// Returns a valid field reference value expression based on the supplied field name.
		/// </summary>
		internal static string GenerateFieldValueExpression(string fieldName)
		{
			return GenerateFieldOrParameterValueExpression(fieldName, ExpressionKind.Fields);
		}

		/// <summary>
		/// Returns a valid parameter reference value expression based on the supplied parameter name.
		/// </summary>
		internal static string GenerateParameterValueExpression(string parameterName)
		{
			return GenerateFieldOrParameterValueExpression(parameterName, ExpressionKind.Parameters);
		}


		private static string GenerateFieldOrParameterValueExpression(string name, ExpressionKind expressionKind)
		{
			name = EscapeFieldOrParameterName(name);
			return string.Format(IsValidCLRIdentifier(name) ? "{0}!{1}.Value" : "{0}.Item(\"{1}\").Value", expressionKind, name);
		}


		/// <summary>
		/// Returns a valid global variable value expression based on the supplied parameter name.
		/// </summary>
		internal static string GenerateGlobalValueExpression(string parameterName)
		{
			string collectionName;
			if (string.Equals(parameterName, "UserID", StringComparison.InvariantCultureIgnoreCase) ||
				string.Equals(parameterName, "Language", StringComparison.InvariantCultureIgnoreCase))
				collectionName = _userCollectionName;
			else
				collectionName = _globalsCollectionName;

			return string.Format(IsValidCLRIdentifier(parameterName) ? "{0}!{1}" : "{0}(\"{1}\")", collectionName, parameterName);
		}

		private static string EscapeFieldOrParameterName(string name)
		{
			return name.Replace("\"", "\"\"");
		}

		static public bool IsValidCLRIdentifier(String identifier)
		{
			if (String.IsNullOrEmpty(identifier))
			{
				return false;
			}
			if (Char.IsDigit(identifier, 0) || identifier[0] == '_') { return false; }
			for (int i = 0; i < identifier.Length; ++i)
			{
				if (!Char.IsLetterOrDigit(identifier, i) && identifier[i] != '_')
				{
					return false;
				}
			}
			return true;
		}

		private static string UnescapeShortFieldOrParameterName(string name)
		{
			var result = new StringBuilder();
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == EscapeSymbol)
				{
					if (name.Length <= i + 1) break;
					i++;
				}
				result.Append(name[i]);
			}

			return result.ToString();
		}


		/// <summary>
		/// Returns the service requested from the given context.
		/// </summary>
		/// <remarks>CR 19430 - In VS the ITypeDescriptorContext is created with a different set of 
		/// services.  This allows for a way to check multiple service containers.</remarks>
		internal static object GetServiceFromTypeDescriptorContext(Type serviceType, ITypeDescriptorContext context)
		{
			object service = context.GetService(serviceType);
			IDesignerHost host;
			if (service == null && (host = context.Container as IDesignerHost) != null)
			{
				service = host.GetService(serviceType);
			}

			var component = context.Instance as IComponent;
			if (service == null && component != null && component.Site != null)
			{
				service = component.Site.GetService(serviceType);
			}

			return service;
		}

		internal static IDesignerHost GetDesignerHostFromContextParent(ITypeDescriptorContext context)
		{
			var parent = context.GetType().GetProperty("Parent").GetValue(context, null);
			return parent.GetType().GetProperty("Container").GetValue(parent, null) as IDesignerHost;
		}

		/// <summary>
		/// Checks if the specified <see cref="ExpressionInfo"/> will be evaluated to an empty string
		/// </summary>
		public static bool IsEmptyExpression(ExpressionInfo expression)
		{
			return
				expression == null ||
				expression == ExpressionInfo.FromString(string.Empty) ||
				expression == ExpressionInfo.FromString(EmptyExpression);
		}

		/// <summary>
		/// Returns a theme image reference value expression based on the supplied image name.
		/// </summary>
		internal static string GenerateThemeImageExpression(string imageName)
		{
			return string.Format(IsValidCLRIdentifier(imageName) ? "Theme.Images!{0}" : "Theme.Images(\"{0}\")", imageName);
		}
		/// <summary>
		/// Returns current primary selected component in designer.
		/// </summary>
		public static IReportComponent GetSelectedComponent(ITypeDescriptorContext context)
		{
			var host = GetServiceFromTypeDescriptorContext(typeof(IDesignerHost), context) as IDesignerHost;
			if (host == null)
				return null;

			var selectionService = Util.GetServiceFromTypeDescriptorContext(typeof(ISelectionService), context) as ISelectionService;
			if (selectionService == null)
			{
				Debug.Fail(typeof(ISelectionService).Name + " is unavailable.");
				return null;
			}

			var component = selectionService.PrimarySelection as IReportComponent;
			if (component == null)
			{
				var childReportItem = selectionService.PrimarySelection as IReportItemChild;
				if (childReportItem != null)
					component = childReportItem.Parent;

				//if (component == null)
				//{
				//	var txMemberDesigner = selectionService.PrimarySelection as TxMemberDesigner;
				//	if (txMemberDesigner != null)
				//		component = txMemberDesigner.Component;
				//}
			}

			return component;
		}

		#region Color helpers

		/// <summary>
		/// Check the color to see if it matches any known color.  If so, return the corresponding known color.
		/// </summary>
		/// <param name="selectedColor">A color created with FromArgb.</param>
		public static Color CheckKnownColors(Color selectedColor)
		{
			if (!selectedColor.IsKnownColor)
			{
				selectedColor = ColorTranslator.FromHtml("#" + selectedColor.Name);
			}
			return selectedColor;
		}
		#endregion


		public static T Evaluate<T>(this IExpressionEvaluatorService evaluator, ExpressionInfo exp)
		{
			return evaluator.Evaluate(exp, default(T));
		}


		public static T Evaluate<T>(this IExpressionEvaluatorService evaluator, ExpressionInfo exp, T defVal)
		{
			if (evaluator == null) return defVal;

			if (IsEmptyExpression(exp))
				return defVal;

			var type = typeof(T);
			if (type.IsEnum)
			{
				var value = evaluator.Evaluate<string>(exp);
				return value != null && Enum.IsDefined(type, value) ? (T)Enum.Parse(type, value) : defVal;
			}

			var refValue = evaluator.Evaluate(exp);
			if (refValue == null)
				return defVal;

			var baseType = Nullable.GetUnderlyingType(type);
			if (baseType != null)
			{
				type = baseType;
			}

			if (type == typeof(string))
			{
				return (T)(object)Convert.ToString(refValue, CultureInfo.InvariantCulture);
			}

			// TODO: should we wrap any exception to return defValue?
			return (T)Convert.ChangeType(refValue, type, CultureInfo.InvariantCulture);
		}

		internal static T GetService<T>(ITypeDescriptorContext context) where T : class
		{
			if (context.Container != null)
				return Util.GetServiceFromTypeDescriptorContext(typeof(T), context) as T;

			var host = Util.GetDesignerHostFromContextParent(context);
			return host != null ? host.GetService(typeof(T)) as T : null;
		}

	}
}
