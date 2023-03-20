using System.Collections.Generic;

namespace ProductCatalogue.WPF.Core.Common
{
    public interface IValidator<T> where T : class
    {
        Dictionary<string, string> Validate(T objectToValidate);
    }
}
