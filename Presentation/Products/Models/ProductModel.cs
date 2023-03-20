using ProductCatalogue.WPF.Core.Common;
using ProductCatalogue.WPF.Core.Products;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ProductCatalogue.WPF.Presentation.Products.Models
{
    public class ProductModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private readonly IValidator<Product> productValidator;
        private readonly IProductModelMapper productModelMapper;

        private int id;
        private string name = string.Empty;
        private ProductType type;
        private int price;

        public ProductModel(IValidator<Product> productValidator, IProductModelMapper productModelMapper)
        {
            this.productValidator = productValidator;
            this.productModelMapper = productModelMapper;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id 
        { 
            get => id;
            set 
            { 
                if(id != value)
                {
                    id = value;
                    RaisePropertyChanged(nameof(Id));
                }
            }
        }
        public string Name 
        { 
            get => name;
            set
            {
                if(name != value)
                {
                    name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        public ProductType Type 
        { 
            get => type;
            set
            {
                if(type != value)
                {
                    type = value;
                    RaisePropertyChanged(nameof(Type));
                    RaisePropertyChanged(nameof(Price));
                }
            } 
        }
        public int Price 
        { 
            get => price;
            set
            {
                if(price != value)
                {
                    price = value;
                    RaisePropertyChanged(nameof(Price));
                }
            } 
        }

        public string this[string columnName] 
        {
            get
            {
                Product product = GetProduct();
                Dictionary<string, string> validationErrors = productValidator.Validate(product);

                validationErrors.TryGetValue(columnName, out string? columnError);

                return columnError ?? string.Empty;
            }
        }

        public bool IsValid
        {
            get
            {
                Product product = GetProduct();
                Dictionary<string, string> validationErrors = productValidator.Validate(product);
                return !validationErrors.Any();
            }
        }

        public string Error => throw new System.NotImplementedException();

        private Product GetProduct()
        {
            Product product = new();
            productModelMapper.FromModel(this, product);

            return product;
        }

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid)));
        }
    }
}
