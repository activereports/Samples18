using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace ActiveReports.Calendar.Design.Converters
{
    /// <summary>
    /// Type converter for the Variant Expression info properties that need to also show a fields list.
    /// </summary>
    /// <devdoc>Made public for a CRI's designer.</devdoc>
    internal class FieldsListVariantExpressionInfoConverter : VariantExpressionInfoConverter
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
		/// Populates the dropdown list of values
		/// </summary>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			var stringCollection = new StringCollection();
			if (context == null || context.Container == null || context.Instance == null)
				return EmptyValuesCollection;

			var host = Util.GetServiceFromTypeDescriptorContext(typeof(IDesignerHost), context) as IDesignerHost;
			if (host == null)
				return EmptyValuesCollection;

			var component = Util.GetSelectedComponent(context);

			stringCollection = GetFields(context, component, host);
			if (stringCollection.Count > 0) return new StandardValuesCollection(stringCollection);

			return EmptyValuesCollection;
		}

		/// <summary>
		/// Gets the fields collection.
		/// </summary>
		protected virtual StringCollection GetFields(ITypeDescriptorContext context, object component, IDesignerHost host) { return new StringCollection(); }
	}
}
