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
    /// Форма для управления поставками в системе.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Форма предоставляет интерфейс для просмотра, добавления, обновления, удаления и фильтрации
    /// поставок. Использует <see cref="DataGridView"/> для отображения списка поставок,
    /// <see cref="BindingNavigator"/> для навигации и элементы управления для ввода данных.
    /// </para>
    /// <para>
    /// Данные обогащаются названиями продуктов и поставщиков через класс <see cref="SupplyView"/>.
    /// Поддерживает фильтрацию по текстовому запросу и валидацию ввода (продукт, поставщик, количество).
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var form = new SupplyForm();
    /// form.ShowDialog();
    /// </code>
    /// </example>
    public partial class SupplyForm : Form
    {
        private readonly ISupplyRepository _supplyRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private Supply _selectedSupply;
        private BindingSource _bindingSource;
        private BindingNavigator _bindingNavigator;
        private ToolStripTextBox _toolStripTextBoxFind;
        private ToolStripButton _toolStripButtonFind;
        private CheckBox _checkBoxFind;
        private ToolStripButton _toolStripButtonReset;
        private List<Supply> _allSupplies;
        private Dictionary<int, string> _productNames;
        private Dictionary<int, string> _supplierNames;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="SupplyForm"/>.
        /// </summary>
        /// <remarks>
        /// Инициализирует компоненты формы, репозитории, <see cref="BindingSource"/> и
        /// <see cref="BindingNavigator"/>. Настраивает элементы управления навигатора, включая
        /// кнопки поиска, сброса и фильтрации.
        /// </remarks>
        public SupplyForm()
        {
            InitializeComponent();
            _supplyRepository = Application.AppContext.Instance.SupplyRepository;
            _productRepository = Application.AppContext.Instance.ProductRepository;
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
        }

        /// <summary>
        /// Обрабатывает событие загрузки формы.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Вызывает методы для загрузки поставок, продуктов и поставщиков.
        /// </remarks>
        private void SupplyForm_Load(object sender, EventArgs e)
        {
            LoadSupplies();
            LoadProducts();
            LoadSuppliers();
        }

        /// <summary>
        /// Загружает список поставок и отображает их в таблице.
        /// </summary>
        /// <remarks>
        /// Получает все поставки через <see cref="ISupplyRepository.GetAll"/>, названия продуктов
        /// через <see cref="IProductRepository.GetAll"/> и названия поставщиков через
        /// <see cref="ISupplierRepository.GetAll"/>. Обогащает данные названиями с помощью
        /// <see cref="SupplyView"/> и привязывает их к <see cref="DataGridView"/> через
        /// <see cref="BindingSource"/>. Настраивает заголовки колонок и обработчик добавления
        /// новой записи.
        /// </remarks>
        private void LoadSupplies()
        {
            try
            {
                _allSupplies = _supplyRepository.GetAll();
                var products = _productRepository.GetAll().ToDictionary(p => p.ProductId, p => p.Name);
                var suppliers = _supplierRepository.GetAll().ToDictionary(s => s.SupplierId, s => s.CompanyName);
                _productNames = products;
                _supplierNames = suppliers;

                var supplyView = _allSupplies.Select(s => new SupplyView
                {
                    SupplyId = s.SupplyId,
                    ProductName = _productNames.ContainsKey(s.ProductId) ? _productNames[s.ProductId] : "Неизвестно",
                    SupplierName = _supplierNames.ContainsKey(s.SupplierId) ? _supplierNames[s.SupplierId] : "Неизвестно",
                    SupplyDate = s.SupplyDate,
                    Quantity = s.Quantity
                }).ToList();

                _bindingSource.DataSource = new BindingList<SupplyView>(supplyView);
                dataGridViewSupplies.DataSource = _bindingSource;

                dataGridViewSupplies.Columns["SupplyId"].HeaderText = "ID";
                dataGridViewSupplies.Columns["ProductName"].HeaderText = "Продукт";
                dataGridViewSupplies.Columns["SupplierName"].HeaderText = "Поставщик";
                dataGridViewSupplies.Columns["SupplyDate"].HeaderText = "Дата поставки";
                dataGridViewSupplies.Columns["Quantity"].HeaderText = "Количество";

                // Обработчик для добавления новой записи через BindingNavigator
                _bindingNavigator.AddNewItem.Click += (s, args) =>
                {
                    ClearInputs();
                    _selectedSupply = null;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки поставок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загружает список продуктов в выпадающий список.
        /// </summary>
        /// <remarks>
        /// Получает данные через <see cref="IProductRepository.GetAll"/> и привязывает их
        /// к <see cref="ComboBox"/> с отображением имени продукта и значением ID.
        /// </remarks>
        private void LoadProducts()
        {
            try
            {
                var products = _productRepository.GetAll();
                cmbProduct.DataSource = products;
                cmbProduct.DisplayMember = "Name";
                cmbProduct.ValueMember = "ProductId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продуктов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Загружает список поставщиков в выпадающий список.
        /// </summary>
        /// <remarks>
        /// Получает данные через <see cref="ISupplierRepository.GetAll"/> и привязывает их
        /// к <see cref="ComboBox"/> с отображением названия компании и значением ID.
        /// </remarks>
        private void LoadSuppliers()
        {
            try
            {
                var suppliers = _supplierRepository.GetAll();
                cmbSupplier.DataSource = suppliers;
                cmbSupplier.DisplayMember = "CompanyName";
                cmbSupplier.ValueMember = "SupplierId";
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
        /// Заполняет элементы управления данными выбранной поставки, включая продукт,
        /// поставщика, дату поставки и количество.
        /// </remarks>
        private void dataGridViewSupplies_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewSupplies.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewSupplies.SelectedRows[0];
                var viewItem = selectedRow.DataBoundItem as SupplyView;
                if (viewItem != null)
                {
                    _selectedSupply = _allSupplies.FirstOrDefault(s => s.SupplyId == viewItem.SupplyId);
                    if (_selectedSupply != null)
                    {
                        cmbProduct.SelectedValue = _selectedSupply.ProductId;
                        cmbSupplier.SelectedValue = _selectedSupply.SupplierId;
                        dtpSupplyDate.Value = _selectedSupply.SupplyDate;
                        txtQuantity.Text = _selectedSupply.Quantity.ToString();
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
        /// Проверяет ввод, создаёт новую поставку, добавляет её в базу данных,
        /// перезагружает список поставок и очищает поля ввода.
        /// </remarks>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var supply = new Supply
                {
                    ProductId = (int)cmbProduct.SelectedValue,
                    SupplierId = (int)cmbSupplier.SelectedValue,
                    SupplyDate = dtpSupplyDate.Value,
                    Quantity = int.Parse(txtQuantity.Text)
                };

                _supplyRepository.Add(supply);
                LoadSupplies();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления поставки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Обновить".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет выбор поставки и ввод, обновляет данные поставки в базе данных,
        /// перезагружает список поставок и очищает поля ввода.
        /// </remarks>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedSupply == null)
                {
                    MessageBox.Show("Выберите поставку для обновления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput()) return;

                _selectedSupply.ProductId = (int)cmbProduct.SelectedValue;
                _selectedSupply.SupplierId = (int)cmbSupplier.SelectedValue;
                _selectedSupply.SupplyDate = dtpSupplyDate.Value;
                _selectedSupply.Quantity = int.Parse(txtQuantity.Text);

                _supplyRepository.Update(_selectedSupply);
                LoadSupplies();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления поставки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обрабатывает событие нажатия кнопки "Удалить".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Проверяет выбор поставки, запрашивает подтверждение, удаляет поставку из базы данных,
        /// перезагружает список поставок и очищает поля ввода.
        /// </remarks>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedSupply == null)
                {
                    MessageBox.Show("Выберите поставку для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Вы уверены, что хотите удалить поставку с ID '{_selectedSupply.SupplyId}'?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _supplyRepository.Delete(_selectedSupply.SupplyId);
                    LoadSupplies();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления поставки: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Применяет фильтр к списку поставок на основе текстового запроса.
        /// </summary>
        /// <param name="searchText">Текст для фильтрации.</param>
        /// <remarks>
        /// Получает отфильтрованные поставки через <see cref="ISupplyRepository.GetFiltered"/>,
        /// обогащает их названиями продуктов и поставщиков и обновляет таблицу. Показывает
        /// сообщение, если ничего не найдено, и перезагружает полный список.
        /// </remarks>
        private void ApplyFilter(string searchText)
        {
            try
            {
                var filteredSupplies = _supplyRepository.GetFiltered(searchText);
                var products = _productRepository.GetAll().ToDictionary(p => p.ProductId, p => p.Name);
                var suppliers = _supplierRepository.GetAll().ToDictionary(s => s.SupplierId, s => s.CompanyName);

                var supplyView = filteredSupplies.Select(s => new SupplyView
                {
                    SupplyId = s.SupplyId,
                    ProductName = products.ContainsKey(s.ProductId) ? products[s.ProductId] : "Неизвестно",
                    SupplierName = suppliers.ContainsKey(s.SupplierId) ? suppliers[s.SupplierId] : "Неизвестно",
                    SupplyDate = s.SupplyDate,
                    Quantity = s.Quantity
                }).ToList();

                _bindingSource.DataSource = new BindingList<SupplyView>(supplyView);
                dataGridViewSupplies.DataSource = _bindingSource;

                if (_bindingSource.Count == 0)
                {
                    MessageBox.Show("Таких записей не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSupplies();
                }
                else
                {
                    _bindingSource.Position = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadSupplies();
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
        /// Перезагружает список поставок и очищает поле поиска.
        /// </remarks>
        private void ToolStripButtonReset_Click(object sender, EventArgs e)
        {
            LoadSupplies();
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
                LoadSupplies();
            }
        }

        /// <summary>
        /// Проверяет корректность введённых данных.
        /// </summary>
        /// <returns><c>true</c>, если данные корректны; иначе <c>false</c>.</returns>
        /// <remarks>
        /// Проверяет выбор продукта, поставщика и корректность количества (положительное число).
        /// </remarks>
        private bool ValidateInput()
        {
            if (cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("Выберите продукт.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbSupplier.SelectedValue == null)
            {
                MessageBox.Show("Выберите поставщика.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество (положительное число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Очищает поля ввода и сбрасывает выбранную поставку.
        /// </summary>
        /// <remarks>
        /// Сбрасывает выпадающие списки продукта и поставщика, устанавливает текущую дату
        /// для поля даты поставки, очищает поле количества и устанавливает
        /// <see cref="_selectedSupply"/> в null.
        /// </remarks>
        private void ClearInputs()
        {
            cmbProduct.SelectedIndex = -1;
            cmbSupplier.SelectedIndex = -1;
            dtpSupplyDate.Value = DateTime.Now;
            txtQuantity.Clear();
            _selectedSupply = null;
        }

        /// <summary>
        /// Представляет модель представления для отображения данных поставок в таблице.
        /// </summary>
        /// <remarks>
        /// Класс обогащает данные <see cref="Supply"/> названиями продукта и поставщика вместо их ID.
        /// </remarks>
        private class SupplyView
        {
            /// <summary>
            /// Получает или задаёт идентификатор поставки.
            /// </summary>
            public int SupplyId { get; set; }

            /// <summary>
            /// Получает или задаёт название продукта.
            /// </summary>
            public string ProductName { get; set; }

            /// <summary>
            /// Получает или задаёт название поставщика.
            /// </summary>
            public string SupplierName { get; set; }

            /// <summary>
            /// Получает или задаёт дату поставки.
            /// </summary>
            public DateTime SupplyDate { get; set; }

            /// <summary>
            /// Получает или задаёт количество продукции в поставке.
            /// </summary>
            public int Quantity { get; set; }
        }
    }
}