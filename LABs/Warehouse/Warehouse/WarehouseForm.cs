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
    public partial class WarehouseForm : Form
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private Warehouse _selectedWarehouse;
        private BindingSource _bindingSource;
        private BindingNavigator _bindingNavigator;
        private ToolStripTextBox _toolStripTextBoxFind;
        private ToolStripButton _toolStripButtonFind;
        private CheckBox _checkBoxFind;
        private ToolStripButton _toolStripButtonReset;
        private List<Warehouse> _allWarehouses;

        public WarehouseForm()
        {
            InitializeComponent();
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
        }

        private void WarehouseForm_Load(object sender, EventArgs e)
        {
            LoadWarehouses();
        }

        private void LoadWarehouses()
        {
            try
            {
                _allWarehouses = _warehouseRepository.GetAll();
                _bindingSource.DataSource = new BindingList<Warehouse>(_allWarehouses);
                dataGridViewWarehouses.DataSource = _bindingSource;

                dataGridViewWarehouses.Columns["WarehouseId"].HeaderText = "ID";
                dataGridViewWarehouses.Columns["Name"].HeaderText = "Название";
                dataGridViewWarehouses.Columns["Address"].HeaderText = "Адрес";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки складов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewWarehouses_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewWarehouses.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewWarehouses.SelectedRows[0];
                _selectedWarehouse = selectedRow.DataBoundItem as Warehouse;
                if (_selectedWarehouse != null)
                {
                    txtName.Text = _selectedWarehouse.Name;
                    txtAddress.Text = _selectedWarehouse.Address;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var warehouse = new Warehouse
                {
                    Name = txtName.Text,
                    Address = txtAddress.Text
                };

                _warehouseRepository.Add(warehouse);
                LoadWarehouses();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления склада: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedWarehouse == null)
                {
                    MessageBox.Show("Выберите склад для обновления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!ValidateInput()) return;

                _selectedWarehouse.Name = txtName.Text;
                _selectedWarehouse.Address = txtAddress.Text;

                _warehouseRepository.Update(_selectedWarehouse);
                LoadWarehouses();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления склада: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedWarehouse == null)
                {
                    MessageBox.Show("Выберите склад для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Вы уверены, что хотите удалить склад '{_selectedWarehouse.Name}'?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _warehouseRepository.Delete(_selectedWarehouse.WarehouseId);
                    LoadWarehouses();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления склада: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter(string searchText)
        {
            try
            {
                var filteredWarehouses = _warehouseRepository.GetFiltered(searchText);
                _bindingSource.DataSource = new BindingList<Warehouse>(filteredWarehouses);
                dataGridViewWarehouses.DataSource = _bindingSource;

                if (_bindingSource.Count == 0)
                {
                    MessageBox.Show("Таких записей не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadWarehouses();
                }
                else
                {
                    _bindingSource.Position = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadWarehouses();
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
            LoadWarehouses();
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
                LoadWarehouses();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название склада.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtAddress.Clear();
            _selectedWarehouse = null;
        }
    }
}