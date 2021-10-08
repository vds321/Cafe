using Cafe.Infrastructure.Converters.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Cafe.Infrastructure.Converters
{
	[MarkupExtensionReturnType(typeof(string))]
	[ValueConversion(typeof(DateTime), (typeof(string)))]
	internal class DateTimeConverter : ConverterBase
	{
		public override object Convert(object v, Type t, object p, CultureInfo c)
		{
			return v is not DateTime dateTime ? null : dateTime.ToString("dd/MM/yyyy HH:mm");
		}
	}
}
