using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;
using System.Windows.Controls;
namespace CoffeeProductionApp
{
    public partial class StorageCellForm : Window
    {
        private Repository<StorageCell> _репозиторий;
        private StorageCell _редактируемаяЗапись;
        private bool _режимРедактирования;
        private Dictionary<int, string> _списокЗон;

        public StorageCellForm(StorageCell запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<StorageCell>();
            ЗагрузитьЗоны();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new StorageCell();
            }
        }

        private void ЗагрузитьЗоны()
        {
            _списокЗон = new Dictionary<int, string>();
            string запрос = @"
                SELECT зона_хранения.ид_зоны, зона_хранения.название_зоны, склад.название AS Склад
                FROM зона_хранения
                JOIN склад ON зона_хранения.склад_вк = склад.ид_склада
                ORDER BY склад.название, зона_хранения.название_зоны";
            DataTable dt = DatabaseHelper.ExecuteQuery(запрос);

            foreach (DataRow row in dt.Rows)
            {
                string название = $"{row["Склад"]} - {row["название_зоны"]}";
                _списокЗон.Add(Convert.ToInt32(row["ид_зоны"]), название);
            }

            СписокЗона.ItemsSource = _списокЗон;
            СписокЗона.DisplayMemberPath = "Value";
            СписокЗона.SelectedValuePath = "Key";
        }

        private void ЗагрузитьДанные()
        {
            ПолеКод.Text = _редактируемаяЗапись.CellCode;

            if (_списокЗон.ContainsKey(_редактируемаяЗапись.ZoneId))
            {
                СписокЗона.SelectedValue = _редактируемаяЗапись.ZoneId;
            }
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.CellCode = ПолеКод.Text;

            if (СписокЗона.SelectedValue != null)
            {
                _редактируемаяЗапись.ZoneId = (int)СписокЗона.SelectedValue;
            }
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеКод.Text))
                {
                    MessageBox.Show("Введите код ячейки", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (СписокЗона.SelectedValue == null)
                {
                    MessageBox.Show("Выберите зону хранения", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                СохранитьДанные();

                if (_режимРедактирования)
                    _репозиторий.Update(_редактируемаяЗапись);
                else
                    _репозиторий.Add(_редактируемаяЗапись);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void КнопкаОтмена_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}