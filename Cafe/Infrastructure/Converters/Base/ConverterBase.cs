using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;


namespace Cafe.Infrastructure.Converters.Base
{
    internal abstract class ConverterBase : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider sp) => this;
        #region Implementation of IValueConverter

        public abstract object Convert(object v, Type t, object p, CultureInfo c);

        public virtual object ConvertBack(object v, Type t, object p, CultureInfo c)
        {
            throw new NotSupportedException("Обратное преобразование не поддерживается");
        }

        #endregion
    }
}
