using System;
using System.Data;
using System.Windows;
using CoffeeProductionApp.DAL;
using CoffeeProductionApp.Models;

namespace CoffeeProductionApp
{
    public partial class MainWindow : Window
    {
        private Repository<Plantation> _плантацииРепозиторий;
        private Repository<Person> _сотрудникиРепозиторий;
        private Repository<Harvest> _урожайРепозиторий;
        private Repository<GreenBeanBatch> _зерноРепозиторий;
        private Repository<ProductionOrder> _заказыРепозиторий;
        private Repository<ProductCatalog> _продукцияРепозиторий;
        private Repository<RoastingProfile> _профилиРепозиторий;
        private Repository<SupplyContract> _договорыРепозиторий;
        private Repository<B2BOrder> _заказыB2BРепозиторий;
        private Repository<RetailShop> _магазиныРепозиторий;

        private string _текущаяТаблица;

        public MainWindow()
        {
            InitializeComponent();

            _плантацииРепозиторий = new Repository<Plantation>();
            _сотрудникиРепозиторий = new Repository<Person>();
            _урожайРепозиторий = new Repository<Harvest>();
            _зерноРепозиторий = new Repository<GreenBeanBatch>();
            _заказыРепозиторий = new Repository<ProductionOrder>();
            _продукцияРепозиторий = new Repository<ProductCatalog>();
            _профилиРепозиторий = new Repository<RoastingProfile>();
            _договорыРепозиторий = new Repository<SupplyContract>();
            _заказыB2BРепозиторий = new Repository<B2BOrder>();
            _магазиныРепозиторий = new Repository<RetailShop>();

            ОткрытьПлантации_Click(null, null);
        }

        private int ПолучитьИдИзВыделеннойСтроки()
        {
            if (ТаблицаДанных.SelectedItem == null)
                return 0;

            var строка = (DataRowView)ТаблицаДанных.SelectedItem;

            foreach (DataColumn колонка in строка.Row.Table.Columns)
            {
                string имя = колонка.ColumnName.ToLower();
                if (имя == "ид" || имя.StartsWith("ид_"))
                {
                    return Convert.ToInt32(строка[колонка.ColumnName]);
                }
            }

            return 0;
        }

