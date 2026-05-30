using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class StoreReceiptForm : Window
    {
        private Repository<StoreReceipt> _repository;
        private StoreReceipt _editingEntity;
        private bool _isEditMode;
        private Dictionary<int, string> _shops;
        private Dictionary<int, string> _batches;

        public StoreReceiptForm(StoreReceipt entity = null)
        {
            InitializeComponent();
            _repository = new Repository<StoreReceipt>();
            LoadShops();
            LoadBatches();

            if (entity != null)
            {
                _isEditMode = true;
                _editingEntity = entity;
                LoadData();
            }
            else
            {
                _isEditMode = false;
                _editingEntity = new StoreReceipt();
            }
        }

        private void LoadShops()
        {
            _shops = new Dictionary<int, string>();
            string query = "SELECT ид_магазина, название FROM розничный_магазин ORDER BY название";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _shops.Add(Convert.ToInt32(row["ид_магазина"]), row["название"].ToString());
            }

            cboShop.ItemsSource = _shops;
            cboShop.DisplayMemberPath = "Value";
            cboShop.SelectedValuePath = "Key";
        }

        private void LoadBatches()
        {
            _batches = new Dictionary<int, string>();
            string query = "SELECT ид_партии_гп, номер_партии FROM партия_готовой_продукции ORDER BY номер_партии";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _batches.Add(Convert.ToInt32(row["ид_партии_гп"]), row["номер_партии"].ToString());
            }

            cboBatch.ItemsSource = _batches;
            cboBatch.DisplayMemberPath = "Value";
            cboBatch.SelectedValuePath = "Key";
        }

        private void LoadData()
        {
            dpReceiptDate.SelectedDate = _editingEntity.ReceiptDate;

            if (_shops.ContainsKey(_editingEntity.ShopId))
            {
                cboShop.SelectedValue = _editingEntity.ShopId;
            }

            if (_batches.ContainsKey(_editingEntity.FinishedProductBatchId))
            {
                cboBatch.SelectedValue = _editingEntity.FinishedProductBatchId;
            }

            txtQuantity.Text = _editingEntity.Quantity?.ToString();
            txtPurchasePrice.Text = _editingEntity.PurchasePrice?.ToString();
            txtRetailPrice.Text = _editingEntity.RetailPrice?.ToString();
        }

        private void SaveData()
        {
            _editingEntity.ReceiptDate = dpReceiptDate.SelectedDate ?? DateTime.Now;

            if (cboShop.SelectedValue != null)
            {
                _editingEntity.ShopId = (int)cboShop.SelectedValue;
            }

            if (cboBatch.SelectedValue != null)
            {
                _editingEntity.FinishedProductBatchId = (int)cboBatch.SelectedValue;
            }

            if (int.TryParse(txtQuantity.Text, out int quantity))
            {
                _editingEntity.Quantity = quantity;
            }

            if (decimal.TryParse(txtPurchasePrice.Text, out decimal purchasePrice))
            {
                _editingEntity.PurchasePrice = purchasePrice;
            }

            if (decimal.TryParse(txtRetailPrice.Text, out decimal retailPrice))
            {
                _editingEntity.RetailPrice = retailPrice;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboShop.SelectedValue == null)
                {
                    MessageBox.Show("Выберите магазин", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cboBatch.SelectedValue == null)
                {
                    MessageBox.Show("Выберите партию готовой продукции", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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