using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class AgroOperationForm : Window
    {
        private Repository<AgroOperation> _repository;
        private AgroOperation _editingEntity;
        private bool _isEditMode;
        private Dictionary<int, string> _plantations;
        private Dictionary<int, string> _operationTypes;

        public AgroOperationForm(AgroOperation entity = null)
        {
            InitializeComponent();
            _repository = new Repository<AgroOperation>();
            LoadPlantations();
            LoadOperationTypes();

            if (entity != null)
            {
                _isEditMode = true;
                _editingEntity = entity;
                LoadData();
            }
            else
            {
                _isEditMode = false;
                _editingEntity = new AgroOperation();
            }
        }

        private void LoadPlantations()
        {
            _plantations = new Dictionary<int, string>();
            string query = "SELECT ид_плантации, название FROM плантация ORDER BY название";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _plantations.Add(Convert.ToInt32(row["ид_плантации"]), row["название"].ToString());
            }

            cboPlantation.ItemsSource = _plantations;
            cboPlantation.DisplayMemberPath = "Value";
            cboPlantation.SelectedValuePath = "Key";
        }

        private void LoadOperationTypes()
        {
            _operationTypes = new Dictionary<int, string>();
            string query = "SELECT ид_типа_операции, название FROM тип_операции ORDER BY название";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _operationTypes.Add(Convert.ToInt32(row["ид_типа_операции"]), row["название"].ToString());
            }

            cboOperationType.ItemsSource = _operationTypes;
            cboOperationType.DisplayMemberPath = "Value";
            cboOperationType.SelectedValuePath = "Key";
        }

        private void LoadData()
        {
            if (_plantations.ContainsKey(_editingEntity.PlantationId))
            {
                cboPlantation.SelectedValue = _editingEntity.PlantationId;
            }

            if (_operationTypes.ContainsKey(_editingEntity.OperationTypeId))
            {
                cboOperationType.SelectedValue = _editingEntity.OperationTypeId;
            }

            dpOperationDate.SelectedDate = _editingEntity.OperationDate;
            txtMaterials.Text = _editingEntity.Materials;
            txtRemarks.Text = _editingEntity.Remarks;
        }

        private void SaveData()
        {
            if (cboPlantation.SelectedValue != null)
            {
                _editingEntity.PlantationId = (int)cboPlantation.SelectedValue;
            }

            if (cboOperationType.SelectedValue != null)
            {
                _editingEntity.OperationTypeId = (int)cboOperationType.SelectedValue;
            }

            _editingEntity.OperationDate = dpOperationDate.SelectedDate ?? DateTime.Now;
            _editingEntity.Materials = txtMaterials.Text;
            _editingEntity.Remarks = txtRemarks.Text;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cboPlantation.SelectedValue == null)
                {
                    MessageBox.Show("Выберите плантацию", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cboOperationType.SelectedValue == null)
                {
                    MessageBox.Show("Выберите тип операции", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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