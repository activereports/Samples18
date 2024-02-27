using ActiveReports.Calendar.Design.Properties;
using GrapeCity.ActiveReports.Design;
using GrapeCity.ActiveReports.PageReportModel;
using System;
using System.ComponentModel;

namespace ActiveReports.Calendar.Design.Converters
{
    internal sealed class DesignImageValueTypeConverter : ImageValueExpressionInfoConverterBase
	{
		protected override ImageSource? GetSource(ITypeDescriptorContext context)
		{
			if (context != null && context.Instance is DesignImage)
			{
				var image = (DesignImage)context.Instance;
				try
				{
					return (ImageSource)Enum.Parse(typeof(ImageSource), image.ImageSource, true);
				}
				catch
				{
					return null;
				}
			}

			return null;
		}
	}
}
