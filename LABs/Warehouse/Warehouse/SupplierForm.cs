using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Interfaces;
using Domain.Models;

namespace UI
{
    /// <summary>
    /// Форма для управления поставщиками в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Форма предоставляет интерфейс для просмотра, добавления, обновления, удаления и фильтрации
    /// поставщиков. Использует <see cref="DataGridView"/> для отображения списка поставщиков,
    /// <see cref="BindingNavigator"/> для навигации и элементы управления для ввода данных.
    /// </para>
    /// <para>
    /// Данные поставщиков загружаются из репозитория <see cref="ISupplierRepository"/>. Форма
    /// поддерживает фильтрацию по текстовому запросу и минимальную валидацию ввода (проверка
    /// названия компании).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var form = new SupplierForm();
    /// form.ShowDialog();
    /// </code>
    /// </example>
    public partial class SupplierForm : Form
    {
        private readonly ISupplierRepository _supplierRepository;
        private Supplier _selectedSupplier;
        private BindingSource _bindingSource;
        private BindingNavigator _bindingNavigator;
        private ToolStripTextBox _toolStripTextBoxFind;
        private ToolStripButton _toolStripButtonFind;
        private CheckBox _checkBoxFind;
        private ToolStripButton _toolStripButtonReset;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SupplierForm"/>.
        /// </summary>
        /// <remarks>
        /// Инициализирует компоненты формы, репозиторий, <see cref="BindingSource"/> и
        /// <see cref="BindingNavigator"/>. Настраивает элементы управления навигатора, включая
        /// кнопки поиска, сброса и фильтрации. Подписывается на событие добавления новой записи.
        /// </remarks>
        public SupplierForm()
        {
            InitializeComponent();
            _supplierRepository = Application.AppContext.Instance.SupplierRepository;
            _bindingSource = new BindingSource();

            _bindingNavigator = new BindingNavigator(_bindingSource);
            _bindingNavigator.Dock = DockStyle.Top;
            this.Controls.Add(_bindingNavigator);

            _bindingNavigator.MoveFirstItem.ToolTipText = "Перейти к первой записи";
            _bindingNavigator.MovePreviousItem.ToolTipText = "Перейти к предыдущей записи";
            _bindingNavigator.MoveNextItem.ToolTipText = "Перейти к следующей записи";
            _bindingNavigator.MoveLastItem.ToolTipText = "Перейти к последней записи";
            _bindingNavigator.AddNewItem.ToolTipText = "Добавить новую запись";
            _bindingNavigator.DeleteItem.ToolTipText = "Удалить текущую запись";
            _bindingNavigator.PositionItem.ToolTipText = "Текущая позиция";
            _bindingNavigator.CountItem.ToolTipText = "Общее количество записей";

            ToolStripSeparator separator = new ToolStripSeparator();
            _bindingNavigator.Items.Add(separator);

            _toolStripTextBoxFind = new ToolStripTextBox();
            _toolStripTextBoxFind.Name = "toolStripTextBoxFind";
            _bindingNavigator.Items.Add(_toolStripTextBoxFind);

            _toolStripButtonFind = new ToolStripButton();
            _toolStripButtonFind.Name = "toolStripButtonFind";
            _toolStripButtonFind.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _toolStripButtonFind.Text = "Поиск";
            _toolStripButtonFind.TextAlign = ContentAlignment.MiddleRight;
            _toolStripButtonFind.Image = SystemIcons.Information.ToBitmap();
            _toolStripButtonFind.ImageAlign = ContentAlignment.MiddleLeft;
            _toolStripButtonFind.Click += ToolStripButtonFind_Click;
            _bindingNavigator.Items.Add(_toolStripButtonFind);

            _toolStripButtonReset = new ToolStripButton();
            _toolStripButtonReset.Name = "toolStripButtonReset";
            _toolStripButtonReset.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _toolStripButtonReset.Text = "Сброс";
            _toolStripButtonReset.TextAlign = ContentAlignment.MiddleRight;
            _toolStripButtonReset.Image = SystemIcons.Error.ToBitmap();
            _toolStripButtonReset.ImageAlign = ContentAlignment.MiddleLeft;
            _toolStripButtonReset.Click += ToolStripButtonReset_Click;
            _bindingNavigator.Items.Add(_toolStripButtonReset);

            _checkBoxFind = new CheckBox();
            _checkBoxFind.Name = "checkBoxFind";
            _checkBoxFind.Text = "Фильтр";
            _checkBoxFind.CheckedChanged += CheckBoxFind_CheckedChanged;
            ToolStripControlHost checkBoxHost = new ToolStripControlHost(_checkBoxFind);
            _bindingNavigator.Items.Add(checkBoxHost);

            _bindingNavigator.AddNewItem.Click += (s, args) =>
            {
                ClearInputs();
                _selectedSupplier = null;
            };
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Вызывает метод для загрузки списка поставщиков.
        /// </remarks>
        private void SupplierForm_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        /// <summary>
        /// Загружает список поставщиков и отображает их в таблице.
        /// </summary>
        /// <remarks>
        /// Получает все записи поставщиков через <see cref="ISupplierRepository.GetAll"/> и
        /// привязывает их к <see cref="DataGridView"/> через <see cref="BindingSource"/>.
        /// Настраивает заголовки колонок.
        /// </remarks>
        private void LoadSuppliers()
        {
            try
            {
                var suppliers = _supplierRepository.GetAll();
                _bindingSource.DataSource = new BindingList<Supplier>(suppliers);
                dataGridViewSuppliers.DataSource = _bindingSource;

                dataGridViewSuppliers.Columns["SupplierId"].HeaderText = "ID";
                dataGridViewSuppliers.Columns["CompanyName"].HeaderText = "Название компании";
                dataGridViewSuppliers.Columns["ContactPerson"].HeaderText = "Контактное лицо";
                dataGridViewSuppliers.Columns["Phone"].HeaderText = "Телефон";
                dataGridViewSuppliers.Columns["Address"].HeaderText = "Адрес";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки поставщиков: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие изменения выбора строки в таблице.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Заполняет элементы управления данными выбранного поставщика, включая название
        /// компании, контактное лицо, телефон и адрес.
        /// </remarks>
        private void dataGridViewSuppliers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewSuppliers.SelectedRows[0];
                _selectedSupplier = selectedRow.DataBoundItem as Supplier;
                if (_selectedSupplier != null)
                {
                    txtCompanyName.Text = _selectedSupplier.CompanyName;
                    txtContactPerson.Text = _selectedSupplier.ContactPerson;
                    txtPhone.Text = _selectedSupplier.Phone;
                    txtAddress.Text = _selectedSupplier.Address;
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Добавить".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет ввод, создаёт нового поставщика, добавляет его в базу данных,
        /// перезагружает список поставщиков и очищает поля ввода.
        /// </remarks>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var supplier = new Supplier
                {
                    CompanyName = txtCompanyName.Text,
                    ContactPerson = txtContactPerson.Text,
                    Phone = txtPhone.Text,
                    Address = txtAddress.Text
                };

                _supplierRepository.Add(supplier);
                LoadSuppliers();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления поставщика: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Обновить".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет выбор поставщика и ввод, обновляет данные поставщика в базе данных,
        /// перезагружает список поставщиков и очищает поля ввода.
        /// </remarks>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedSupplier == null)
                {
                    MessageBox.Show("Выберите поставщика для обновления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput()) return;

                _selectedSupplier.CompanyName = txtCompanyName.Text;
                _selectedSupplier.ContactPerson = txtContactPerson.Text;
                _selectedSupplier.Phone = txtPhone.Text;
                _selectedSupplier.Address = txtAddress.Text;

                _supplierRepository.Update(_selectedSupplier);
                LoadSuppliers();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления поставщика: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Удалить".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет выбор поставщика, запрашивает подтверждение, удаляет поставщика из базы
        /// данных, перезагружает список поставщиков и очищает поля ввода.
        /// </remarks>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedSupplier == null)
                {
                    MessageBox.Show("Выберите поставщика для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Вы уверены, что хотите удалить поставщика '{_selectedSupplier.CompanyName}'?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _supplierRepository.Delete(_selectedSupplier.SupplierId);
                    LoadSuppliers();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления поставщика: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Применяет фильтр к списку поставщиков на основе текстового запроса.
        /// </summary>
        /// <param name="searchText">Текст для фильтрации.</param>
        /// <remarks>
        /// Получает отфильтрованные записи через <see cref="ISupplierRepository.GetFiltered"/>,
        /// обновляет таблицу. Показывает сообщение, если ничего не найдено, и перезагружает
        /// полный список.
        /// </remarks>
        private void ApplyFilter(string searchText)
        {
            try
            {
                var filteredSuppliers = _supplierRepository.GetFiltered(searchText);
                _bindingSource.DataSource = new BindingList<Supplier>(filteredSuppliers);
                dataGridViewSuppliers.DataSource = _bindingSource;

                if (_bindingSource.Count == 0)
                {
                    MessageBox.Show("Таких записей не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSuppliers();
                }
                else
                {
                    _bindingSource.Position = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadSuppliers();
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Поиск".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет текстовое поле поиска и вызывает <see cref="ApplyFilter"/> с введённым текстом.
        /// </remarks>
        private void ToolStripButtonFind_Click(object sender, EventArgs e)
        {
            if (_toolStripTextBoxFind == null)
            {
                MessageBox.Show("Ошибка: toolStripTextBoxFind не инициализирован.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string searchText = _toolStripTextBoxFind.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Введите текст для поиска.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ApplyFilter(searchText);
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Сброс".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Перезагружает список поставщиков и очищает поле поиска.
        /// </remarks>
        private void ToolStripButtonReset_Click(object sender, EventArgs e)
        {
            LoadSuppliers();
            _toolStripTextBoxFind.Text = null;
        }

        /// <summary>
        /// Обрабатывает событие изменения состояния флажка фильтрации.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// При включённом флажке применяет фильтр на основе текста поиска, при выключенном — сбрасывает данные.
        /// </remarks>
        private void CheckBoxFind_CheckedChanged(object sender, EventArgs e)
        {
            if (_toolStripTextBoxFind == null || _checkBoxFind == null)
            {
                MessageBox.Show("Ошибка: элементы не инициализированы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _checkBoxFind.Checked = false;
                return;
            }

            if (_checkBoxFind.Checked)
            {
                string searchText = _toolStripTextBoxFind.Text.Trim();
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    MessageBox.Show("Введите текст для фильтрации.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _checkBoxFind.Checked = false;
                    return;
                }

                ApplyFilter(searchText);
            }
            else
            {
                LoadSuppliers();
            }
        }

        /// <summary>
        /// Проверяет корректность введённых данных.
        /// </summary>
        /// <returns><c>true</c>, если данные корректны; иначе <c>false</c>.</returns>
        /// <remarks>
        /// Проверяет только наличие названия компании. Другие поля (контактное лицо, телефон,
        /// адрес) считаются необязательными.
        /// </remarks>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                MessageBox.Show("Введите название компании.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Очищает поля ввода и сбрасывает выбранного поставщика.
        /// </summary>
        /// <remarks>
        /// Очищает текстовые поля для названия компании, контактного лица, телефона и адреса.
        /// Устанавливает <see cref="_selectedSupplier"/> в null.
        /// </remarks>
        private void ClearInputs()
        {
            txtCompanyName.Clear();
            txtContactPerson.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            _selectedSupplier = null;
        }
    }
}