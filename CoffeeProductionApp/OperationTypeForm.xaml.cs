using System;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;
using System.Windows.Controls;
namespace CoffeeProductionApp
{
    public partial class OperationTypeForm : Window
    {
        private Repository<OperationType> _репозиторий;
        private OperationType _редактируемаяЗапись;
        private bool _режимРедактирования;

        public OperationTypeForm(OperationType запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<OperationType>();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new OperationType();
            }
        }

        private void ЗагрузитьДанные()
        {
            ПолеНазвание.Text = _редактируемаяЗапись.Name;
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.Name = ПолеНазвание.Text;
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ПолеНазвание.Text))
                {
                    MessageBox.Show("Введите название операции", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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