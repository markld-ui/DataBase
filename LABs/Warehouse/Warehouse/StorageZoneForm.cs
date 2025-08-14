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
    /// Форма для управления зонами хранения в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Форма предоставляет интерфейс для просмотра, добавления, обновления, удаления и фильтрации
    /// зон хранения. Использует <see cref="DataGridView"/> для отображения списка зон,
    /// <see cref="BindingNavigator"/> для навигации и элементы управления для ввода данных.
    /// </para>
    /// <para>
    /// Данные обогащаются названием склада через класс <see cref="StorageZoneView"/>. Поддерживает
    /// фильтрацию по текстовому запросу и работу с типами зон через перечисление <see cref="ZoneType"/>.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var form = new StorageZoneForm();
    /// form.ShowDialog();
    /// </code>
    /// </example>
    public partial class StorageZoneForm : Form
    {
        private readonly IStorageZoneRepository _storageZoneRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private StorageZone _selectedStorageZone;
        private BindingSource _bindingSource;
        private BindingNavigator _bindingNavigator;
        private ToolStripTextBox _toolStripTextBoxFind;
        private ToolStripButton _toolStripButtonFind;
        private CheckBox _checkBoxFind;
        private ToolStripButton _toolStripButtonReset;
        private List<StorageZone> _allStorageZones;
        private Dictionary<int, string> _warehouseNames;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="StorageZoneForm"/>.
        /// </summary>
        /// <remarks>
        /// Инициализирует компоненты формы, репозитории, <see cref="BindingSource"/> и
        /// <see cref="BindingNavigator"/>. Настраивает элементы управления навигатора, включая
        /// кнопки поиска, сброса и фильтрации. Подписывается на событие добавления новой записи.
        /// </remarks>
        public StorageZoneForm()
        {
            InitializeComponent();
            _storageZoneRepository = Application.AppContext.Instance.StorageZoneRepository;
            _warehouseRepository = Application.AppContext.Instance.WarehouseRepository;
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
                _selectedStorageZone = null;
            };
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Вызывает методы для загрузки зон хранения, складов и типов зон.
        /// </remarks>
        private void StorageZoneForm_Load(object sender, EventArgs e)
        {
            LoadStorageZones();
            LoadWarehouses();
            LoadZoneTypes();
        }

        /// <summary>
        /// Загружает список зон хранения и отображает их в таблице.
        /// </summary>
        /// <remarks>
        /// Получает все зоны хранения через <see cref="IStorageZoneRepository.GetAll"/> и
        /// названия складов через <see cref="IWarehouseRepository.GetAll"/>. Обогащает данные
        /// названием склада с помощью <see cref="StorageZoneView"/> и привязывает их к
        /// <see cref="DataGridView"/> через <see cref="BindingSource"/>. Настраивает заголовки колонок.
        /// </remarks>
        private void LoadStorageZones()
        {
            try
            {
                _allStorageZones = _storageZoneRepository.GetAll();

                var warehouses = _warehouseRepository.GetAll().ToDictionary(w => w.WarehouseId, w => w.Name);
                _warehouseNames = warehouses;

                var storageZoneView = _allStorageZones.Select(sz => new StorageZoneView
                {
                    StorageId = sz.StorageId,
                    WarehouseName = _warehouseNames.ContainsKey(sz.WarehouseId) ? _warehouseNames[sz.WarehouseId] : "Неизвестно",
                    Capacity = sz.Capacity,
                    ZoneType = sz.ZoneType,
                    ZoneName = sz.ZoneName
                }).ToList();

                _bindingSource.DataSource = new BindingList<StorageZoneView>(storageZoneView);
                dataGridViewStorageZones.DataSource = _bindingSource;

                dataGridViewStorageZones.Columns["StorageId"].HeaderText = "ID";
                dataGridViewStorageZones.Columns["WarehouseName"].HeaderText = "Склад";
                dataGridViewStorageZones.Columns["Capacity"].HeaderText = "Вместимость";
                dataGridViewStorageZones.Columns["ZoneType"].HeaderText = "Тип зоны";
                dataGridViewStorageZones.Columns["ZoneName"].HeaderText = "Название зоны";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки зон хранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загружает список складов в выпадающий список.
        /// </summary>
        /// <remarks>
        /// Получает данные через <see cref="IWarehouseRepository.GetAll"/> и привязывает их
        /// к <see cref="ComboBox"/> с отображением имени склада и значением ID.
        /// </remarks>
        private void LoadWarehouses()
        {
            try
            {
                var warehouses = _warehouseRepository.GetAll();
                cmbWarehouse.DataSource = warehouses;
                cmbWarehouse.DisplayMember = "Name";
                cmbWarehouse.ValueMember = "WarehouseId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки складов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загружает типы зон в выпадающий список.
        /// </summary>
        /// <remarks>
        /// Получает значения перечисления <see cref="ZoneType"/> и привязывает их к <see cref="ComboBox"/>.
        /// </remarks>
        private void LoadZoneTypes()
        {
            try
            {
                var zoneTypes = Enum.GetValues(typeof(ZoneType));
                cmbZoneType.DataSource = zoneTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов зон: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие изменения выбора строки в таблице.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Заполняет элементы управления данными выбранной зоны хранения, включая склад,
        /// вместимость, тип зоны и название.
        /// </remarks>
        private void dataGridViewStorageZones_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewStorageZones.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewStorageZones.SelectedRows[0];
                var viewItem = selectedRow.DataBoundItem as StorageZoneView;

                if (viewItem != null)
                {
                    _selectedStorageZone = _allStorageZones.FirstOrDefault(sz => sz.StorageId == viewItem.StorageId);

                    if (_selectedStorageZone != null)
                    {
                        cmbWarehouse.SelectedValue = _selectedStorageZone.WarehouseId;
                        txtCapacity.Text = _selectedStorageZone.Capacity.ToString();
                        cmbZoneType.SelectedItem = _selectedStorageZone.ZoneType;
                        txtZoneName.Text = _selectedStorageZone.ZoneName;
                    }
                }
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Добавить".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет ввод, создаёт новую зону хранения, добавляет её в базу данных,
        /// перезагружает список зон и очищает поля ввода.
        /// </remarks>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var storageZone = new StorageZone
                {
                    WarehouseId = (int)cmbWarehouse.SelectedValue,
                    Capacity = int.Parse(txtCapacity.Text),
                    ZoneType = (ZoneType)cmbZoneType.SelectedItem,
                    ZoneName = txtZoneName.Text
                };

                _storageZoneRepository.Add(storageZone);
                LoadStorageZones();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления зоны хранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Обновить".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет выбор зоны и ввод, обновляет данные зоны в базе данных,
        /// перезагружает список зон и очищает поля ввода.
        /// </remarks>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedStorageZone == null)
                {
                    MessageBox.Show("Выберите зону хранения для обновления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput()) return;

                _selectedStorageZone.WarehouseId = (int)cmbWarehouse.SelectedValue;
                _selectedStorageZone.Capacity = int.Parse(txtCapacity.Text);
                _selectedStorageZone.ZoneType = (ZoneType)cmbZoneType.SelectedItem;
                _selectedStorageZone.ZoneName = txtZoneName.Text;

                _storageZoneRepository.Update(_selectedStorageZone);
                LoadStorageZones();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления зоны хранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Удалить".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет выбор зоны, запрашивает подтверждение, удаляет зону из базы данных,
        /// перезагружает список зон и очищает поля ввода.
        /// </remarks>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedStorageZone == null)
                {
                    MessageBox.Show("Выберите зону хранения для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Вы уверены, что хотите удалить зону хранения '{_selectedStorageZone.ZoneName}'?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _storageZoneRepository.Delete(_selectedStorageZone.StorageId);
                    LoadStorageZones();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления зоны хранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Применяет фильтр к списку зон хранения на основе текстового запроса.
        /// </summary>
        /// <param name="searchText">Текст для фильтрации.</param>
        /// <remarks>
        /// Получает отфильтрованные зоны через <see cref="IStorageZoneRepository.GetFiltered"/>,
        /// обогащает их названием склада и обновляет таблицу. Показывает сообщение, если ничего не найдено.
        /// </remarks>
        private void ApplyFilter(string searchText)
        {
            try
            {
                var filteredStorageZones = _storageZoneRepository.GetFiltered(searchText);

                var storageZoneView = filteredStorageZones.Select(sz => new StorageZoneView
                {
                    StorageId = sz.StorageId,
                    WarehouseName = _warehouseNames.ContainsKey(sz.WarehouseId) ? _warehouseNames[sz.WarehouseId] : "Неизвестно",
                    Capacity = sz.Capacity,
                    ZoneType = sz.ZoneType,
                    ZoneName = sz.ZoneName
                }).ToList();

                _bindingSource.DataSource = new BindingList<StorageZoneView>(storageZoneView);
                dataGridViewStorageZones.DataSource = _bindingSource;

                if (_bindingSource.Count == 0)
                {
                    MessageBox.Show("Таких записей не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStorageZones();
                }
                else
                {
                    _bindingSource.Position = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadStorageZones();
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
        /// Перезагружает список зон хранения и очищает поле поиска.
        /// </remarks>
        private void ToolStripButtonReset_Click(object sender, EventArgs e)
        {
            LoadStorageZones();
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
                LoadStorageZones();
            }
        }

        /// <summary>
        /// Проверяет корректность введённых данных.
        /// </summary>
        /// <returns><c>true</c>, если данные корректны; иначе <c>false</c>.</returns>
        /// <remarks>
        /// Проверяет выбор склада, корректность вместимости, выбор типа зоны и наличие названия зоны.
        /// Показывает сообщения об ошибках при некорректных данных.
        /// </remarks>
        private bool ValidateInput()
        {
            if (cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCapacity.Text) || !int.TryParse(txtCapacity.Text, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Введите корректную вместимость (положительное число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbZoneType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип зоны.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtZoneName.Text))
            {
                MessageBox.Show("Введите название зоны.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Очищает поля ввода и сбрасывает выбранную зону хранения.
        /// </summary>
        /// <remarks>
        /// Сбрасывает выпадающий список склада, поля вместимости и названия зоны, а также тип зоны.
        /// Устанавливает <see cref="_selectedStorageZone"/> в null.
        /// </remarks>
        private void ClearInputs()
        {
            cmbWarehouse.SelectedIndex = -1;
            txtCapacity.Clear();
            cmbZoneType.SelectedIndex = -1;
            txtZoneName.Clear();
            _selectedStorageZone = null;
        }

        /// <summary>
        /// Представляет модель представления для отображения данных зон хранения в таблице.
        /// </summary>
        /// <remarks>
        /// Класс обогащает данные <see cref="StorageZone"/> названием склада вместо его ID.
        /// </remarks>
        private class StorageZoneView
        {
            /// <summary>
            /// Получает или задаёт идентификатор зоны хранения.
            /// </summary>
            public int StorageId { get; set; }

            /// <summary>
            /// Получает или задаёт название склада.
            /// </summary>
            public string WarehouseName { get; set; }

            /// <summary>
            /// Получает или задаёт вместимость зоны.
            /// </summary>
            public int Capacity { get; set; }

            /// <summary>
            /// Получает или задаёт тип зоны.
            /// </summary>
            public ZoneType ZoneType { get; set; }

            /// <summary>
            /// Получает или задаёт название зоны.
            /// </summary>
            public string ZoneName { get; set; }
        }
    }
}