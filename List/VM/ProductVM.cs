using List.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace List.VM
{
    public class ProductVM : ViewModelBase
    {
        public Models.Product Product { get; }

        public Manufacturer Manufacturer { get; }

        public ProductVM(Models.Product product, Manufacturer manufacturer)
        {
            Product = product;
            Manufacturer = manufacturer;
        }

        public string Background => Product.QuantityInStock == 0 ? "LightGray" : "#d3f5f3";

        public string ProductPhoto
        {
            get
            {
                if (string.IsNullOrEmpty(Product.ProductPhoto) )
                {
                    return "/Images/picture.png";
                }

                return Product.ProductPhoto;
            }
        }
        public string FormattedQuantityInStock => $"Количество на складе: {Product.QuantityInStock}";

    }
}
