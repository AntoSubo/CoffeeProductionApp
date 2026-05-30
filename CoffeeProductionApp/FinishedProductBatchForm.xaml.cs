using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class FinishedProductBatchForm : Window
    {
        private Repository<FinishedProductBatch> _repository;
        private FinishedProductBatch _editingEntity;
        private bool _isEditMode;
        private Dictionary<int, string> _productionOrders;
        private Dictionary<int, string> _products;

        public FinishedProductBatchForm(FinishedProductBatch entity = null)
        {
            InitializeComponent();
            _repository = new Repository<FinishedProductBatch>();
            LoadProductionOrders();
            LoadProducts();

            if (entity != null)
            {
                _isEditMode = true;
                _editingEntity = entity;
                LoadData();
            }
            else
            {
                _isEditMode = false;
                _editingEntity = new FinishedProductBatch();
            }
        }

        private void LoadProductionOrders()
        {
            _productionOrders = new Dictionary<int, string>();
            string query = "SELECT ид_заказа, номер_заказа FROM производственный_заказ ORDER BY номер_заказа";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _productionOrders.Add(Convert.ToInt32(row["ид_заказа"]), row["номер_заказа"].ToString());
            }

            cboProductionOrder.ItemsSource = _productionOrders;
            cboProductionOrder.DisplayMemberPath = "Value";
            cboProductionOrder.SelectedValuePath = "Key";
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

        private void LoadData()
        {
            txtBatchNumber.Text = _editingEntity.BatchNumber;
            dpReleaseDate.SelectedDate = _editingEntity.ReleaseDate;

            if (_productionOrders.ContainsKey(_editingEntity.ProductionOrderId))
            {
                cboProductionOrder.SelectedValue = _editingEntity.ProductionOrderId;
            }

            if (_products.ContainsKey(_editingEntity.ProductCatalogId))
            {
                cboProduct.SelectedValue = _editingEntity.ProductCatalogId;
            }

            if (!string.IsNullOrEmpty(_editingEntity.PackageType))
            {
                foreach (ComboBoxItem item in cboPackageType.Items)
                {
                    if (item.Content.ToString() == _editingEntity.PackageType)
                    {
                        cboPackageType.SelectedItem = item;
                        break;
                    }
                }
            }

            txtPackageCount.Text = _editingEntity.PackageCount?.ToString();
            txtUnitCost.Text = _editingEntity.UnitCost?.ToString();
            txtRetailPrice.Text = _editingEntity.RetailPrice?.ToString();
            txtWholesalePrice.Text = _editingEntity.WholesalePrice?.ToString();
            dpExpiryDate.SelectedDate = _editingEntity.ExpiryDate;
        }

        private void SaveData()
        {
            _editingEntity.BatchNumber = txtBatchNumber.Text;
            _editingEntity.ReleaseDate = dpReleaseDate.SelectedDate ?? DateTime.Now;

            if (cboProductionOrder.SelectedValue != null)
            {
                _editingEntity.ProductionOrderId = (int)cboProductionOrder.SelectedValue;
            }

            if (cboProduct.SelectedValue != null)
            {
                _editingEntity.ProductCatalogId = (int)cboProduct.SelectedValue;
            }

            if (cboPackageType.SelectedItem is ComboBoxItem selectedPackage)
            {
                _editingEntity.PackageType = selectedPackage.Content.ToString();
            }

            if (int.TryParse(txtPackageCount.Text, out int packageCount))
            {
                _editingEntity.PackageCount = packageCount;
            }

            if (decimal.TryParse(txtUnitCost.Text, out decimal unitCost))
            {
                _editingEntity.UnitCost = unitCost;
            }

            if (decimal.TryParse(txtRetailPrice.Text, out decimal retailPrice))
            {
                _editingEntity.RetailPrice = retailPrice;
            }

            if (decimal.TryParse(txtWholesalePrice.Text, out decimal wholesalePrice))
            {
                _editingEntity.WholesalePrice = wholesalePrice;
            }

            _editingEntity.ExpiryDate = dpExpiryDate.SelectedDate;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBatchNumber.Text))
                {
                    MessageBox.Show("Введите номер партии", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cboProductionOrder.SelectedValue == null)
                {
                    MessageBox.Show("Выберите производственный заказ", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cboProduct.SelectedValue == null)
                {
                    MessageBox.Show("Выберите продукцию", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaveData();

                if (_isEditMode)
                {
                    _repository.Update(_editingEntity);
                }
                else
                {
                    _repository.Add(_editingEntity);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}