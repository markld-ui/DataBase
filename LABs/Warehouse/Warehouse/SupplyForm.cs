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

        private void SupplyForm_Load(object sender, EventArgs e)
        {
            LoadSupplies();
            LoadProducts();
            LoadSuppliers();
        }

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
            LoadSupplies();
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
                LoadSupplies();
            }
        }

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

        private void ClearInputs()
        {
            cmbProduct.SelectedIndex = -1;
            cmbSupplier.SelectedIndex = -1;
            dtpSupplyDate.Value = DateTime.Now;
            txtQuantity.Clear();
            _selectedSupply = null;
        }

        // Временный класс для поддержки BindingNavigator
        private class SupplyView
        {
            public int SupplyId { get; set; }
            public string ProductName { get; set; }
            public string SupplierName { get; set; }
            public DateTime SupplyDate { get; set; }
            public int Quantity { get; set; }
        }
    }
}