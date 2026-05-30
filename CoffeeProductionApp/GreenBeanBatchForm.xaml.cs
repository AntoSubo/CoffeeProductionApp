using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class GreenBeanBatchForm : Window
    {
        private Repository<GreenBeanBatch> _repository;
        private GreenBeanBatch _editingEntity;
        private bool _isEditMode;
        private Dictionary<int, string> _cells;

        public GreenBeanBatchForm(GreenBeanBatch entity = null)
        {
            InitializeComponent();
            _repository = new Repository<GreenBeanBatch>();
            LoadCells();

            if (entity != null && entity.Id > 0)
            {
                _isEditMode = true;
                _editingEntity = entity;
                LoadData();
                this.Title = "Редактирование партии зеленого зерна";
            }
            else
            {
                _isEditMode = false;
                _editingEntity = new GreenBeanBatch();
                this.Title = "Новая партия зеленого зерна";
            }
        }

        private void LoadCells()
        {
            _cells = new Dictionary<int, string>();
            string query = @"
                SELECT 
                    ячейка_хранения.ид_ячейки,
                    ячейка_хранения.код_ячейки,
                    склад.название AS Склад
                FROM ячейка_хранения
                JOIN зона_хранения ON ячейка_хранения.зона_вк = зона_хранения.ид_зоны
                JOIN склад ON зона_хранения.склад_вк = склад.ид_склада
                ORDER BY склад.название, ячейка_хранения.код_ячейки";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                string name = $"{row["Склад"]} - {row["код_ячейки"]}";
                _cells.Add(Convert.ToInt32(row["ид_ячейки"]), name);
            }

            cboCell.ItemsSource = _cells;
            cboCell.DisplayMemberPath = "Value";
            cboCell.SelectedValuePath = "Key";
        }

        private void LoadData()
        {
            txtBatchNumber.Text = _editingEntity.BatchNumber;
            dpReceiptDate.SelectedDate = _editingEntity.ReceiptDate;
            txtCountry.Text = _editingEntity.CountryOfOrigin;
            txtVariety.Text = _editingEntity.Variety;
            txtNetWeight.Text = _editingEntity.NetWeightKg.ToString();
            txtHumidity.Text = _editingEntity.HumidityPercent.ToString();

            if (_cells.ContainsKey(_editingEntity.CellId.GetValueOrDefault()))
            {
                cboCell.SelectedValue = _editingEntity.CellId;
            }

            txtShelfLife.Text = _editingEntity.ShelfLifeMonths?.ToString();
        }

        private void SaveData()
        {
            _editingEntity.BatchNumber = txtBatchNumber.Text;
            _editingEntity.ReceiptDate = dpReceiptDate.SelectedDate ?? DateTime.Now;
            _editingEntity.CountryOfOrigin = txtCountry.Text;
            _editingEntity.Variety = txtVariety.Text;

            if (decimal.TryParse(txtNetWeight.Text, out decimal netWeight))
            {
                _editingEntity.NetWeightKg = netWeight;
            }

            if (decimal.TryParse(txtHumidity.Text, out decimal humidity))
            {
                _editingEntity.HumidityPercent = humidity;
            }

            if (cboCell.SelectedValue != null)
            {
                _editingEntity.CellId = (int)cboCell.SelectedValue;
            }
            else
            {
                _editingEntity.CellId = null;
            }

            if (int.TryParse(txtShelfLife.Text, out int shelfLife))
            {
                _editingEntity.ShelfLifeMonths = shelfLife;
            }
            else
            {
                _editingEntity.ShelfLifeMonths = null;
            }
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

                SaveData();

                if (_isEditMode)
                {
                    _repository.Update(_editingEntity);
                    MessageBox.Show("Запись успешно обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    int newId = _repository.Add(_editingEntity);
                    MessageBox.Show($"Запись успешно добавлена. ID: {newId}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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