        private void ОткрытьПлантации_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "Плантации";
            string запрос = @"
                SELECT 
                    ид_плантации AS ид,
                    название AS Название,
                    страна AS Страна,
                    регион AS Регион,
                    координаты AS Координаты,
                    площадь AS Площадь,
                    дата_посадки AS ДатаПосадки,
                    сорт_кофе AS СортКофе,
                    тип_почвы AS ТипПочвы,
                    система_полива AS СистемаПолива
                FROM плантация
                ORDER BY ид_плантации";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьСотрудники_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "Сотрудники";
            string запрос = @"
                SELECT 
                    ид_человека AS ид,
                    фио AS ФИО,
                    телефон AS Телефон,
                    электронная_почта AS Email,
                    дата_рождения AS ДатаРождения,
                    паспортные_данные AS Паспорт
                FROM человек
                ORDER BY фио";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьТипыОпераций_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ТипыОпераций";
            string запрос = @"
                SELECT 
                    ид_типа_операции AS ид,
                    название AS Название
                FROM тип_операции
                ORDER BY ид_типа_операции";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьПрофилиОбжарки_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ПрофилиОбжарки";
            string запрос = @"
                SELECT 
                    ид_профиля AS ид,
                    название AS Название,
                    описание AS Описание
                FROM профиль_обжарки
                ORDER BY ид_профиля";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьПродукцию_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "Продукция";
            string запрос = @"
                SELECT 
                    ид_справочника AS ид,
                    артикул AS Артикул,
                    торговое_наименование AS Наименование,
                    сорт AS Сорт,
                    степень_обжарки AS СтепеньОбжарки
                FROM готовая_продукция_справочник
                ORDER BY ид_справочника";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьМагазины_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "Магазины";
            string запрос = @"
                SELECT 
                    ид_магазина AS ид,
                    название AS Название,
                    адрес AS Адрес,
                    телефон AS Телефон,
                    статус AS Статус
                FROM розничный_магазин
                ORDER BY ид_магазина";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьСклады_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "Склады";
            string запрос = @"
                SELECT 
                    ид_склада AS ид,
                    название AS Название,
                    тип_склада AS Тип,
                    адрес AS Адрес
                FROM склад
                ORDER BY ид_склада";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьАгроОперации_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "АгроОперации";
            string запрос = @"
                SELECT 
                    ид_агрооперации AS ид,
                    плантация_вк AS ИдПлантации,
                    тип_операции_вк AS ИдТипа,
                    дата_операции AS Дата,
                    материалы AS Материалы
                FROM агротехническая_операция
                ORDER BY ид_агрооперации";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьУрожай_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "Урожай";
            string запрос = @"
                SELECT 
                    урожай.ид_урожая AS ид,
                    плантация.название AS Плантация,
                    урожай.дата_сбора AS ДатаСбора,
                    урожай.метод_обработки AS МетодОбработки,
                    урожай.вес_ягод_кг AS ВесЯгодКг,
                    урожай.вес_зеленого_зерна_кг AS ВесЗернаКг,
                    урожай.процент_дефектов AS Дефекты
                FROM урожай
                JOIN плантация ON урожай.плантация_вк = плантация.ид_плантации
                ORDER BY урожай.ид_урожая";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьПартииЗерна_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ПартииЗерна";
            string запрос = @"
                SELECT 
                    ид_партии_зеленого_зерна AS ид,
                    номер_партии AS НомерПартии,
                    дата_приемки AS ДатаПриемки,
                    сорт AS Сорт,
                    вес_нетто_кг AS ВесКг,
                    влажность_процент AS Влажность
                FROM партия_зеленого_зерна
                ORDER BY ид_партии_зеленого_зерна";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьПроизводствоЗаказы_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ПроизводственныеЗаказы";
            string запрос = @"
                SELECT 
                    ид_заказа AS ид,
                    номер_заказа AS НомерЗаказа,
                    дата_создания AS ДатаСоздания,
                    степень_обжарки AS СтепеньОбжарки,
                    плановый_вес_кг AS ПлановыйВесКг,
                    дата_выполнения AS ДатаВыполнения
                FROM производственный_заказ
                ORDER BY ид_заказа";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьПартииГП_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ПартииГП";
            string запрос = @"
                SELECT 
                    ид_партии_гп AS ид,
                    номер_партии AS НомерПартии,
                    дата_выпуска AS ДатаВыпуска,
                    количество_упаковок AS Количество,
                    розничная_цена AS РозничнаяЦена
                FROM партия_готовой_продукции
                ORDER BY ид_партии_гп";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьПоступление_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ПоступлениеВМагазин";
            string запрос = @"
                SELECT 
                    ид_поступления AS ид,
                    дата AS Дата,
                    магазин_вк AS ИдМагазина,
                    партия_гп_вк AS ИдПартии,
                    количество AS Количество
                FROM поступление_в_магазин
                ORDER BY ид_поступления";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьДоговоры_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "Договоры";
            string запрос = @"
                SELECT 
                    ид_договора AS ид,
                    номер_договора AS НомерДоговора,
                    дата_заключения AS ДатаЗаключения,
                    наименование_юрлица AS Покупатель
                FROM договор_поставки
                ORDER BY ид_договора";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьЗаказыB2B_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ЗаказыB2B";
            string запрос = @"
                SELECT 
                    ид_заказа_в2в AS ид,
                    номер_заказа AS НомерЗаказа,
                    дата_создания AS ДатаСоздания,
                    статус AS Статус
                FROM заказ_в2в
                ORDER BY ид_заказа_в2в";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьАнализы_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "АнализыКачества";
            string запрос = @"
                SELECT 
                    ид_анализа AS ид,
                    дата_анализа AS ДатаАнализа,
                    тип_образца AS ТипОбразца,
                    заключение AS Заключение
                FROM анализ_качества
                ORDER BY ид_анализа";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьСертификаты_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "Сертификаты";
            string запрос = @"
                SELECT 
                    ид_сертификата AS ид,
                    номер_сертификата AS НомерСертификата,
                    дата_выдачи AS ДатаВыдачи,
                    срок_действия AS СрокДействия
                FROM сертификат_соответствия
                ORDER BY ид_сертификата";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОтчетОстаткиЗерна_Click(object sender, RoutedEventArgs e)
        {
            string запрос = @"
                SELECT 
                    партия_зеленого_зерна.номер_партии AS НомерПартии,
                    партия_зеленого_зерна.сорт AS Сорт,
                    партия_зеленого_зерна.вес_нетто_кг AS ОстатокКг,
                    ячейка_хранения.код_ячейки AS Ячейка,
                    склад.название AS Склад
                FROM партия_зеленого_зерна
                LEFT JOIN ячейка_хранения ON партия_зеленого_зерна.ячейка_вк = ячейка_хранения.ид_ячейки
                LEFT JOIN зона_хранения ON ячейка_хранения.зона_вк = зона_хранения.ид_зоны
                LEFT JOIN склад ON зона_хранения.склад_вк = склад.ид_склада
                WHERE партия_зеленого_зерна.вес_нетто_кг > 0
                ORDER BY склад.название, ячейка_хранения.код_ячейки";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОтчетСебестоимость_Click(object sender, RoutedEventArgs e)
        {
            string запрос = @"
                SELECT 
                    партия_готовой_продукции.номер_партии AS НомерПартии,
                    готовая_продукция_справочник.торговое_наименование AS Продукт,
                    партия_готовой_продукции.себестоимость_единицы AS Себестоимость,
                    партия_готовой_продукции.розничная_цена AS РозничнаяЦена,
                    партия_готовой_продукции.оптовая_цена AS ОптоваяЦена
                FROM партия_готовой_продукции
                JOIN готовая_продукция_справочник ON партия_готовой_продукции.справочник_гп_вк = готовая_продукция_справочник.ид_справочника
                ORDER BY партия_готовой_продукции.дата_выпуска DESC";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОтчетВыполнениеЗаказов_Click(object sender, RoutedEventArgs e)
        {
            string запрос = @"
                SELECT 
                    номер_заказа AS НомерЗаказа,
                    дата_создания AS ДатаСоздания,
                    плановый_вес_кг AS ПлановыйВес,
                    фактический_вес_кг AS ФактическийВес,
                    CASE 
                        WHEN дата_выполнения IS NOT NULL THEN 'Выполнен'
                        ELSE 'В работе'
                    END AS Статус
                FROM производственный_заказ
                ORDER BY дата_создания DESC";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void КнопкаДобавить_Click(object sender, RoutedEventArgs e)
        {
            if (_текущаяТаблица == "Плантации")
            {
                var форма = new PlantationForm();
                if (форма.ShowDialog() == true)
                    ОткрытьПлантации_Click(null, null);
            }
            else if (_текущаяТаблица == "Сотрудники")
            {
                var форма = new PersonForm();
                if (форма.ShowDialog() == true)
                    ОткрытьСотрудники_Click(null, null);
            }
            else
            {
                MessageBox.Show($"Добавление для раздела \"{_текущаяТаблица}\" будет реализовано позже", "В разработке", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void КнопкаИзменить_Click(object sender, RoutedEventArgs e)
        {
            if (ТаблицаДанных.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для изменения", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int ид = ПолучитьИдИзВыделеннойСтроки();
            if (ид == 0)
            {
                MessageBox.Show("Не удалось определить ID записи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_текущаяТаблица == "Плантации")
            {
                var запись = _плантацииРепозиторий.GetById(ид);
                if (запись != null)
                {
                    var форма = new PlantationForm(запись);
                    if (форма.ShowDialog() == true)
                        ОткрытьПлантации_Click(null, null);
                }
            }
            else if (_текущаяТаблица == "Сотрудники")
            {
                var запись = _сотрудникиРепозиторий.GetById(ид);
                if (запись != null)
                {
                    var форма = new PersonForm(запись);
                    if (форма.ShowDialog() == true)
                        ОткрытьСотрудники_Click(null, null);
                }
            }
            else
            {
                MessageBox.Show($"Редактирование для раздела \"{_текущаяТаблица}\" будет реализовано позже", "В разработке", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void КнопкаУдалить_Click(object sender, RoutedEventArgs e)
        {
            if (ТаблицаДанных.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var результат = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (результат == MessageBoxResult.Yes)
            {
                try
                {
                    int ид = ПолучитьИдИзВыделеннойСтроки();
                    if (ид == 0)
                    {
                        MessageBox.Show("Не удалось определить ID записи для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    switch (_текущаяТаблица)
                    {
                        case "Плантации":
                            _плантацииРепозиторий.Delete(ид);
                            break;
                        case "Сотрудники":
                            _сотрудникиРепозиторий.Delete(ид);
                            break;
                        case "ТипыОпераций":
                            new Repository<OperationType>().Delete(ид);
                            break;
                        case "ПрофилиОбжарки":
                            _профилиРепозиторий.Delete(ид);
                            break;
                        case "Продукция":
                            _продукцияРепозиторий.Delete(ид);
                            break;
                        case "Магазины":
                            _магазиныРепозиторий.Delete(ид);
                            break;
                        case "Склады":
                            new Repository<Warehouse>().Delete(ид);
                            break;
                        default:
                            MessageBox.Show($"Удаление для раздела \"{_текущаяТаблица}\" не реализовано", "В разработке", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                    }

                    КнопкаОбновить_Click(null, null);
                    MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void КнопкаОбновить_Click(object sender, RoutedEventArgs e)
        {
            switch (_текущаяТаблица)
            {
                case "Плантации": ОткрытьПлантации_Click(null, null); break;
                case "Сотрудники": ОткрытьСотрудники_Click(null, null); break;
                case "ТипыОпераций": ОткрытьТипыОпераций_Click(null, null); break;
                case "ПрофилиОбжарки": ОткрытьПрофилиОбжарки_Click(null, null); break;
                case "Продукция": ОткрытьПродукцию_Click(null, null); break;
                case "Магазины": ОткрытьМагазины_Click(null, null); break;
                case "Склады": ОткрытьСклады_Click(null, null); break;
                case "АгроОперации": ОткрытьАгроОперации_Click(null, null); break;
                case "Урожай": ОткрытьУрожай_Click(null, null); break;
                case "ПартииЗерна": ОткрытьПартииЗерна_Click(null, null); break;
                case "ПроизводственныеЗаказы": ОткрытьПроизводствоЗаказы_Click(null, null); break;
                case "ПартииГП": ОткрытьПартииГП_Click(null, null); break;
                case "ПоступлениеВМагазин": ОткрытьПоступление_Click(null, null); break;
                case "Договоры": ОткрытьДоговоры_Click(null, null); break;
                case "ЗаказыB2B": ОткрытьЗаказыB2B_Click(null, null); break;
                case "АнализыКачества": ОткрытьАнализы_Click(null, null); break;
                case "Сертификаты": ОткрытьСертификаты_Click(null, null); break;
                default: ОткрытьПлантации_Click(null, null); break;
            }
        }
    }
}