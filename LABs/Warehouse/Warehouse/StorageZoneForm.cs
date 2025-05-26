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

        private void StorageZoneForm_Load(object sender, EventArgs e)
        {
            LoadStorageZones();
            LoadWarehouses();
            LoadZoneTypes();
        }

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
            LoadStorageZones();
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
                LoadStorageZones();
            }
        }

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

        private void ClearInputs()
        {
            cmbWarehouse.SelectedIndex = -1;
            txtCapacity.Clear();
            cmbZoneType.SelectedIndex = -1;
            txtZoneName.Clear();
            _selectedStorageZone = null;
        }

        private class StorageZoneView
        {
            public int StorageId { get; set; }
            public string WarehouseName { get; set; }
            public int Capacity { get; set; }
            public ZoneType ZoneType { get; set; }
            public string ZoneName { get; set; }
        }
    }
}