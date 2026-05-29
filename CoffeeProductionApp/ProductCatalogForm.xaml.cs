using System;
using System.Windows;
using System.Windows.Controls;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class ProductCatalogForm : Window
    {
        private Repository<ProductCatalog> _репозиторий;
        private ProductCatalog _редактируемаяЗапись;
        private bool _режимРедактирования;

        public ProductCatalogForm(ProductCatalog запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<ProductCatalog>();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new ProductCatalog();
            }
        }

        private void ЗагрузитьДанные()
        {
            ПолеАртикул.Text = _редактируемаяЗапись.Sku;
            ПолеНаименование.Text = _редактируемаяЗапись.TradeName;

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.Variety))
            {
                foreach (ComboBoxItem item in СписокСорт.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.Variety)
                    {
                        СписокСорт.SelectedItem = item;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.CountryOfOrigin))
            {
                foreach (ComboBoxItem item in СписокСтрана.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.CountryOfOrigin)
                    {
                        СписокСтрана.SelectedItem = item;
                        break;
                    }
                }
            }

            ПолеВысота.Text = _редактируемаяЗапись.GrowingAltitude?.ToString();

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.RoastLevel))
            {
                foreach (ComboBoxItem item in СписокОбжарка.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.RoastLevel)
                    {
                        СписокОбжарка.SelectedItem = item;
                        break;
                    }
                }
            }

            ПолеВкусовыеНоты.Text = _редактируемаяЗапись.FlavorNotes;

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.DefaultPackageType))
            {
                foreach (ComboBoxItem item in СписокУпаковка.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.DefaultPackageType)
                    {
                        СписокУпаковка.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.Sku = ПолеАртикул.Text;
            _редактируемаяЗапись.TradeName = ПолеНаименование.Text;

            if (СписокСорт.SelectedItem is ComboBoxItem выбранныйСорт)
                _редактируемаяЗапись.Variety = выбранныйСорт.Content.ToString();

            if (СписокСтрана.SelectedItem is ComboBoxItem выбраннаяСтрана)
                _редактируемаяЗапись.CountryOfOrigin = выбраннаяСтрана.Content.ToString();

            if (int.TryParse(ПолеВысота.Text, out int высота))
                _редактируемаяЗапись.GrowingAltitude = высота;

            if (СписокОбжарка.SelectedItem is ComboBoxItem выбраннаяОбжарка)
                _редактируемаяЗапись.RoastLevel = выбраннаяОбжарка.Content.ToString();

            _редактируемаяЗапись.FlavorNotes = ПолеВкусовыеНоты.Text;

            if (СписокУпаковка.SelectedItem is ComboBoxItem выбраннаяУпаковка)
                _редактируемаяЗапись.DefaultPackageType = выбраннаяУпаковка.Content.ToString();
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеАртикул.Text))
                {
                    MessageBox.Show("Введите артикул", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ПолеНаименование.Text))
                {
                    MessageBox.Show("Введите наименование", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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