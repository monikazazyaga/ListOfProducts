using List.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace List.VM
{
    internal class ListOfProductsVM : ViewModelBase
    {
        private string _searchText;
        private Manufacturer _selectedManufacturer;
        private string _selectedPriceSort;
        private ObservableCollection<ProductVM> _allProducts;

        public ObservableCollection<ProductVM> Products { get; set; }
        public ObservableCollection<Manufacturer> Manufacturers { get; set; }

        public string FilteredProductsCount => $"{Products.Count} из {_allProducts.Count}";

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                ApplyFilters();
            }
        }

        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                _selectedManufacturer = value;
                ApplyFilters();
            }
        }

        public string SelectedPriceSort
        {
            get => _selectedPriceSort;
            set
            {
                _selectedPriceSort = value;
                ApplyFilters();
            }
        }

        public ListOfProductsVM()
        {
            Products = new ObservableCollection<ProductVM>();
            _allProducts = new ObservableCollection<ProductVM>();
            LoadProducts();
            LoadManufacturers();
            Manufacturer allManufacturers = new Manufacturer { ID = 0, Name = "Все производители" };
            Manufacturers.Insert(0, allManufacturers);
        }

        private void LoadProducts()
{
    string connectionString = "Data Source=|DataDirectory|Products.db;Version=3;";

    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
    {
        connection.Open();

        string query = "SELECT * FROM Products";
        SQLiteCommand command = new SQLiteCommand(query, connection);

        SQLiteDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Models.Product product = new Models.Product
            {
                ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                ProductPhoto = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                Name = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Price = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3),
                ManufacturerID = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                QuantityInStock = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
            };

            Manufacturer manufacturer = GetManufacturerById(product.ManufacturerID);
            ProductVM productVM = new ProductVM(product, manufacturer);
            Products.Add(productVM);
            _allProducts.Add(productVM);
        }
    }
}
        private Manufacturer GetManufacturerById(int manufacturerId)
        {
            Manufacturer manufacturer = null;

            string connectionString = "Data Source=|DataDirectory|Products.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Manufacturers WHERE ID = @ManufacturerId";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    manufacturer = new Manufacturer
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                    };
                }
            }

            return manufacturer;
        }

        private void LoadManufacturers()
        {
            Manufacturers = new ObservableCollection<Manufacturer>();

            string connectionString = "Data Source=|DataDirectory|Products.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Manufacturers";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Manufacturer manufacturer = new Manufacturer
                    {
                        ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1)
                    };
                    Manufacturers.Add(manufacturer);
                }
            }
        }

        private void ApplyFilters()
        {
            IEnumerable<ProductVM> filteredProducts = _allProducts;

            if (!string.IsNullOrEmpty(SearchText))
            {
                filteredProducts = filteredProducts.Where(p =>
                    p.Product.Name.ToLower().Contains(SearchText.ToLower()) ||
                    p.Manufacturer.Name.ToLower().Contains(SearchText.ToLower()) ||
                    p.Product.Price.ToString().Contains(SearchText));
            }

            if (SelectedManufacturer != null && SelectedManufacturer.ID != 0)
            {
                filteredProducts = filteredProducts.Where(p => p.Product.ManufacturerID == SelectedManufacturer.ID);
            }

            if (SelectedPriceSort == "По возрастанию")
            {
                filteredProducts = filteredProducts.OrderBy(p => p.Product.Price);
            }
            else if (SelectedPriceSort == "По убыванию")
            {
                filteredProducts = filteredProducts.OrderByDescending(p => p.Product.Price);
            }

            Products = new ObservableCollection<ProductVM>(filteredProducts);
            OnPropertyChanged(nameof(Products));
            OnPropertyChanged(nameof(FilteredProductsCount));
        }


    }

}

