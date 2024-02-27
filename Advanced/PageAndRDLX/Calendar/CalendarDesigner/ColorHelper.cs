using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace ActiveReports.Calendar
{
	internal static class ColorHelper 
	{
		/// <summary>
		/// Specifies the colors to be displayed in the <see cref="ColorListBox"/>
		/// </summary>
		//		[DoNotObfuscateType]
		internal enum ColorPalette
		{
			/// <summary>
			/// All Colors in <see cref="System.Drawing.KnownColor"/>
			/// </summary>
			AllKnownColors,
			/// <summary>
			/// System defined Colors
			/// </summary>
			SystemColors,
			/// <summary>
			/// A web color palette.
			/// </summary>
			WebColors
		}

		public static Color[] GetColors(bool supportsTransparent)
		{
			var colors = ColorsFromStaticMembers(typeof(Color), supportsTransparent);
			Array.Sort(colors, HsbColorComparer.Singleton);
			return colors;
		}

		/// <summary>
		/// Returns the Colors from all public static properties of the specified type who's return <see cref="System.Type"/> is <see cref="System.Drawing.Color"/>
		/// </summary>
		public static Color[] ColorsFromStaticMembers(Type type, bool hasTransparent)
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

		#region HsbColorComparer
		/// <summary>
		/// Compares/sorts colors by Hue, Saturation, then Brightness.
		/// </summary>
		/// <remarks>Both arguments to the compare method must be Color or the result is undefined.</remarks>
		internal sealed class HsbColorComparer : IComparer
		{
			public static readonly HsbColorComparer Singleton = new HsbColorComparer();

			private HsbColorComparer()
			{
			}

			public int Compare(Color x, Color y)
			{
				const int precision = 10000000;

				int diff = (int)(x.GetHue() * precision) - (int)(y.GetHue() * precision);

				if (diff == 0)
					diff = (int)(x.GetSaturation() * precision) - (int)(y.GetSaturation() * precision);

				if (diff == 0)
					diff = (int)(x.GetBrightness() * precision) - (int)(y.GetBrightness() * precision);

				return diff;
			}

			int IComparer.Compare(object x, object y)
			{
				if (x is Color && y is Color)
				{
					return Compare((Color)x, (Color)y);
				}
				return 0;
			}
		}
		#endregion

		#region NamedColorComparer
		/// <summary>
		/// Compares/sorts colors by <see cref="Color.Name"/>.
		/// </summary>
		/// <remarks>Both arguments to the compare method must be Color or the result is undefined.</remarks>
		internal sealed class NamedColorComparer : IComparer
		{
			public static readonly NamedColorComparer Singleton = new NamedColorComparer();

			private NamedColorComparer()
			{
			}

			public int Compare(Color x, Color y)
			{
				return string.Compare(x.Name, y.Name, false, CultureInfo.InvariantCulture);
			}

			int IComparer.Compare(object x, object y)
			{
				if (x is Color && y is Color)
				{
					return Compare((Color)x, (Color)y);
				}
				return 0;
			}
		}
		#endregion
	}
}
