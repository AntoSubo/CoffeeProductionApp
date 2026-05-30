using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class ProductionOrderForm : Window
    {
        private Repository<ProductionOrder> _репозиторий;
        private ProductionOrder _редактируемаяЗапись;
        private bool _режимРедактирования;
        private Dictionary<int, string> _списокПартийЗерна;
        private Dictionary<int, string> _списокПрофилей;

        public ProductionOrderForm(ProductionOrder запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<ProductionOrder>();
            ЗагрузитьПартииЗерна();
            ЗагрузитьПрофили();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new ProductionOrder();
            }
        }

        private void ЗагрузитьПартииЗерна()
        {
            _списокПартийЗерна = new Dictionary<int, string>();
            string запрос = "SELECT ид_партии_зеленого_зерна, номер_партии FROM партия_зеленого_зерна ORDER BY номер_партии";
            DataTable dt = DatabaseHelper.ExecuteQuery(запрос);

            foreach (DataRow row in dt.Rows)
            {
                _списокПартийЗерна.Add(Convert.ToInt32(row["ид_партии_зеленого_зерна"]), row["номер_партии"].ToString());
            }

            СписокПартияЗерна.ItemsSource = _списокПартийЗерна;
            СписокПартияЗерна.DisplayMemberPath = "Value";
            СписокПартияЗерна.SelectedValuePath = "Key";
        }

        private void ЗагрузитьПрофили()
        {
            _списокПрофилей = new Dictionary<int, string>();
            string запрос = "SELECT ид_профиля, название FROM профиль_обжарки ORDER BY название";
            DataTable dt = DatabaseHelper.ExecuteQuery(запрос);

            foreach (DataRow row in dt.Rows)
            {
                _списокПрофилей.Add(Convert.ToInt32(row["ид_профиля"]), row["название"].ToString());
            }

            СписокПрофиль.ItemsSource = _списокПрофилей;
            СписокПрофиль.DisplayMemberPath = "Value";
            СписокПрофиль.SelectedValuePath = "Key";
        }

        private void ЗагрузитьДанные()
        {
            ПолеНомерЗаказа.Text = _редактируемаяЗапись.OrderNumber;
            ПолеДатаСоздания.SelectedDate = _редактируемаяЗапись.CreationDate;

            if (_списокПартийЗерна.ContainsKey(_редактируемаяЗапись.GreenBeanBatchId))
            {
                СписокПартияЗерна.SelectedValue = _редактируемаяЗапись.GreenBeanBatchId;
            }

            if (_списокПрофилей.ContainsKey(_редактируемаяЗапись.RoastingProfileId))
            {
                СписокПрофиль.SelectedValue = _редактируемаяЗапись.RoastingProfileId;
            }

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.RoastLevel))
            {
                foreach (ComboBoxItem item in СписокСтепеньОбжарки.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.RoastLevel)
                    {
                        СписокСтепеньОбжарки.SelectedItem = item;
                        break;
                    }
                }
            }

            ПолеЦелевойВкус.Text = _редактируемаяЗапись.TargetFlavor;
            ПолеПлановаяДата.SelectedDate = _редактируемаяЗапись.PlannedCompletionDate;
            ПолеПлановыйВес.Text = _редактируемаяЗапись.PlannedWeightKg.ToString();
            ПолеФактическийВес.Text = _редактируемаяЗапись.ActualWeightKg?.ToString();
            ПолеДатаВыполнения.SelectedDate = _редактируемаяЗапись.CompletionDate;
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.OrderNumber = ПолеНомерЗаказа.Text;
            _редактируемаяЗапись.CreationDate = ПолеДатаСоздания.SelectedDate ?? DateTime.Now;

            if (СписокПартияЗерна.SelectedValue != null)
            {
                _редактируемаяЗапись.GreenBeanBatchId = (int)СписокПартияЗерна.SelectedValue;
            }

            if (СписокПрофиль.SelectedValue != null)
            {
                _редактируемаяЗапись.RoastingProfileId = (int)СписокПрофиль.SelectedValue;
            }

            if (СписокСтепеньОбжарки.SelectedItem is ComboBoxItem выбраннаяСтепень)
            {
                _редактируемаяЗапись.RoastLevel = выбраннаяСтепень.Content.ToString();
            }

            _редактируемаяЗапись.TargetFlavor = ПолеЦелевойВкус.Text;
            _редактируемаяЗапись.PlannedCompletionDate = ПолеПлановаяДата.SelectedDate;

            if (decimal.TryParse(ПолеПлановыйВес.Text, out decimal плановыйВес))
            {
                _редактируемаяЗапись.PlannedWeightKg = плановыйВес;
            }

            if (decimal.TryParse(ПолеФактическийВес.Text, out decimal фактическийВес))
            {
                _редактируемаяЗапись.ActualWeightKg = фактическийВес;
            }

            _редактируемаяЗапись.CompletionDate = ПолеДатаВыполнения.SelectedDate;
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеНомерЗаказа.Text))
                {
                    MessageBox.Show("Введите номер заказа", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (СписокПартияЗерна.SelectedValue == null)
                {
                    MessageBox.Show("Выберите партию зеленого зерна", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (СписокПрофиль.SelectedValue == null)
                {
                    MessageBox.Show("Выберите профиль обжарки", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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