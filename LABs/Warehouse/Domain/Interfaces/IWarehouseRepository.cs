using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    /// <summary>
    /// Определяет контракт для репозитория складов.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Предоставляет методы для выполнения CRUD-операций и фильтрации данных складов,
    /// включая получение всех записей, получение по идентификатору, добавление, обновление,
    /// удаление и поиск по текстовому запросу.
    /// </para>
    /// <para>
    /// Реализация должна обеспечивать атомарность операций и защиту от SQL-инъекций.
    /// </para>
    /// </remarks>
    public interface IWarehouseRepository
    {
        /// <summary>
        /// Получает список всех складов.
        /// </summary>
        /// <returns>
        /// Список объектов <see cref="List{Warehouse}"/> всех складов.
        /// Возвращает пустой список, если склады отсутствуют.
        /// </returns>
        List<Warehouse> GetAll();

        /// <summary>
        /// Получает склад по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор склада (положительное число).</param>
        /// <returns>
        /// Объект <see cref="Warehouse"/> с данными склада или null, если склад не найден.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="id"/> меньше или равен 0.
        /// </exception>
        Warehouse GetById(int id);

        /// <summary>
        /// Добавляет новый склад.
        /// </summary>
        /// <param name="warehouse">Объект склада типа <see cref="Warehouse"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если <paramref name="warehouse"/> равен null.
        /// </exception>
        void Add(Warehouse warehouse);

        /// <summary>
        /// Обновляет данные существующего склада.
        /// </summary>
        /// <param name="warehouse">Объект склада с обновлёнными данными типа <see cref="Warehouse"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если <paramref name="warehouse"/> равен null.
        /// </exception>
        void Update(Warehouse warehouse);

        /// <summary>
        /// Удаляет склад по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор склада (положительное число).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если <paramref name="id"/> меньше или равен 0.
        /// </exception>
        void Delete(int id);

        /// <summary>
        /// Получает отфильтрованный список складов на основе текста поиска.
        /// </summary>
        /// <param name="searchText">Текст для поиска по полям склада (идентификатор, название, адрес).</param>
        /// <returns>
        /// Список объектов <see cref="List{Warehouse}"/> складов, соответствующих критериям поиска.
        /// Возвращает пустой список, если ничего не найдено.
        /// </returns>
        /// <remarks>
        /// Поиск выполняется без учёта регистра.
        /// </remarks>
        List<Warehouse> GetFiltered(string searchText);
    }
}
