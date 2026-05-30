using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class B2BOrderForm : Window
    {
        private Repository<B2BOrder> _repository;
        private B2BOrder _editingEntity;
        private bool _isEditMode;
        private Dictionary<int, string> _contracts;
        private ObservableCollection<OrderItemViewModel> _orderItems;
        private List<B2BOrderItem> _itemsToSave;

        public class OrderItemViewModel
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        public B2BOrderForm(B2BOrder entity = null)
        {
            InitializeComponent();
            _repository = new Repository<B2BOrder>();
            _orderItems = new ObservableCollection<OrderItemViewModel>();
            _itemsToSave = new List<B2BOrderItem>();
            dgOrderItems.ItemsSource = _orderItems;
            LoadContracts();

            if (entity != null)
            {
                _isEditMode = true;
                _editingEntity = entity;
                LoadData();
                LoadOrderItems();
            }
            else
            {
                _isEditMode = false;
                _editingEntity = new B2BOrder();
            }
        }

        private void LoadContracts()
        {
            _contracts = new Dictionary<int, string>();
            string query = "SELECT ид_договора, номер_договора FROM договор_поставки ORDER BY номер_договора";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _contracts.Add(Convert.ToInt32(row["ид_договора"]), row["номер_договора"].ToString());
            }

            cboContract.ItemsSource = _contracts;
            cboContract.DisplayMemberPath = "Value";
            cboContract.SelectedValuePath = "Key";
        }

        private void LoadData()
        {
            txtOrderNumber.Text = _editingEntity.OrderNumber;
            dpCreationDate.SelectedDate = _editingEntity.CreationDate;

            if (_contracts.ContainsKey(_editingEntity.ContractId))
            {
                cboContract.SelectedValue = _editingEntity.ContractId;
            }

            dpShipmentDate.SelectedDate = _editingEntity.ShipmentDate;
            txtDeliveryMethod.Text = _editingEntity.DeliveryMethod;
            txtTtnNumber.Text = _editingEntity.TtnNumber;

            if (!string.IsNullOrEmpty(_editingEntity.Status))
            {
                foreach (ComboBoxItem item in cboStatus.Items)
                {
                    if (item.Content.ToString() == _editingEntity.Status)
                    {
                        cboStatus.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void LoadOrderItems()
        {
            string query = @"
                SELECT 
                    позиция_заказа_в2в.ид_позиции,
                    позиция_заказа_в2в.количество,
                    позиция_заказа_в2в.оптовая_цена,
                    готовая_продукция_справочник.торговое_наименование AS Продукт,
                    готовая_продукция_справочник.ид_справочника AS ИдПродукта
                FROM позиция_заказа_в2в
                JOIN готовая_продукция_справочник ON позиция_заказа_в2в.справочник_гп_вк = готовая_продукция_справочник.ид_справочника
                WHERE позиция_заказа_в2в.заказ_в2в_вк = @orderId";

            var parameters = new Microsoft.Data.SqlClient.SqlParameter[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("@orderId", _editingEntity.Id)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                _orderItems.Add(new OrderItemViewModel
                {
                    ProductId = Convert.ToInt32(row["ИдПродукта"]),
                    ProductName = row["Продукт"].ToString(),
                    Quantity = Convert.ToInt32(row["количество"]),
                    Price = Convert.ToDecimal(row["оптовая_цена"])
                });

                _itemsToSave.Add(new B2BOrderItem
                {
                    Id = Convert.ToInt32(row["ид_позиции"]),
                    ProductCatalogId = Convert.ToInt32(row["ИдПродукта"]),
                    Quantity = Convert.ToInt32(row["количество"]),
                    WholesalePrice = Convert.ToDecimal(row["оптовая_цена"])
                });
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SelectProductDialog();
            if (dialog.ShowDialog() == true)
            {
                _orderItems.Add(new OrderItemViewModel
                {
                    ProductId = dialog.SelectedProductId,
                    ProductName = dialog.SelectedProductName,
                    Quantity = dialog.Quantity,
                    Price = dialog.Price
                });
            }
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrderItems.SelectedItem != null)
            {
                _orderItems.Remove((OrderItemViewModel)dgOrderItems.SelectedItem);
            }
        }

        private void SaveData()
        {
            _editingEntity.OrderNumber = txtOrderNumber.Text;
            _editingEntity.CreationDate = dpCreationDate.SelectedDate ?? DateTime.Now;

            if (cboContract.SelectedValue != null)
            {
                _editingEntity.ContractId = (int)cboContract.SelectedValue;
            }

            _editingEntity.ShipmentDate = dpShipmentDate.SelectedDate;
            _editingEntity.DeliveryMethod = txtDeliveryMethod.Text;
            _editingEntity.TtnNumber = txtTtnNumber.Text;

            if (cboStatus.SelectedItem is ComboBoxItem selectedStatus)
            {
                _editingEntity.Status = selectedStatus.Content.ToString();
            }
        }

        private void SaveOrderItems(int orderId)
        {
            var itemRepository = new Repository<B2BOrderItem>();

            foreach (var existingItem in _itemsToSave)
            {
                itemRepository.Delete(existingItem.Id);
            }

            foreach (var item in _orderItems)
            {
                var newItem = new B2BOrderItem
                {
                    B2BOrderId = orderId,
                    ProductCatalogId = item.ProductId,
                    Quantity = item.Quantity,
                    WholesalePrice = item.Price
                };
                itemRepository.Add(newItem);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtOrderNumber.Text))
                {
                    MessageBox.Show("Введите номер заказа", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cboContract.SelectedValue == null)
                {
                    MessageBox.Show("Выберите договор", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaveData();

                int orderId;

                if (_isEditMode)
                {
                    _repository.Update(_editingEntity);
                    orderId = _editingEntity.Id;
                }
                else
                {
                    orderId = _repository.Add(_editingEntity);
                }

                SaveOrderItems(orderId);

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