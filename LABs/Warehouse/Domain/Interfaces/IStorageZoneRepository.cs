using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    /// <summary>
    /// Определяет контракт для репозитория зон хранения.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Предоставляет базовые CRUD-операции для работы с зонами хранения,
    /// включая создание, чтение, обновление, удаление и фильтрацию зон хранения.
    /// </para>
    /// <para>
    /// Все методы должны быть реализованы как атомарные и потокобезопасные операции.
    /// </para>
    /// </remarks>
    public interface IStorageZoneRepository
    {
        /// <summary>
        /// Получает список всех зон хранения.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{StorageZone}"/> всех зон хранения.
        /// Если зоны хранения отсутствуют, возвращает пустой список.
        /// </returns>
        List<StorageZone> GetAll();

        /// <summary>
        /// Получает зону хранения по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор зоны хранения (положительное число).</param>
        /// <returns>
        /// Объект <see cref="StorageZone"/> с данными зоны хранения или null, если зона не найдена.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        StorageZone GetById(int id);

        /// <summary>
        /// Добавляет новую зону хранения.
        /// </summary>
        /// <param name="storageZone">Объект зоны хранения типа <see cref="StorageZone"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="storageZone"/> равен null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Выбрасывается, если зона с таким идентификатором уже существует.
        /// </exception>
        void Add(StorageZone storageZone);

        /// <summary>
        /// Обновляет существующую зону хранения.
        /// </summary>
        /// <param name="storageZone">Объект зоны хранения с обновлёнными данными типа <see cref="StorageZone"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр <paramref name="storageZone"/> равен null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если зона с указанным идентификатором не найдена.
        /// </exception>
        void Update(StorageZone storageZone);

        /// <summary>
        /// Удаляет зону хранения по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор зоны хранения (положительное число).</param>
        /// <exception cref="System.ArgumentException">
        /// Выбрасывается, если параметр <paramref name="id"/> меньше или равен 0.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если зона с указанным идентификатором не найдена.
        /// </exception>
        void Delete(int id);

        /// <summary>
        /// Получает отфильтрованный список зон хранения.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по идентификатору, названию зоны, типу зоны или данным склада).</param>
        /// <returns>
        /// Список <see cref="List{StorageZone}"/> зон хранения, удовлетворяющих условиям поиска.
        /// Если зоны не найдены или <paramref name="searchText"/> пустой, возвращает пустой список.
        /// </returns>
        /// <remarks>
        /// Поиск выполняется без учёта регистра по всем текстовым полям зоны хранения.
        /// </remarks>
        List<StorageZone> GetFiltered(string searchText);
    }
}