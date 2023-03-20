using ProductCatalogue.WPF.Core.Common;
using System.Collections.Generic;

namespace ProductCatalogue.WPF.Core.Products
{
    // TODO: implement based on validation rules
    public class ProductValidator : IValidator<Product>
    {
        public Dictionary<string, string> Validate(Product product)
        {
            Dictionary<string, string> errors = new();

            if(product.Type == ProductType.Peripheral && product.Price <= 0)
            {
                errors.Add(nameof(product.Price), "Price of the peripheral product must be positive, non-zero value");
            }
            else if(product.Type == ProductType.Integrated && (product.Price < 1000 || product.Price > 2600))
            {
                errors.Add(nameof(product.Price), "The price of the integrated products must be within the range of 1000 to 2600 dollars");
            }
            if (string.IsNullOrEmpty(product.Name))
            {
                errors.Add(nameof(product.Name), "Product name must not be empty");
            }
            return errors;
        }
    }
}
