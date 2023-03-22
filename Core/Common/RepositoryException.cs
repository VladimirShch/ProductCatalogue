using System;

namespace ProductCatalogue.WPF.Core.Common
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string? message) : base(message) { }

        public RepositoryException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
