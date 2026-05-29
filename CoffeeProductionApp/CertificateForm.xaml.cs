using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeProductionApp
{
    public partial class CertificateForm : Window
    {
        private Repository<Certificate> _репозиторий;
        private Certificate _редактируемаяЗапись;
        private bool _режимРедактирования;
        private Dictionary<int, string> _списокПродукции;

        public CertificateForm(Certificate запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<Certificate>();
            ЗагрузитьПродукцию();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new Certificate();
            }
        }

        private void ЗагрузитьПродукцию()
        {
            _списокПродукции = new Dictionary<int, string>();
            string запрос = "SELECT ид_справочника, торговое_наименование FROM готовая_продукция_справочник ORDER BY торговое_наименование";
            DataTable dt = DatabaseHelper.ExecuteQuery(запрос);

            foreach (DataRow row in dt.Rows)
            {
                _списокПродукции.Add(Convert.ToInt32(row["ид_справочника"]), row["торговое_наименование"].ToString());
            }

            СписокПродукция.ItemsSource = _списокПродукции;
            СписокПродукция.DisplayMemberPath = "Value";
            СписокПродукция.SelectedValuePath = "Key";
        }

        private void ЗагрузитьДанные()
        {
            ПолеНомер.Text = _редактируемаяЗапись.CertificateNumber;
            ПолеДатаВыдачи.SelectedDate = _редактируемаяЗапись.IssueDate;
            ПолеСрокДействия.SelectedDate = _редактируемаяЗапись.ExpiryDate;

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.CertificationBody))
            {
                foreach (ComboBoxItem item in СписокОрган.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.CertificationBody)
                    {
                        СписокОрган.SelectedItem = item;
                        break;
                    }
                }
            }

            if (_списокПродукции.ContainsKey(_редактируемаяЗапись.ProductCatalogId))
            {
                СписокПродукция.SelectedValue = _редактируемаяЗапись.ProductCatalogId;
            }

            if (!string.IsNullOrEmpty(_редактируемаяЗапись.Standard))
            {
                foreach (ComboBoxItem item in СписокСтандарт.Items)
                {
                    if (item.Content.ToString() == _редактируемаяЗапись.Standard)
                    {
                        СписокСтандарт.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.CertificateNumber = ПолеНомер.Text;
            _редактируемаяЗапись.IssueDate = ПолеДатаВыдачи.SelectedDate ?? DateTime.Now;
            _редактируемаяЗапись.ExpiryDate = ПолеСрокДействия.SelectedDate;

            if (СписокОрган.SelectedItem is ComboBoxItem выбранныйОрган)
                _редактируемаяЗапись.CertificationBody = выбранныйОрган.Content.ToString();

            if (СписокПродукция.SelectedValue != null)
                _редактируемаяЗапись.ProductCatalogId = (int)СписокПродукция.SelectedValue;

            if (СписокСтандарт.SelectedItem is ComboBoxItem выбранныйСтандарт)
                _редактируемаяЗапись.Standard = выбранныйСтандарт.Content.ToString();
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеНомер.Text))
                {
                    MessageBox.Show("Введите номер сертификата", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (СписокПродукция.SelectedValue == null)
                {
                    MessageBox.Show("Выберите продукцию", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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