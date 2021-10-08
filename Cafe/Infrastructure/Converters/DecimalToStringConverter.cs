using Cafe.Infrastructure.Converters.Base;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Cafe.Infrastructure.Converters
{
	[ValueConversion(typeof(decimal), typeof(string))]
	[MarkupExtensionReturnType(typeof(string))]
	class DecimalToStringConverter : ConverterBase
	{
		#region Overrides of ConverterBase
		public override object Convert(object v, Type t, object p, CultureInfo c)
		{
			return ((decimal)v).ToString("C", c);
		}

		#endregion
	}
}
