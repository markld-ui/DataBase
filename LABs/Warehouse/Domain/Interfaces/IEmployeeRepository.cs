using Domain.Models;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    /// <summary>
    /// Определяет контракт для репозитория сотрудников.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Предоставляет базовые CRUD-операции для работы с сотрудниками,
    /// а также метод для фильтрации сотрудников.
    /// </para>
    /// <para>
    /// Все методы должны быть реализованы как потокобезопасные.
    /// </para>
    /// </remarks>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Получает список всех сотрудников.
        /// </summary>
        /// <returns>
        /// Список <see cref="List{Employee}"/>, содержащий всех сотрудников.
        /// Возвращает пустой список, если сотрудники отсутствуют.
        /// </returns>
        List<Employee> GetAll();

        /// <summary>
        /// Получает сотрудника по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сотрудника.</param>
        /// <returns>
        /// Найденный сотрудник типа <see cref="Employee"/> или null, если сотрудник не найден.
        /// </returns>
        Employee GetById(int id);

        /// <summary>
        /// Добавляет нового сотрудника.
        /// </summary>
        /// <param name="employee">Добавляемый сотрудник типа <see cref="Employee"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр employee равен null.
        /// </exception>
        void Add(Employee employee);

        /// <summary>
        /// Обновляет данные существующего сотрудника.
        /// </summary>
        /// <param name="employee">Обновляемый сотрудник типа <see cref="Employee"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Выбрасывается, если параметр employee равен null.
        /// </exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если сотрудник с таким идентификатором не найден.
        /// </exception>
        void Update(Employee employee);

        /// <summary>
        /// Удаляет сотрудника по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого сотрудника.</param>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// Выбрасывается, если сотрудник с указанным идентификатором не найден.
        /// </exception>
        void Delete(int id);

        /// <summary>
        /// Получает отфильтрованный список сотрудников.
        /// </summary>
        /// <param name="searchText">Текст для поиска (по имени, фамилии или должности).</param>
        /// <returns>
        /// Список <see cref="List{Employee}"/>, содержащий сотрудников, удовлетворяющих условиям поиска.
        /// Возвращает пустой список, если сотрудники не найдены.
        /// </returns>
        /// <remarks>
        /// Поиск выполняется без учета регистра.
        /// </remarks>
        List<Employee> GetFiltered(string searchText);
    }
}