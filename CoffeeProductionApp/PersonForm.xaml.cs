using System;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class PersonForm : Window
    {
        private Repository<Person> _репозиторий;
        private Person _редактируемаяЗапись;
        private bool _режимРедактирования;

        public PersonForm(Person запись = null)
        {
            InitializeComponent();
            _репозиторий = new Repository<Person>();

            if (запись != null)
            {
                _режимРедактирования = true;
                _редактируемаяЗапись = запись;
                ЗагрузитьДанные();
            }
            else
            {
                _режимРедактирования = false;
                _редактируемаяЗапись = new Person();
            }
        }

        private void ЗагрузитьДанные()
        {
            ПолеФИО.Text = _редактируемаяЗапись.FullName;
            ПолеТелефон.Text = _редактируемаяЗапись.Phone;
            ПолеEmail.Text = _редактируемаяЗапись.Email;
            ПолеДатаРождения.SelectedDate = _редактируемаяЗапись.BirthDate;
            ПолеПаспорт.Text = _редактируемаяЗапись.PassportData;
        }

        private void СохранитьДанные()
        {
            _редактируемаяЗапись.FullName = ПолеФИО.Text;
            _редактируемаяЗапись.Phone = ПолеТелефон.Text;
            _редактируемаяЗапись.Email = ПолеEmail.Text;
            _редактируемаяЗапись.BirthDate = ПолеДатаРождения.SelectedDate;
            _редактируемаяЗапись.PassportData = ПолеПаспорт.Text;
        }

        private void КнопкаСохранить_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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