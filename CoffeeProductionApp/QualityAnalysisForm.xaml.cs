using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class QualityAnalysisForm : Window
    {
        private Repository<QualityAnalysis> _repository;
        private QualityAnalysis _editingEntity;
        private bool _isEditMode;
        private Dictionary<int, string> _greenBatches;
        private Dictionary<int, string> _finishedBatches;

        public QualityAnalysisForm(QualityAnalysis entity = null)
        {
            InitializeComponent();
            _repository = new Repository<QualityAnalysis>();
            LoadGreenBatches();
            LoadFinishedBatches();

            cboSampleType.SelectionChanged += cboSampleType_SelectionChanged;

            if (entity != null)
            {
                _isEditMode = true;
                _editingEntity = entity;
                LoadData();
            }
            else
            {
                _isEditMode = false;
                _editingEntity = new QualityAnalysis();
            }
        }

        private void LoadGreenBatches()
        {
            _greenBatches = new Dictionary<int, string>();
            string query = "SELECT ид_партии_зеленого_зерна, номер_партии FROM партия_зеленого_зерна ORDER BY номер_партии";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _greenBatches.Add(Convert.ToInt32(row["ид_партии_зеленого_зерна"]), row["номер_партии"].ToString());
            }

            cboGreenBatch.ItemsSource = _greenBatches;
            cboGreenBatch.DisplayMemberPath = "Value";
            cboGreenBatch.SelectedValuePath = "Key";
        }

        private void LoadFinishedBatches()
        {
            _finishedBatches = new Dictionary<int, string>();
            string query = "SELECT ид_партии_гп, номер_партии FROM партия_готовой_продукции ORDER BY номер_партии";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dt.Rows)
            {
                _finishedBatches.Add(Convert.ToInt32(row["ид_партии_гп"]), row["номер_партии"].ToString());
            }

            cboFinishedBatch.ItemsSource = _finishedBatches;
            cboFinishedBatch.DisplayMemberPath = "Value";
            cboFinishedBatch.SelectedValuePath = "Key";
        }

        private void cboSampleType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSampleType.SelectedItem is ComboBoxItem selected)
            {
                if (selected.Content.ToString() == "Зеленое зерно")
                {
                    cboGreenBatch.IsEnabled = true;
                    cboFinishedBatch.IsEnabled = false;
                    cboFinishedBatch.SelectedValue = null;
                }
                else
                {
                    cboGreenBatch.IsEnabled = false;
                    cboFinishedBatch.IsEnabled = true;
                    cboGreenBatch.SelectedValue = null;
                }
            }
        }

        private void LoadData()
        {
            dpAnalysisDate.SelectedDate = _editingEntity.AnalysisDate;

            if (_editingEntity.SampleType == "Зеленое зерно")
            {
                cboSampleType.SelectedIndex = 0;
                if (_greenBatches.ContainsKey(_editingEntity.GreenBeanBatchId.GetValueOrDefault()))
                {
                    cboGreenBatch.SelectedValue = _editingEntity.GreenBeanBatchId;
                }
            }
            else
            {
                cboSampleType.SelectedIndex = 1;
                if (_finishedBatches.ContainsKey(_editingEntity.FinishedProductBatchId.GetValueOrDefault()))
                {
                    cboFinishedBatch.SelectedValue = _editingEntity.FinishedProductBatchId;
                }
            }

            txtParameters.Text = _editingEntity.Parameters;
            txtCuppingScore.Text = _editingEntity.CuppingScore?.ToString();

            if (!string.IsNullOrEmpty(_editingEntity.Conclusion))
            {
                foreach (ComboBoxItem item in cboConclusion.Items)
                {
                    if (item.Content.ToString() == _editingEntity.Conclusion)
                    {
                        cboConclusion.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void SaveData()
        {
            _editingEntity.AnalysisDate = dpAnalysisDate.SelectedDate ?? DateTime.Now;

            if (cboSampleType.SelectedItem is ComboBoxItem selectedType)
            {
                _editingEntity.SampleType = selectedType.Content.ToString();
            }

            if (_editingEntity.SampleType == "Зеленое зерно" && cboGreenBatch.SelectedValue != null)
            {
                _editingEntity.GreenBeanBatchId = (int)cboGreenBatch.SelectedValue;
                _editingEntity.FinishedProductBatchId = null;
            }
            else if (cboFinishedBatch.SelectedValue != null)
            {
                _editingEntity.FinishedProductBatchId = (int)cboFinishedBatch.SelectedValue;
                _editingEntity.GreenBeanBatchId = null;
            }

            _editingEntity.Parameters = txtParameters.Text;

            if (decimal.TryParse(txtCuppingScore.Text, out decimal score))
            {
                _editingEntity.CuppingScore = score;
            }

            if (cboConclusion.SelectedItem is ComboBoxItem selectedConclusion)
            {
                _editingEntity.Conclusion = selectedConclusion.Content.ToString();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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