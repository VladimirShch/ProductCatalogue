using System;
using System.Windows.Markup;

namespace ProductCatalogue.WPF.Presentation.Common
{
    public class EnumValuesProviderExtension : MarkupExtension
    {
        public Type EnumType { get; init; }

        public EnumValuesProviderExtension(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
            {
                throw new Exception("EnumType must be enum and must not be null");
            }

            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return EnumType.GetEnumValues();
        }
    }
}
