using System;
using System.Windows;
using System.Windows.Controls;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class PlantationForm : Window
    {
        private Repository<Plantation> _репозиторий;
        private Plantation _редактируемаяЗапись;
        private bool _режимРедактирования;

        public PlantationForm(Plantation запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<Plantation>();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new Plantation();
            }
        }

        private void ЗагрузитьДанные()
        {
            ПолеНазвание.Text = _редактируемаяЗапись.Name;
            ПолеСтрана.Text = _редактируемаяЗапись.Country;
            ПолеРегион.Text = _редактируемаяЗапись.Region;
            ПолеКоординаты.Text = _редактируемаяЗапись.Coordinates;
            ПолеПлощадь.Text = _редактируемаяЗапись.Area.ToString();
            ПолеДатаПосадки.SelectedDate = _редактируемаяЗапись.PlantingDate;

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.CoffeeVariety))
            {
                foreach (ComboBoxItem item in СписокСорт.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.CoffeeVariety)
                    {
                        СписокСорт.SelectedItem = item;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.SoilType))
            {
                foreach (ComboBoxItem item in СписокТипПочвы.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.SoilType)
                    {
                        СписокТипПочвы.SelectedItem = item;
                        break;
                    }
                }

                if (СписокТипПочвы.SelectedItem == null)
                {
                    СписокТипПочвы.Text = _редактируемаяЗапись.SoilType;
                }
            }

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.IrrigationSystem))
            {
                foreach (ComboBoxItem item in СписокСистемаПолива.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.IrrigationSystem)
                    {
                        СписокСистемаПолива.SelectedItem = item;
                        break;
                    }
                }

                if (СписокСистемаПолива.SelectedItem == null)
                {
                    СписокСистемаПолива.Text = _редактируемаяЗапись.IrrigationSystem;
                }
            }
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.Name = ПолеНазвание.Text;
            _редактируемаяЗапись.Country = ПолеСтрана.Text;
            _редактируемаяЗапись.Region = ПолеРегион.Text;
            _редактируемаяЗапись.Coordinates = ПолеКоординаты.Text;

            if (decimal.TryParse(ПолеПлощадь.Text, out decimal площадь))
                _редактируемаяЗапись.Area = площадь;

            _редактируемаяЗапись.PlantingDate = ПолеДатаПосадки.SelectedDate;

            if (СписокСорт.SelectedItem is ComboBoxItem выбранныйСорт)
                _редактируемаяЗапись.CoffeeVariety = выбранныйСорт.Content.ToString();

            _редактируемаяЗапись.SoilType = СписокТипПочвы.Text;
            _редактируемаяЗапись.IrrigationSystem = СписокСистемаПолива.Text;
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеНазвание.Text))
                {
                    MessageBox.Show("Введите название плантации", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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