using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class HarvestForm : Window
    {
        private Repository<Harvest> _репозиторий;
        private Harvest _редактируемаяЗапись;
        private bool _режимРедактирования;
        private Dictionary<int, string> _списокПлантаций;

        public HarvestForm(Harvest запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<Harvest>();
            ЗагрузитьПлантации();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new Harvest();
            }
        }

        private void ЗагрузитьПлантации()
        {
            _списокПлантаций = new Dictionary<int, string>();
            string запрос = "SELECT ид_плантации, название FROM плантация ORDER BY название";
            DataTable dt = DatabaseHelper.ExecuteQuery(запрос);

            foreach (DataRow row in dt.Rows)
            {
                _списокПлантаций.Add(Convert.ToInt32(row["ид_плантации"]), row["название"].ToString());
            }

            СписокПлантация.ItemsSource = _списокПлантаций;
            СписокПлантация.DisplayMemberPath = "Value";
            СписокПлантация.SelectedValuePath = "Key";
        }

        private void ЗагрузитьДанные()
        {
            if (_списокПлантаций.ContainsKey(_редактируемаяЗапись.PlantationId))
            {
                СписокПлантация.SelectedValue = _редактируемаяЗапись.PlantationId;
            }

            ПолеДатаСбора.SelectedDate = _редактируемаяЗапись.HarvestDate;

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.ProcessingMethod))
            {
                foreach (ComboBoxItem item in СписокМетодОбработки.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.ProcessingMethod)
                    {
                        СписокМетодОбработки.SelectedItem = item;
                        break;
                    }
                }
            }

            ПолеВесЯгод.Text = _редактируемаяЗапись.BerriesWeightKg.ToString();
            ПолеВлажность.Text = _редактируемаяЗапись.DryingHumidity.ToString();
            ПолеВесЗерна.Text = _редактируемаяЗапись.GreenBeansWeightKg.ToString();
            ПолеДефекты.Text = _редактируемаяЗапись.DefectPercentage.ToString();
        }

        private void СохранитьДанные()
        {
            if (СписокПлантация.SelectedValue != null)
            {
                _редактируемаяЗапись.PlantationId = (int)СписокПлантация.SelectedValue;
            }

            _редактируемаяЗапись.HarvestDate = ПолеДатаСбора.SelectedDate ?? DateTime.Now;

            if (СписокМетодОбработки.SelectedItem is ComboBoxItem выбранныйМетод)
            {
                _редактируемаяЗапись.ProcessingMethod = выбранныйМетод.Content.ToString();
            }

            if (decimal.TryParse(ПолеВесЯгод.Text, out decimal весЯгод))
            {
                _редактируемаяЗапись.BerriesWeightKg = весЯгод;
            }

            if (decimal.TryParse(ПолеВлажность.Text, out decimal влажность))
            {
                _редактируемаяЗапись.DryingHumidity = влажность;
            }

            if (decimal.TryParse(ПолеВесЗерна.Text, out decimal весЗерна))
            {
                _редактируемаяЗапись.GreenBeansWeightKg = весЗерна;
            }

            if (decimal.TryParse(ПолеДефекты.Text, out decimal дефекты))
            {
                _редактируемаяЗапись.DefectPercentage = дефекты;
            }
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (СписокПлантация.SelectedValue == null)
                {
                    MessageBox.Show("Выберите плантацию", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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