using Domain.Models;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Определяет контракт для репозитория поставок.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Предоставляет методы для выполнения CRUD-операций и фильтрации данных поставок,
    /// включая получение всех записей, получение по идентификатору, добавление, обновление,
    /// удаление и поиск по текстовому запросу.
    /// </para>
    /// <para>
    /// Реализация должна обеспечивать атомарность операций и защиту от SQL-инъекций.
    /// </para>
    /// </remarks>
    public interface ISupplyRepository
    {
        /// <summary>
        /// Получает список всех поставок.
        /// </summary>
        /// <returns>
        /// Список объектов <see cref="List{Supply}"/> всех поставок.
        /// Возвращает пустой список, если поставки отсутствуют.
        /// </returns>
        List<Supply> GetAll();

        /// <summary>
        /// Получает поставку по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставки (положительное число).</param>
        /// <returns>
        /// Объект <see cref="Supply"/> с данными поставки или null, если поставка не найдена.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="id"/> меньше или равен 0.
        /// </exception>
        Supply GetById(int id);

        /// <summary>
        /// Добавляет новую поставку.
        /// </summary>
        /// <param name="supply">Объект поставки типа <see cref="Supply"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если <paramref name="supply"/> равен null.
        /// </exception>
        void Add(Supply supply);

        /// <summary>
        /// Обновляет данные существующей поставки.
        /// </summary>
        /// <param name="supply">Объект поставки с обновлёнными данными типа <see cref="Supply"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если <paramref name="supply"/> равен null.
        /// </exception>
        void Update(Supply supply);

        /// <summary>
        /// Удаляет поставку по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор поставки (положительное число).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="id"/> меньше или равен 0.
        /// </exception>
        void Delete(int id);

        /// <summary>
        /// Получает отфильтрованный список поставок на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска по полям поставки (идентификатор, продукт, поставщик, дата поставки, количество).</param>
        /// <returns>
        /// Список объектов <see cref="List{Supply}"/> поставок, соответствующих критериям поиска.
        /// Возвращает пустой список, если ничего не найдено.
        /// </returns>
        /// <remarks>
        /// Поиск выполняется без учёта регистра.
        /// </remarks>
        List<Supply> GetFiltered(string searchText);
    }
}
