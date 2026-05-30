using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using CoffeeProductionApp.DAL;

namespace CoffeeProductionApp
{
    public partial class SelectProductDialog : Window
    {
        public int SelectedProductId { get; set; }
        public string SelectedProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        private Dictionary<int, string> _products;

        public SelectProductDialog()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            _products = new Dictionary<int, string>();
            string query = "SELECT ид_справочника, торговое_наименование FROM готовая_продукция_справочник ORDER BY торговое_наименование";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _products.Add(Convert.ToInt32(row["ид_справочника"]), row["торговое_наименование"].ToString());
            }

            cboProduct.ItemsSource = _products;
            cboProduct.DisplayMemberPath = "Value";
            cboProduct.SelectedValuePath = "Key";
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (cboProduct.SelectedValue == null)
            {
                MessageBox.Show("Выберите продукт", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SelectedProductId = (int)cboProduct.SelectedValue;

            if (cboProduct.SelectedItem is KeyValuePair<int, string> selected)
            {
                SelectedProductName = selected.Value;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Quantity = quantity;
            Price = price;

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}