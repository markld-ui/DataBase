using Domain.Models;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Определяет контракт для репозитория продуктов.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Предоставляет базовые CRUD-операции для работы с продуктами,
    /// включая создание, чтение, обновление и удаление продуктов.
    /// </para>
    /// <para>
    /// Все методы должны быть реализованы как атомарные и потокобезопасные операции.
    /// </para>
    /// </remarks>
    public interface IProductRepository
    {
        /// <summary>
        /// Получает список всех продуктов.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{Product}"/> всех продуктов.
        /// Если продукты отсутствуют, возвращает пустой список.
        /// </returns>
        List<Product> GetAll();

        /// <summary>
        /// Получает продукт по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор продукта (положительное число).</param>
        /// <returns>
        /// Найденный продукт типа <see cref="Product"/> или null, если продукт не найден.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        Product GetById(int id);

        /// <summary>
        /// Добавляет новый продукт.
        /// </summary>
        /// <param name="product">Добавляемый продукт типа <see cref="Product"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="product"/> равен null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если продукт с таким идентификатором уже существует.
        /// </exception>
        void Add(Product product);

        /// <summary>
        /// Обновляет существующий продукт.
        /// </summary>
        /// <param name="product">Обновляемый продукт типа <see cref="Product"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="product"/> равен null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если продукт с указанным идентификатором не найден.
        /// </exception>
        void Update(Product product);

        /// <summary>
        /// Удаляет продукт по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого продукта.</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если продукт с указанным идентификатором не найден.
        /// </exception>
        void Delete(int id);

        /// <summary>
        /// Получает отфильтрованный список продуктов.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по названию, коду или описанию продукта).</param>
        /// <returns>
        /// Список <see cref="List{Product}"/> продуктов, удовлетворяющих условиям поиска.
        /// Если продукты не найдены, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Поиск выполняется без учета регистра по всем текстовым полям продукта.
        /// </para>
        /// <para>
        /// Если <paramref name="searchText"/> равен null или пустой строке, возвращает все продукты.
        /// </para>
        /// </remarks>
        List<Product> GetFiltered(string searchText);
    }
}