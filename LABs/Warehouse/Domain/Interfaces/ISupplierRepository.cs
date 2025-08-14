using Domain.Models;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Определяет контракт для репозитория поставщиков.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Предоставляет базовые CRUD-операции для работы с поставщиками,
    /// включая создание, чтение, обновление, удаление и фильтрацию поставщиков.
    /// </para>
    /// <para>
    /// Все методы должны быть реализованы как атомарные и потокобезопасные операции.
    /// </para>
    /// </remarks>
    public interface ISupplierRepository
    {
        /// <summary>
        /// Получает список всех поставщиков.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{Supplier}"/> всех поставщиков.
        /// Если поставщики отсутствуют, возвращает пустой список.
        /// </returns>
        List<Supplier> GetAll();

        /// <summary>
        /// Получает поставщика по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставщика (положительное число).</param>
        /// <returns>
        /// Объект <see cref="Supplier"/> с данными поставщика или null, если поставщик не найден.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        Supplier GetById(int id);

        /// <summary>
        /// Добавляет нового поставщика.
        /// </summary>
        /// <param name="supplier">Объект поставщика типа <see cref="Supplier"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="supplier"/> равен null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если поставщик с таким идентификатором уже существует.
        /// </exception>
        void Add(Supplier supplier);

        /// <summary>
        /// Обновляет существующего поставщика.
        /// </summary>
        /// <param name="supplier">Объект поставщика с обновлёнными данными типа <see cref="Supplier"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="supplier"/> равен null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если поставщик с указанным идентификатором не найден.
        /// </exception>
        void Update(Supplier supplier);

        /// <summary>
        /// Удаляет поставщика по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставщика (положительное число).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если поставщик с указанным идентификатором не найден.
        /// </exception>
        void Delete(int id);

        /// <summary>
        /// Получает отфильтрованный список поставщиков.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по идентификатору, названию компании, контактному лицу, телефону или адресу).</param>
        /// <returns>
        /// Список <see cref="List{Supplier}"/> поставщиков, удовлетворяющих условиям поиска.
        /// Если поставщики не найдены или <paramref name="searchText"/> пустой, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// Поиск выполняется без учёта регистра по всем текстовым полям поставщика.
        /// </remarks>
        List<Supplier> GetFiltered(string searchText);
    }
}
