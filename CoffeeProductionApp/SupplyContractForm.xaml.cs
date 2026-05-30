using System;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;
using System.Windows.Controls;
namespace CoffeeProductionApp
{
    public partial class SupplyContractForm : Window
    {
        private Repository<SupplyContract> _репозиторий;
        private SupplyContract _редактируемаяЗапись;
        private bool _режимРедактирования;

        public SupplyContractForm(SupplyContract запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<SupplyContract>();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new SupplyContract();
            }
        }

        private void ЗагрузитьДанные()
        {
            ПолеНомер.Text = _редактируемаяЗапись.ContractNumber;
            ПолеДата.SelectedDate = _редактируемаяЗапись.ConclusionDate;
            ПолеЮрлицо.Text = _редактируемаяЗапись.LegalEntityName;
            ПолеИНН.Text = _редактируемаяЗапись.Inn;
            ПолеУсловия.Text = _редактируемаяЗапись.PaymentTerms;
            ПолеМинОбъем.Text = _редактируемаяЗапись.MinOrderKg?.ToString();
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.ContractNumber = ПолеНомер.Text;
            _редактируемаяЗапись.ConclusionDate = ПолеДата.SelectedDate ?? DateTime.Now;
            _редактируемаяЗапись.LegalEntityName = ПолеЮрлицо.Text;
            _редактируемаяЗапись.Inn = ПолеИНН.Text;
            _редактируемаяЗапись.PaymentTerms = ПолеУсловия.Text;

            if (decimal.TryParse(ПолеМинОбъем.Text, out decimal минОбъем))
                _редактируемаяЗапись.MinOrderKg = минОбъем;
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеНомер.Text))
                {
                    MessageBox.Show("Введите номер договора", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ПолеЮрлицо.Text))
                {
                    MessageBox.Show("Введите наименование юридического лица", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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