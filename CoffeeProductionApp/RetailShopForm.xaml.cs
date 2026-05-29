using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeProductionApp
{
    public partial class RetailShopForm : Window
    {
        private Repository<RetailShop> _репозиторий;
        private RetailShop _редактируемаяЗапись;
        private bool _режимРедактирования;

        public RetailShopForm(RetailShop запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<RetailShop>();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new RetailShop();
            }
        }

        private void ЗагрузитьДанные()
        {
            ПолеНазвание.Text = _редактируемаяЗапись.Name;
            ПолеАдрес.Text = _редактируемаяЗапись.Address;
            ПолеТелефон.Text = _редактируемаяЗапись.Phone;

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.Status))
            {
                foreach (ComboBoxItem item in СписокСтатус.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.Status)
                    {
                        СписокСтатус.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.Name = ПолеНазвание.Text;
            _редактируемаяЗапись.Address = ПолеАдрес.Text;
            _редактируемаяЗапись.Phone = ПолеТелефон.Text;

            if (СписокСтатус.SelectedItem is ComboBoxItem выбранныйСтатус)
                _редактируемаяЗапись.Status = выбранныйСтатус.Content.ToString();
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеНазвание.Text))
                {
                    MessageBox.Show("Введите название магазина", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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