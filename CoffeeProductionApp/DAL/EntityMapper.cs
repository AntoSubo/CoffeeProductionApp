using System;
using System.Data;
using System.Reflection;

namespace CoffeeProductionApp.DAL
{
    public static class EntityMapper
    {
        public static T MapToEntity<T>(DataRow row) where T : new()
        {
            T entity = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                string columnName = GetColumnName(prop.Name);
                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                {
                    object value = row[columnName];
                    if (prop.PropertyType == typeof(DateTime?) && value is DateTime dt)
                        prop.SetValue(entity, dt);
                    else if (prop.PropertyType == typeof(decimal?) && value is decimal dec)
                        prop.SetValue(entity, dec);
                    else if (prop.PropertyType == typeof(int?) && value is int i)
                        prop.SetValue(entity, i);
                    else if (prop.PropertyType.IsEnum)
                        prop.SetValue(entity, Enum.ToObject(prop.PropertyType, value));
                    else
                        prop.SetValue(entity, Convert.ChangeType(value, prop.PropertyType));
                }
            }
            return entity;
        }

        public static string GetColumnName(string propertyName)
        {
            switch (propertyName)
            {
                case "Id": return "ид";
                case "Name": return "название";
                case "FullName": return "фио";
                case "Phone": return "телефон";
                case "Email": return "электронная_почта";
                case "BirthDate": return "дата_рождения";
                case "PassportData": return "паспортные_данные";
                case "PlantationId": return "плантация_вк";
                case "HarvestDate": return "дата_сбора";
                case "ProcessingMethod": return "метод_обработки";
                case "BerriesWeightKg": return "вес_ягод_кг";
                case "DryingHumidity": return "влажность_после_сушки";
                case "GreenBeansWeightKg": return "вес_зеленого_зерна_кг";
                case "DefectPercentage": return "процент_дефектов";
                case "HarvestId": return "урожай_вк";
                case "PersonId": return "человек_вк";
                case "CollectedWeightKg": return "собранный_вес_кг";
                case "WarehouseId": return "склад_вк";
                case "ZoneId": return "зона_вк";
                case "CellId": return "ячейка_вк";
                case "CellCode": return "код_ячейки";
                case "MeasurementDate": return "дата_замера";
                case "TemperatureC": return "температура_c";
                case "HumidityPercent": return "влажность_процент";
                case "BatchNumber": return "номер_партии";
                case "ReceiptDate": return "дата_приемки";
                case "CountryOfOrigin": return "страна_происхождения";
                case "Variety": return "сорт";
                case "NetWeightKg": return "вес_нетто_кг";
                case "ShelfLifeMonths": return "срок_хранения_мес";
                case "Description": return "описание";
                case "Parameters": return "параметры";
                case "OrderNumber": return "номер_заказа";
                case "CreationDate": return "дата_создания";
                case "PlannedCompletionDate": return "плановая_дата_выполнения";
                case "GreenBeanBatchId": return "партия_зеленого_зерна_вк";
                case "RoastingProfileId": return "профиль_обжарки_вк";
                case "RoastLevel": return "степень_обжарки";
                case "TargetFlavor": return "целевой_вкус";
                case "PlannedWeightKg": return "плановый_вес_кг";
                case "ActualWeightKg": return "фактический_вес_кг";
                case "CompletionDate": return "дата_выполнения";
                case "ProductionOrderId": return "производственный_заказ_вк";
                case "RoleInOrder": return "роль_в_заказе";
                case "Sku": return "артикул";
                case "TradeName": return "торговое_наименование";
                case "GrowingAltitude": return "высота_произрастания";
                case "FlavorNotes": return "вкусовые_ноты";
                case "DefaultPackageType": return "тип_упаковки_по_умолчанию";
                case "ReleaseDate": return "дата_выпуска";
                case "PackageType": return "тип_упаковки";
                case "PackageCount": return "количество_упаковок";
                case "UnitCost": return "себестоимость_единицы";
                case "RetailPrice": return "розничная_цена";
                case "WholesalePrice": return "оптовая_цена";
                case "ExpiryDate": return "срок_годности";
                case "ContractNumber": return "номер_договора";
                case "ConclusionDate": return "дата_заключения";
                case "LegalEntityName": return "наименование_юрлица";
                case "Inn": return "инн";
                case "PaymentTerms": return "условия_оплаты";
                case "MinOrderKg": return "мин_объем_заказа_кг";
                case "ContractId": return "договор_вк";
                case "RoleInContract": return "роль_в_договоре";
                case "B2BOrderId": return "заказ_в2в_вк";
                case "ProductCatalogId": return "справочник_гп_вк";
                case "Quantity": return "количество";
                case "ShopId": return "магазин_вк";
                case "FinishedProductBatchId": return "партия_гп_вк";
                case "PurchasePrice": return "закупочная_цена";
                case "AnalysisDate": return "дата_анализа";
                case "SampleType": return "тип_образца";
                case "CuppingScore": return "оценка_каппинга";
                case "Conclusion": return "заключение";
                case "CertificateNumber": return "номер_сертификата";
                case "IssueDate": return "дата_выдачи";
                case "CertificationBody": return "орган_сертификации";
                case "Standard": return "стандарт";
                case "WarehouseType": return "тип_склада";
                case "Address": return "адрес";
                case "ZoneName": return "название_зоны";
                case "Status": return "статус";
                case "Position": return "должность";
                case "HireDate": return "дата_приема";
                case "Coordinates": return "координаты";
                case "Area": return "площадь";
                case "PlantingDate": return "дата_посадки";
                case "CoffeeVariety": return "сорт_кофе";
                case "SoilType": return "тип_почвы";
                case "IrrigationSystem": return "система_полива";
                case "Materials": return "материалы";
                case "Remarks": return "замечания";
                case "OperationDate": return "дата_операции";
                case "OperationTypeId": return "тип_операции_вк";
                case "ShipmentDate": return "дата_отгрузки";
                case "DeliveryMethod": return "способ_доставки";
                case "TtnNumber": return "номер_ттн";
                case "ShopName": return "название";
                case "Country": return "страна";
                case "Region": return "регион";
                default: return propertyName.ToLower();
            }
        }

        public static string GetTableName<T>()
        {
            string typeName = typeof(T).Name;
            switch (typeName)
            {
                case "Plantation": return "плантация";
                case "Person": return "человек";
                case "OperationType": return "тип_операции";
                case "AgroOperation": return "агротехническая_операция";
                case "Harvest": return "урожай";
                case "HarvestPerson": return "урожай_человек";
                case "Warehouse": return "склад";
                case "StorageZone": return "зона_хранения";
                case "StorageCell": return "ячейка_хранения";
                case "StorageConditionRecord": return "запись_условий_хранения";
                case "GreenBeanBatch": return "партия_зеленого_зерна";
                case "RoastingProfile": return "профиль_обжарки";
                case "ProductionOrder": return "производственный_заказ";
                case "ProductionOrderPerson": return "производственный_заказ_человек";
                case "ProductCatalog": return "готовая_продукция_справочник";
                case "FinishedProductBatch": return "партия_готовой_продукции";
                case "SupplyContract": return "договор_поставки";
                case "ContractPerson": return "договор_человек";
                case "B2BOrder": return "заказ_в2в";
                case "B2BOrderItem": return "позиция_заказа_в2в";
                case "RetailShop": return "розничный_магазин";
                case "ShopPerson": return "магазин_человек";
                case "StoreReceipt": return "поступление_в_магазин";
                case "QualityAnalysis": return "анализ_качества";
                case "Certificate": return "сертификат_соответствия";
                default: return typeName.ToLower();
            }
        }
    }
}