using System;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
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
        private Repository<Warehouse> _складыРепозиторий;
        private Repository<StorageZone> _зоныРепозиторий;
        private Repository<StorageCell> _ячейкиРепозиторий;
        private Repository<Certificate> _сертификатыРепозиторий;
        private Repository<OperationType> _типыОперацийРепозиторий;
        private Repository<AgroOperation> _агроОперацииРепозиторий;
        private Repository<FinishedProductBatch> _партииГПРепозиторий;
        private Repository<StoreReceipt> _поступлениеРепозиторий;
        private Repository<QualityAnalysis> _анализыРепозиторий;

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
            _складыРепозиторий = new Repository<Warehouse>();
            _зоныРепозиторий = new Repository<StorageZone>();
            _ячейкиРепозиторий = new Repository<StorageCell>();
            _сертификатыРепозиторий = new Repository<Certificate>();
            _типыОперацийРепозиторий = new Repository<OperationType>();
            _агроОперацииРепозиторий = new Repository<AgroOperation>();
            _партииГПРепозиторий = new Repository<FinishedProductBatch>();
            _поступлениеРепозиторий = new Repository<StoreReceipt>();
            _анализыРепозиторий = new Repository<QualityAnalysis>();

            ОткрытьПлантации_Click(null, null);
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

        private void ОткрытьЗоныХранения_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ЗоныХранения";
            string запрос = @"
                SELECT 
                    зона_хранения.ид_зоны AS ид,
                    зона_хранения.название_зоны AS НазваниеЗоны,
                    склад.название AS Склад
                FROM зона_хранения
                JOIN склад ON зона_хранения.склад_вк = склад.ид_склада
                ORDER BY склад.название, зона_хранения.название_зоны";
            ТаблицаДанных.ItemsSource = DatabaseHelper.ExecuteQuery(запрос).DefaultView;
        }

        private void ОткрытьЯчейкиХранения_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "ЯчейкиХранения";
            string запрос = @"
                SELECT 
                    ячейка_хранения.ид_ячейки AS ид,
                    ячейка_хранения.код_ячейки AS КодЯчейки,
                    зона_хранения.название_зоны AS Зона,
                    склад.название AS Склад
                FROM ячейка_хранения
                JOIN зона_хранения ON ячейка_хранения.зона_вк = зона_хранения.ид_зоны
                JOIN склад ON зона_хранения.склад_вк = склад.ид_склада
                ORDER BY склад.название, зона_хранения.название_зоны, ячейка_хранения.код_ячейки";
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

        private void ОткрытьАгроОперации_Click(object sender, RoutedEventArgs e)
        {
            _текущаяТаблица = "АгроОперации";
            string запрос = @"
                SELECT 
                    ид_агрооперации AS ид,
                    плантация_вк AS ИдПлантации,
                    тип_операции_вк AS ИдТипа,
                    дата_операции AS Дата,
                    материалы AS Материалы,
                    замечания AS Замечания
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
                форма.ShowDialog();
                ОткрытьПлантации_Click(null, null);
            }
            else if (_текущаяТаблица == "Сотрудники")
            {
                var форма = new PersonForm();
                форма.ShowDialog();
                ОткрытьСотрудники_Click(null, null);
            }
            else if (_текущаяТаблица == "Продукция")
            {
                var форма = new ProductCatalogForm();
                форма.ShowDialog();
                ОткрытьПродукцию_Click(null, null);
            }
            else if (_текущаяТаблица == "ПрофилиОбжарки")
            {
                var форма = new RoastingProfileForm();
                форма.ShowDialog();
                ОткрытьПрофилиОбжарки_Click(null, null);
            }
            else if (_текущаяТаблица == "Магазины")
            {
                var форма = new RetailShopForm();
                форма.ShowDialog();
                ОткрытьМагазины_Click(null, null);
            }
            else if (_текущаяТаблица == "ТипыОпераций")
            {
                var форма = new OperationTypeForm();
                форма.ShowDialog();
                ОткрытьТипыОпераций_Click(null, null);
            }
            else if (_текущаяТаблица == "Склады")
            {
                var форма = new WarehouseForm();
                форма.ShowDialog();
                ОткрытьСклады_Click(null, null);
            }
            else if (_текущаяТаблица == "ЗоныХранения")
            {
                var форма = new StorageZoneForm();
                форма.ShowDialog();
                ОткрытьЗоныХранения_Click(null, null);
            }
            else if (_текущаяТаблица == "ЯчейкиХранения")
            {
                var форма = new StorageCellForm();
                форма.ShowDialog();
                ОткрытьЯчейкиХранения_Click(null, null);
            }
            else if (_текущаяТаблица == "Договоры")
            {
                var форма = new SupplyContractForm();
                форма.ShowDialog();
                ОткрытьДоговоры_Click(null, null);
            }
            else if (_текущаяТаблица == "Сертификаты")
            {
                var форма = new CertificateForm();
                форма.ShowDialog();
                ОткрытьСертификаты_Click(null, null);
            }
            else if (_текущаяТаблица == "Урожай")
            {
                var форма = new HarvestForm();
                форма.ShowDialog();
                ОткрытьУрожай_Click(null, null);
            }
            else if (_текущаяТаблица == "ПартииЗерна")
            {
                var форма = new GreenBeanBatchForm();
                форма.ShowDialog();
                ОткрытьПартииЗерна_Click(null, null);
            }
            else if (_текущаяТаблица == "ПроизводственныеЗаказы")
            {
                var форма = new ProductionOrderForm();
                форма.ShowDialog();
                ОткрытьПроизводствоЗаказы_Click(null, null);
            }
            else if (_текущаяТаблица == "ПартииГП")
            {
                var форма = new FinishedProductBatchForm();
                форма.ShowDialog();
                ОткрытьПартииГП_Click(null, null);
            }
            else if (_текущаяТаблица == "ПоступлениеВМагазин")
            {
                var форма = new StoreReceiptForm();
                форма.ShowDialog();
                ОткрытьПоступление_Click(null, null);
            }
            else if (_текущаяТаблица == "ЗаказыB2B")
            {
                var форма = new B2BOrderForm();
                форма.ShowDialog();
                ОткрытьЗаказыB2B_Click(null, null);
            }
            else if (_текущаяТаблица == "АнализыКачества")
            {
                var форма = new QualityAnalysisForm();
                форма.ShowDialog();
                ОткрытьАнализы_Click(null, null);
            }
            else if (_текущаяТаблица == "АгроОперации")
            {
                var форма = new AgroOperationForm();
                форма.ShowDialog();
                ОткрытьАгроОперации_Click(null, null);
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

            var строка = (DataRowView)ТаблицаДанных.SelectedItem;
            int ид = 0;

            foreach (DataColumn колонка in строка.Row.Table.Columns)
            {
                if (колонка.ColumnName.ToLower() == "ид" || колонка.ColumnName.ToLower().StartsWith("ид_"))
                {
                    ид = Convert.ToInt32(строка[колонка.ColumnName]);
                    break;
                }
            }

            if (ид == 0)
            {
                MessageBox.Show("Не удалось определить ID записи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_текущаяТаблица == "Плантации")
            {
                string query = $"SELECT * FROM плантация WHERE ид_плантации = {ид}";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    var запись = new Plantation
                    {
                        Id = ид,
                        Name = row["название"].ToString(),
                        Country = row["страна"].ToString(),
                        Region = row["регион"].ToString(),
                        Coordinates = row["координаты"].ToString(),
                        Area = Convert.ToDecimal(row["площадь"]),
                        PlantingDate = row["дата_посадки"] as DateTime?,
                        CoffeeVariety = row["сорт_кофе"].ToString(),
                        SoilType = row["тип_почвы"].ToString(),
                        IrrigationSystem = row["система_полива"].ToString()
                    };
                    var форма = new PlantationForm(запись);
                    форма.ShowDialog();
                    ОткрытьПлантации_Click(null, null);
                }
            }
            else if (_текущаяТаблица == "Сотрудники")
            {
                string query = $"SELECT * FROM человек WHERE ид_человека = {ид}";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    var запись = new Person
                    {
                        Id = ид,
                        FullName = row["фио"].ToString(),
                        Phone = row["телефон"].ToString(),
                        Email = row["электронная_почта"].ToString(),
                        BirthDate = row["дата_рождения"] as DateTime?,
                        PassportData = row["паспортные_данные"].ToString()
                    };
                    var форма = new PersonForm(запись);
                    форма.ShowDialog();
                    ОткрытьСотрудники_Click(null, null);
                }
            }
            else if (_текущаяТаблица == "ПартииЗерна")
            {
                string query = $"SELECT * FROM партия_зеленого_зерна WHERE ид_партии_зеленого_зерна = {ид}";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    var запись = new GreenBeanBatch
                    {
                        Id = ид,
                        BatchNumber = row["номер_партии"].ToString(),
                        ReceiptDate = Convert.ToDateTime(row["дата_приемки"]),
                        CountryOfOrigin = row["страна_происхождения"].ToString(),
                        Variety = row["сорт"].ToString(),
                        NetWeightKg = Convert.ToDecimal(row["вес_нетто_кг"]),
                        HumidityPercent = Convert.ToDecimal(row["влажность_процент"]),
                        CellId = row["ячейка_вк"] != DBNull.Value ? Convert.ToInt32(row["ячейка_вк"]) : (int?)null,
                        ShelfLifeMonths = row["срок_хранения_мес"] != DBNull.Value ? Convert.ToInt32(row["срок_хранения_мес"]) : (int?)null
                    };
                    var форма = new GreenBeanBatchForm(запись);
                    форма.ShowDialog();
                    ОткрытьПартииЗерна_Click(null, null);
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

            if (результат != MessageBoxResult.Yes) return;

            try
            {
                var строка = (DataRowView)ТаблицаДанных.SelectedItem;
                int ид = 0;

                foreach (DataColumn колонка in строка.Row.Table.Columns)
                {
                    string имя = колонка.ColumnName.ToLower();
                    if (имя == "ид" || имя.StartsWith("ид_"))
                    {
                        ид = Convert.ToInt32(строка[колонка.ColumnName]);
                        break;
                    }
                }

                if (ид == 0)
                {
                    MessageBox.Show("Не удалось определить ID записи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string имяТаблицы = "";
                string имяКолонки = "";

                switch (_текущаяТаблица)
                {
                    case "Плантации":
                        имяТаблицы = "плантация";
                        имяКолонки = "ид_плантации";
                        break;
                    case "Сотрудники":
                        имяТаблицы = "человек";
                        имяКолонки = "ид_человека";
                        break;
                    case "Продукция":
                        имяТаблицы = "готовая_продукция_справочник";
                        имяКолонки = "ид_справочника";
                        break;
                    case "ПрофилиОбжарки":
                        имяТаблицы = "профиль_обжарки";
                        имяКолонки = "ид_профиля";
                        break;
                    case "Магазины":
                        имяТаблицы = "розничный_магазин";
                        имяКолонки = "ид_магазина";
                        break;
                    case "ТипыОпераций":
                        имяТаблицы = "тип_операции";
                        имяКолонки = "ид_типа_операции";
                        break;
                    case "Склады":
                        имяТаблицы = "склад";
                        имяКолонки = "ид_склада";
                        break;
                    case "ЗоныХранения":
                        имяТаблицы = "зона_хранения";
                        имяКолонки = "ид_зоны";
                        break;
                    case "ЯчейкиХранения":
                        имяТаблицы = "ячейка_хранения";
                        имяКолонки = "ид_ячейки";
                        break;
                    case "Договоры":
                        имяТаблицы = "договор_поставки";
                        имяКолонки = "ид_договора";
                        break;
                    case "Сертификаты":
                        имяТаблицы = "сертификат_соответствия";
                        имяКолонки = "ид_сертификата";
                        break;
                    case "Урожай":
                        имяТаблицы = "урожай";
                        имяКолонки = "ид_урожая";
                        break;
                    case "ПартииЗерна":
                        имяТаблицы = "партия_зеленого_зерна";
                        имяКолонки = "ид_партии_зеленого_зерна";
                        break;
                    case "ПроизводственныеЗаказы":
                        имяТаблицы = "производственный_заказ";
                        имяКолонки = "ид_заказа";
                        break;
                    case "ПартииГП":
                        имяТаблицы = "партия_готовой_продукции";
                        имяКолонки = "ид_партии_гп";
                        break;
                    case "ПоступлениеВМагазин":
                        имяТаблицы = "поступление_в_магазин";
                        имяКолонки = "ид_поступления";
                        break;
                    case "ЗаказыB2B":
                        имяТаблицы = "заказ_в2в";
                        имяКолонки = "ид_заказа_в2в";
                        break;
                    case "АнализыКачества":
                        имяТаблицы = "анализ_качества";
                        имяКолонки = "ид_анализа";
                        break;
                    case "АгроОперации":
                        имяТаблицы = "агротехническая_операция";
                        имяКолонки = "ид_агрооперации";
                        break;
                    default:
                        MessageBox.Show($"Удаление для раздела \"{_текущаяТаблица}\" не реализовано", "В разработке", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                }

                string query = $"DELETE FROM {имяТаблицы} WHERE {имяКолонки} = @ид";
                SqlParameter[] параметры = { new SqlParameter("@ид", ид) };
                DatabaseHelper.ExecuteNonQuery(query, параметры);

                КнопкаОбновить_Click(null, null);
                MessageBox.Show("Запись успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("REFERENCE constraint"))
                {
                    MessageBox.Show("Невозможно удалить запись, так как на неё есть ссылки в других таблицах.\n\nСначала удалите все связанные записи.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void КнопкаОбновить_Click(object sender, RoutedEventArgs e)
        {
            if (_текущаяТаблица == "Плантации")
                ОткрытьПлантации_Click(null, null);
            else if (_текущаяТаблица == "Сотрудники")
                ОткрытьСотрудники_Click(null, null);
            else if (_текущаяТаблица == "ТипыОпераций")
                ОткрытьТипыОпераций_Click(null, null);
            else if (_текущаяТаблица == "ПрофилиОбжарки")
                ОткрытьПрофилиОбжарки_Click(null, null);
            else if (_текущаяТаблица == "Продукция")
                ОткрытьПродукцию_Click(null, null);
            else if (_текущаяТаблица == "Магазины")
                ОткрытьМагазины_Click(null, null);
            else if (_текущаяТаблица == "Склады")
                ОткрытьСклады_Click(null, null);
            else if (_текущаяТаблица == "ЗоныХранения")
                ОткрытьЗоныХранения_Click(null, null);
            else if (_текущаяТаблица == "ЯчейкиХранения")
                ОткрытьЯчейкиХранения_Click(null, null);
            else if (_текущаяТаблица == "Договоры")
                ОткрытьДоговоры_Click(null, null);
            else if (_текущаяТаблица == "Сертификаты")
                ОткрытьСертификаты_Click(null, null);
            else if (_текущаяТаблица == "АгроОперации")
                ОткрытьАгроОперации_Click(null, null);
            else if (_текущаяТаблица == "Урожай")
                ОткрытьУрожай_Click(null, null);
            else if (_текущаяТаблица == "ПартииЗерна")
                ОткрытьПартииЗерна_Click(null, null);
            else if (_текущаяТаблица == "ПроизводственныеЗаказы")
                ОткрытьПроизводствоЗаказы_Click(null, null);
            else if (_текущаяТаблица == "ПартииГП")
                ОткрытьПартииГП_Click(null, null);
            else if (_текущаяТаблица == "ПоступлениеВМагазин")
                ОткрытьПоступление_Click(null, null);
            else if (_текущаяТаблица == "ЗаказыB2B")
                ОткрытьЗаказыB2B_Click(null, null);
            else if (_текущаяТаблица == "АнализыКачества")
                ОткрытьАнализы_Click(null, null);
            else
                ОткрытьПлантации_Click(null, null);
        }
    }
}