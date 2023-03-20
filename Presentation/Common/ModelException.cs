using System;

namespace ProductCatalogue.WPF.Presentation.Common
{
    public class ModelException : Exception
    {
        public ModelException(string message) : base(message) { }
    }
}
