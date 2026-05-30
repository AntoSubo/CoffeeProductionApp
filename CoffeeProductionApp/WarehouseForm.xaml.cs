using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;
using System;
using System.Windows;
using System.Windows.Controls;


namespace CoffeeProductionApp
{
    public partial class WarehouseForm : Window
    {
        private Repository<Warehouse> _репозиторий;
        private Warehouse _редактируемаяЗапись;
        private bool _режимРедактирования;

        public WarehouseForm(Warehouse запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<Warehouse>();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new Warehouse();
            }
        }

        private void ЗагрузитьДанные()
        {
            ПолеНазвание.Text = _редактируемаяЗапись.Name;
            ПолеАдрес.Text = _редактируемаяЗапись.Address;

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.WarehouseType))
            {
                foreach (ComboBoxItem item in СписокТип.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.WarehouseType)
                    {
                        СписокТип.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.Name = ПолеНазвание.Text;

            if (СписокТип.SelectedItem is ComboBoxItem выбранныйТип)
                _редактируемаяЗапись.WarehouseType = выбранныйТип.Content.ToString();

            _редактируемаяЗапись.Address = ПолеАдрес.Text;
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеНазвание.Text))
                {
                    MessageBox.Show("Введите название склада", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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