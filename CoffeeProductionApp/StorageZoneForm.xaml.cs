using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;
using System.Windows.Controls;
namespace CoffeeProductionApp
{
    public partial class StorageZoneForm : Window
    {
        private Repository<StorageZone> _репозиторий;
        private StorageZone _редактируемаяЗапись;
        private bool _режимРедактирования;
        private Dictionary<int, string> _списокСкладов;

        public StorageZoneForm(StorageZone запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<StorageZone>();
            ЗагрузитьСклады();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new StorageZone();
            }
        }

        private void ЗагрузитьСклады()
        {
            _списокСкладов = new Dictionary<int, string>();
            string запрос = "SELECT ид_склада, название FROM склад ORDER BY название";
            DataTable dt = DatabaseHelper.ExecuteQuery(запрос);

            foreach (DataRow row in dt.Rows)
            {
                _списокСкладов.Add(Convert.ToInt32(row["ид_склада"]), row["название"].ToString());
            }

            СписокСклад.ItemsSource = _списокСкладов;
            СписокСклад.DisplayMemberPath = "Value";
            СписокСклад.SelectedValuePath = "Key";
        }

        private void ЗагрузитьДанные()
        {
            ПолеНазвание.Text = _редактируемаяЗапись.ZoneName;

            if (_списокСкладов.ContainsKey(_редактируемаяЗапись.WarehouseId))
            {
                СписокСклад.SelectedValue = _редактируемаяЗапись.WarehouseId;
            }
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.ZoneName = ПолеНазвание.Text;

            if (СписокСклад.SelectedValue != null)
            {
                _редактируемаяЗапись.WarehouseId = (int)СписокСклад.SelectedValue;
            }
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеНазвание.Text))
                {
                    MessageBox.Show("Введите название зоны", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (СписокСклад.SelectedValue == null)
                {
                    MessageBox.Show("Выберите склад", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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