using System.Data;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Определяет контракт для репозитория SQL-запросов.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Предоставляет методы для выполнения сложных SQL-запросов и операций
    /// с базой данных, включая подзапросы, агрегатные функции и CRUD-операции.
    /// </para>
    /// <para>
    /// Все методы должны быть реализованы с использованием параметризованных запросов
    /// для защиты от SQL-инъекций и быть потокобезопасными.
    /// </para>
    /// </remarks>
    public interface ISQLQueriesRepository
    {
        /// <summary>
        /// Получает все записи учета продуктов.
        /// </summary>
        /// <returns>
        /// Объект <see cref="DataTable"/> с полным набором записей учета продуктов.
        /// Структура таблицы включает все поля из таблицы базы данных, а также связанные данные
        /// из таблиц employee и storage_zone.
        /// </returns>
        DataTable GetAllRecords();

        /// <summary>
        /// Получает записи учета продуктов по идентификатору сотрудника.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника (положительное число).</param>
        /// <returns>
        /// Объект <see cref="DataTable"/> с записями, связанными с указанным сотрудником,
        /// включая данные о сотруднике и зоне хранения.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="employeeId"/> меньше или равен 0.
        /// </exception>
        DataTable GetRecordsByEmployee(int employeeId);

        /// <summary>
        /// Получает агрегированные данные учета продуктов.
        /// </summary>
        /// <param name="minRecordCount">Минимальное количество записей для включения в результат.</param>
        /// <param name="startDate">Начальная дата для фильтрации записей.</param>
        /// <returns>
        /// Объект <see cref="DataTable"/> с агрегированными данными, содержащий
        /// название зоны хранения, количество записей, сумму и среднее значение количества.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="minRecordCount"/> меньше 0
        /// или <paramref name="startDate"/> находится в будущем относительно текущей даты.
        /// </exception>
        DataTable GetAggregateRecords(int minRecordCount, DateTime startDate);

        /// <summary>
        /// Получает упрощенные записи учета продуктов (без связанных данных).
        /// </summary>
        /// <returns>
        /// Объект <see cref="DataTable"/> с ограниченным набором полей:
        /// productAcc_id, accounting_date, quantity, employee_id, supply_id, storage_id.
        /// </returns>
        DataTable GetSimpleProductAccountingRecords();

        /// <summary>
        /// Выполняет коррелированный подзапрос для получения связанных данных поставки.
        /// </summary>
        /// <param name="supplyId">Идентификатор поставки (положительное число).</param>
        /// <returns>
        /// Объект <see cref="DataTable"/> с результатами коррелированного подзапроса,
        /// включая данные учета продуктов, дату поставки и имя сотрудника.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="supplyId"/> меньше или равен 0.
        /// </exception>
        DataTable GetCorrelatedSubquery(int supplyId);

        /// <summary>
        /// Выполняет некоррелированный подзапрос для получения связанных данных поставки.
        /// </summary>
        /// <param name="supplyId">Идентификатор поставки (положительное число).</param>
        /// <returns>
        /// Объект <see cref="DataTable"/> с результатами некоррелированного подзапроса,
        /// включая данные учета продуктов с количеством выше среднего для указанной поставки.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="supplyId"/> меньше или равен 0.
        /// </exception>
        DataTable GetNonCorrelatedSubquery(int supplyId);

        /// <summary>
        /// Проверяет существование сотрудника по идентификатору.
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника (положительное число).</param>
        /// <returns>
        /// <c>true</c>, если сотрудник существует; <c>false</c> в противном случае.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="employeeId"/> меньше или равен 0.
        /// </exception>
        bool EmployeeExists(int employeeId);

        /// <summary>
        /// Проверяет существование поставки по идентификатору.
        /// </summary>
        /// <param name="supplyId">Идентификатор поставки (положительное число).</param>
        /// <returns>
        /// <c>true</c>, если поставка существует; <c>false</c> в противном случае.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="supplyId"/> меньше или равен 0.
        /// </exception>
        bool SupplyExists(int supplyId);

        /// <summary>
        /// Проверяет существование зоны хранения по идентификатору.
        /// </summary>
        /// <param name="storageId">Идентификатор зоны хранения (положительное число).</param>
        /// <returns>
        /// <c>true</c>, если зона хранения существует; <c>false</c> в противном случае.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="storageId"/> меньше или равен 0.
        /// </exception>
        bool StorageZoneExists(int storageId);

        /// <summary>
        /// Проверяет существование записи учета по идентификатору.
        /// </summary>
        /// <param name="recordId">Идентификатор записи учета (положительное число).</param>
        /// <returns>
        /// <c>true</c>, если запись существует; <c>false</c> в противном случае.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="recordId"/> меньше или равен 0.
        /// </exception>
        bool RecordExists(int recordId);

        /// <summary>
        /// Добавляет новую запись учета продукта.
        /// </summary>
        /// <param name="accountingDate">Дата учета.</param>
        /// <param name="quantity">Количество продуктов.</param>
        /// <param name="employeeId">Идентификатор сотрудника.</param>
        /// <param name="supplyId">Идентификатор поставки.</param>
        /// <param name="storageId">Идентификатор зоны хранения.</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если:
        /// - любой идентификатор (<paramref name="employeeId"/>, <paramref name="supplyId"/>, <paramref name="storageId"/>) меньше или равен 0;
        /// - <paramref name="quantity"/> меньше или равен 0;
        /// - <paramref name="accountingDate"/> находится в будущем.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если:
        /// - сотрудник с указанным <paramref name="employeeId"/> не существует;
        /// - поставка с указанным <paramref name="supplyId"/> не существует;
        /// - зона хранения с указанным <paramref name="storageId"/> не существует.
        /// </exception>
        void InsertRecord(DateTime accountingDate, int quantity, int employeeId, int supplyId, int storageId);

        /// <summary>
        /// Обновляет существующую запись учета продукта.
        /// </summary>
        /// <param name="id">Идентификатор записи (положительное число).</param>
        /// <param name="accountingDate">Новая дата учета (null, если не требуется обновление).</param>
        /// <param name="quantity">Новое количество (null, если не требуется обновление).</param>
        /// <param name="employeeId">Новый идентификатор сотрудника (null, если не требуется обновление).</param>
        /// <param name="supplyId">Новый идентификатор поставки (null, если не требуется обновление).</param>
        /// <param name="storageId">Новый идентификатор зоны хранения (null, если не требуется обновление).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если:
        /// - <paramref name="id"/> меньше или равен 0;
        /// - <paramref name="quantity"/> меньше или равен 0 (если указано);
        /// - <paramref name="accountingDate"/> находится в будущем (если указано);
        /// - любой идентификатор (<paramref name="employeeId"/>, <paramref name="supplyId"/>, <paramref name="storageId"/>) меньше или равен 0 (если указано).
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если:
        /// - запись с указанным <paramref name="id"/> не существует;
        /// - сотрудник с указанным <paramref name="employeeId"/> не существует (если указано);
        /// - поставка с указанным <paramref name="supplyId"/> не существует (если указано);
        /// - зона хранения с указанным <paramref name="storageId"/> не существует (если указано).
        /// </exception>
        void UpdateRecord(int id, DateTime? accountingDate, int? quantity, int? employeeId, int? supplyId, int? storageId);

        /// <summary>
        /// Удаляет запись учета продукта.
        /// </summary>
        /// <param name="id">Идентификатор записи (положительное число).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если запись с указанным <paramref name="id"/> не существует.
        /// </exception>
        void DeleteRecord(int id);
    }
}