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
    public partial class ProductAccountingForm : Form
    {
        private readonly IProductAccountingRepository _productAccountingRepository;
        private readonly ISupplyRepository _supplyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IStorageZoneRepository _storageZoneRepository;
        private ProductAccounting _selectedProductAccounting;
        private BindingSource _bindingSource;
        private BindingNavigator _bindingNavigator;
        private ToolStripTextBox _toolStripTextBoxFind;
        private ToolStripButton _toolStripButtonFind;
        private CheckBox _checkBoxFind;
        private ToolStripButton _toolStripButtonReset;
        private List<string> _movementStatuses;

        public ProductAccountingForm()
        {
            InitializeComponent();
            _productAccountingRepository = Application.AppContext.Instance.ProductAccountingRepository;
            _supplyRepository = Application.AppContext.Instance.SupplyRepository;
            _employeeRepository = Application.AppContext.Instance.EmployeeRepository;
            _storageZoneRepository = Application.AppContext.Instance.StorageZoneRepository;
            _bindingSource = new BindingSource();
            _movementStatuses = new List<string> { "В наличии", "Перемещено", "Списано" };

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
                _selectedProductAccounting = null;
            };
        }

        private void ProductAccountingForm_Load(object sender, EventArgs e)
        {
            LoadProductAccountings();
            LoadSupplies();
            LoadEmployees();
            LoadStorageZones();
            dtpLastMovementDate.Enabled = false;
        }

        private void LoadProductAccountings()
        {
            try
            {
                var productAccountings = _productAccountingRepository.GetAll();
                var enrichedData = productAccountings.Select(pa =>
                {
                    var employee = _employeeRepository.GetById(pa.EmployeeId);
                    var lastMovementDate = pa.LastMovementDate.HasValue ? DateTime.Parse(pa.LastMovementDate.Value.ToString("dd.MM.yyyy")) : (DateTime?)null;
                    return new ProductAccountingView
                    {
                        ProductAccId = pa.ProductAccId,
                        SupplyId = pa.SupplyId.ToString(),
                        EmployeeId = pa.EmployeeId.ToString(),
                        EmployeeName = employee?.FullName ?? "Не найдено",
                        EmployeePosition = employee?.Position ?? "Не указано",
                        StorageId = pa.StorageId.ToString(),
                        StorageZone = _storageZoneRepository.GetById(pa.StorageId)?.ZoneName ?? "Не найдено",
                        AccountingDate = pa.AccountingDate,
                        Quantity = pa.Quantity,
                        LastMovementDate = pa.LastMovementDate?.ToString("dd.MM.yyyy") ?? "Не указано",
                        DaysSinceAccounting = (DateTime.Now - pa.AccountingDate).Days,
                        IsRecentMovement = lastMovementDate.HasValue && (DateTime.Now - lastMovementDate.Value).Days <= 30 ? "Да" : "Нет",
                        MovementStatus = pa.MovementStatus ?? "В наличии"
                    };
                }).ToList();

                _bindingSource.DataSource = new BindingList<ProductAccountingView>(enrichedData);

                // Отключаем автоматическую генерацию колонок
                dataGridViewProductAccountings.AutoGenerateColumns = false;

                // Очищаем существующие колонки
                dataGridViewProductAccountings.Columns.Clear();

                // Создаём колонки вручную
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ProductAccId",
                    HeaderText = "ID учета",
                    Name = "ProductAccId",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "SupplyId",
                    HeaderText = "ID поставки",
                    Name = "SupplyId",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "EmployeeId",
                    HeaderText = "ID сотрудника",
                    Name = "EmployeeId",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "EmployeeName",
                    HeaderText = "Имя сотрудника",
                    Name = "EmployeeName",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "EmployeePosition",
                    HeaderText = "Должность сотрудника",
                    Name = "EmployeePosition",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StorageId",
                    HeaderText = "ID зоны",
                    Name = "StorageId",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StorageZone",
                    HeaderText = "Название зоны хранения",
                    Name = "StorageZone",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "AccountingDate",
                    HeaderText = "Дата учета",
                    Name = "AccountingDate",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Quantity",
                    HeaderText = "Количество продукции",
                    Name = "Quantity",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "LastMovementDate",
                    HeaderText = "Дата последнего движения",
                    Name = "LastMovementDate",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "DaysSinceAccounting",
                    HeaderText = "Дней с даты учета",
                    Name = "DaysSinceAccounting",
                    ReadOnly = true
                });
                dataGridViewProductAccountings.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "IsRecentMovement",
                    HeaderText = "Недавнее движение?",
                    Name = "IsRecentMovement",
                    ReadOnly = true
                });

                // Добавляем выпадающий список для MovementStatus
                var comboBoxColumn = new DataGridViewComboBoxColumn
                {
                    DataPropertyName = "MovementStatus",
                    HeaderText = "Статус передвижения",
                    Name = "MovementStatus",
                    DataSource = _movementStatuses,
                    FlatStyle = FlatStyle.Flat
                };
                dataGridViewProductAccountings.Columns.Add(comboBoxColumn);

                // Привязываем источник данных
                dataGridViewProductAccountings.DataSource = _bindingSource;

                // Подписываемся на события для обработки изменений
                dataGridViewProductAccountings.CellValueChanged += DataGridViewProductAccountings_CellValueChanged;
                dataGridViewProductAccountings.CurrentCellDirtyStateChanged += DataGridViewProductAccountings_CurrentCellDirtyStateChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки учета продукции: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridViewProductAccountings_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewProductAccountings.IsCurrentCellDirty)
            {
                dataGridViewProductAccountings.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DataGridViewProductAccountings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewProductAccountings.Columns["MovementStatus"].Index && e.RowIndex >= 0)
            {
                try
                {
                    var row = dataGridViewProductAccountings.Rows[e.RowIndex];
                    var productAccId = (int)row.Cells["ProductAccId"].Value;
                    var newStatus = row.Cells["MovementStatus"].Value?.ToString();

                    var productAccounting = _productAccountingRepository.GetById(productAccId);
                    if (productAccounting != null)
                    {
                        productAccounting.MovementStatus = newStatus;
                        _productAccountingRepository.Update(productAccounting);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка обновления статуса: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadProductAccountings();
                }
            }
        }

        private void LoadSupplies()
        {
            try
            {
                var supplies = _supplyRepository.GetAll();
                cmbSupply.DataSource = supplies;
                cmbSupply.DisplayMember = "SupplyId";
                cmbSupply.ValueMember = "SupplyId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки поставок: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadEmployees()
        {
            try
            {
                var employees = _employeeRepository.GetAll();
                cmbEmployee.DataSource = employees;
                cmbEmployee.DisplayMember = "FullName";
                cmbEmployee.ValueMember = "EmployeeId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сотрудников: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStorageZones()
        {
            try
            {
                var storageZones = _storageZoneRepository.GetAll();
                cmbStorageZone.DataSource = storageZones;
                cmbStorageZone.DisplayMember = "ZoneName";
                cmbStorageZone.ValueMember = "StorageId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки зон хранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewProductAccountings_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProductAccountings.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewProductAccountings.SelectedRows[0];
                var selectedView = selectedRow.DataBoundItem as ProductAccountingView;
                _selectedProductAccounting = _productAccountingRepository.GetById(selectedView.ProductAccId);
                if (_selectedProductAccounting != null)
                {
                    cmbSupply.SelectedValue = _selectedProductAccounting.SupplyId;
                    cmbEmployee.SelectedValue = _selectedProductAccounting.EmployeeId;
                    cmbStorageZone.SelectedValue = _selectedProductAccounting.StorageId;
                    dtpAccountingDate.Value = _selectedProductAccounting.AccountingDate;
                    txtQuantity.Text = _selectedProductAccounting.Quantity.ToString();
                    if (_selectedProductAccounting.LastMovementDate.HasValue)
                    {
                        dtpLastMovementDate.Value = _selectedProductAccounting.LastMovementDate.Value;
                        chkLastMovementDate.Checked = true;
                    }
                    else
                    {
                        chkLastMovementDate.Checked = false;
                    }
                }
            }
        }

        private void chkLastMovementDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpLastMovementDate.Enabled = chkLastMovementDate.Checked;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var productAccounting = new ProductAccounting
                {
                    SupplyId = (int)cmbSupply.SelectedValue,
                    EmployeeId = (int)cmbEmployee.SelectedValue,
                    StorageId = (int)cmbStorageZone.SelectedValue,
                    AccountingDate = dtpAccountingDate.Value,
                    Quantity = int.Parse(txtQuantity.Text),
                    LastMovementDate = chkLastMovementDate.Checked ? dtpLastMovementDate.Value : null,
                    MovementStatus = "В наличии" // Значение по умолчанию для новой записи
                };

                _productAccountingRepository.Add(productAccounting);
                LoadProductAccountings();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления учета продукции: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedProductAccounting == null)
                {
                    MessageBox.Show("Выберите запись учета продукции для обновления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput()) return;

                _selectedProductAccounting.SupplyId = (int)cmbSupply.SelectedValue;
                _selectedProductAccounting.EmployeeId = (int)cmbEmployee.SelectedValue;
                _selectedProductAccounting.StorageId = (int)cmbStorageZone.SelectedValue;
                _selectedProductAccounting.AccountingDate = dtpAccountingDate.Value;
                _selectedProductAccounting.Quantity = int.Parse(txtQuantity.Text);
                _selectedProductAccounting.LastMovementDate = chkLastMovementDate.Checked ? dtpLastMovementDate.Value : null;

                _productAccountingRepository.Update(_selectedProductAccounting);
                LoadProductAccountings();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления учета продукции: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedProductAccounting == null)
                {
                    MessageBox.Show("Выберите запись учета продукции для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Вы уверены, что хотите удалить запись учета с ID '{_selectedProductAccounting.ProductAccId}'?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _productAccountingRepository.Delete(_selectedProductAccounting.ProductAccId);
                    LoadProductAccountings();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления учета продукции: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter(string searchText)
        {
            try
            {
                var filteredProductAccountings = _productAccountingRepository.GetFiltered(searchText);
                var enrichedData = filteredProductAccountings.Select(pa =>
                {
                    var employee = _employeeRepository.GetById(pa.EmployeeId);
                    var lastMovementDate = pa.LastMovementDate.HasValue ? DateTime.Parse(pa.LastMovementDate.Value.ToString("dd.MM.yyyy")) : (DateTime?)null;
                    return new ProductAccountingView
                    {
                        ProductAccId = pa.ProductAccId,
                        SupplyId = pa.SupplyId.ToString(),
                        EmployeeId = pa.EmployeeId.ToString(),
                        EmployeeName = employee?.FullName ?? "Не найдено",
                        EmployeePosition = employee?.Position ?? "Не указано",
                        StorageId = pa.StorageId.ToString(),
                        StorageZone = _storageZoneRepository.GetById(pa.StorageId)?.ZoneName ?? "Не найдено",
                        AccountingDate = pa.AccountingDate,
                        Quantity = pa.Quantity,
                        LastMovementDate = pa.LastMovementDate?.ToString("dd.MM.yyyy") ?? "Не указано",
                        DaysSinceAccounting = (DateTime.Now - pa.AccountingDate).Days,
                        IsRecentMovement = lastMovementDate.HasValue && (DateTime.Now - lastMovementDate.Value).Days <= 30 ? "Да" : "Нет",
                        MovementStatus = pa.MovementStatus ?? "В наличии"
                    };
                }).ToList();

                _bindingSource.DataSource = new BindingList<ProductAccountingView>(enrichedData);
                dataGridViewProductAccountings.DataSource = _bindingSource;

                if (_bindingSource.Count == 0)
                {
                    MessageBox.Show("Таких записей не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductAccountings();
                }
                else
                {
                    _bindingSource.Position = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadProductAccountings();
            }
        }

        private void ToolStripButtonFind_Click(object sender, EventArgs e)
        {
            if (_toolStripTextBoxFind == null)
            {
                MessageBox.Show("Ошибка: toolStripTextBoxFind не инициализирован.");
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

        private void ToolStripButtonReset_Click(object sender, EventArgs e)
        {
            LoadProductAccountings();
            _toolStripTextBoxFind.Text = null;
        }

        private void CheckBoxFind_CheckedChanged(object sender, EventArgs e)
        {
            if (_toolStripTextBoxFind == null || _checkBoxFind == null)
            {
                MessageBox.Show("Ошибка: элементы не инициализированы.");
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
                LoadProductAccountings();
            }
        }

        private bool ValidateInput()
        {
            if (cmbSupply.SelectedValue == null)
            {
                MessageBox.Show("Выберите поставку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbEmployee.SelectedValue == null)
            {
                MessageBox.Show("Выберите сотрудника.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbStorageZone.SelectedValue == null)
            {
                MessageBox.Show("Выберите зону хранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество (положительное число).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            cmbSupply.SelectedIndex = -1;
            cmbEmployee.SelectedIndex = -1;
            cmbStorageZone.SelectedIndex = -1;
            dtpAccountingDate.Value = DateTime.Now;
            txtQuantity.Clear();
            dtpLastMovementDate.Value = DateTime.Now;
            chkLastMovementDate.Checked = false;
            _selectedProductAccounting = null;
        }
    }


    public class ProductAccountingView
    {
        public int ProductAccId { get; set; }
        public string SupplyId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePosition { get; set; } // Подстановочная колонка
        public string StorageId { get; set; }
        public string StorageZone { get; set; }
        public DateTime AccountingDate { get; set; }
        public int Quantity { get; set; }
        public string LastMovementDate { get; set; }
        public int DaysSinceAccounting { get; set; } // Вычисляемая колонка
        public string IsRecentMovement { get; set; } // Вычисляемая колонка
        public string MovementStatus { get; set; } // Новое поле для выпадающего списка

        public override string ToString()
        {
            return $"{ProductAccId} - {EmployeeName} - {StorageZone}";
        }
    }
}