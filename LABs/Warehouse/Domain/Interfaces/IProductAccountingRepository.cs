using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    /// <summary>
    /// Определяет контракт для репозитория учета продуктов.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Предоставляет базовые CRUD-операции для работы с учетом продуктов,
    /// включая добавление, обновление, удаление и поиск записей.
    /// </para>
    /// <para>
    /// Все методы должны быть реализованы как атомарные и потокобезопасные.
    /// </para>
    /// </remarks>
    public interface IProductAccountingRepository
    {
        /// <summary>
        /// Получает все записи учета продуктов.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{ProductAccounting}"/> всех записей учета продуктов.
        /// Если записи отсутствуют, возвращает пустой список.
        /// </returns>
        List<ProductAccounting> GetAll();

        /// <summary>
        /// Получает запись учета продукта по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи учета продукта.</param>
        /// <returns>
        /// Найденная запись типа <see cref="ProductAccounting"/> или null, если запись не найдена.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        ProductAccounting GetById(int id);

        /// <summary>
        /// Добавляет новую запись учета продукта.
        /// </summary>
        /// <param name="productAccounting">Добавляемая запись учета продукта типа <see cref="ProductAccounting"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="productAccounting"/> равен null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если запись с таким идентификатором уже существует.
        /// </exception>
        void Add(ProductAccounting productAccounting);

        /// <summary>
        /// Обновляет существующую запись учета продукта.
        /// </summary>
        /// <param name="productAccounting">Обновляемая запись учета продукта типа <see cref="ProductAccounting"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="productAccounting"/> равен null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если запись с указанным идентификатором не найдена.
        /// </exception>
        void Update(ProductAccounting productAccounting);

        /// <summary>
        /// Удаляет запись учета продукта по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи.</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если запись с указанным идентификатором не найдена.
        /// </exception>
        void Delete(int id);

        /// <summary>
        /// Получает отфильтрованный список записей учета продуктов.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по названию продукта, коду или месту хранения).</param>
        /// <returns>
        /// Список <see cref="List{ProductAccounting}"/> записей, удовлетворяющих условиям поиска.
        /// Если записи не найдены, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// Поиск выполняется без учета регистра по всем текстовым полям записи.
        /// </remarks>
        List<ProductAccounting> GetFiltered(string searchText);
    }
}
