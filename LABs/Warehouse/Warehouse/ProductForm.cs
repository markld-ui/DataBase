using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Application;
using Domain.Interfaces;
using Domain.Models;

namespace UI
{
    public partial class ProductForm : Form
    {
        private readonly IProductRepository _productRepository;
        private Product _selectedProduct;
        private BindingSource _bindingSource;
        private BindingNavigator _bindingNavigator;
        private ToolStripTextBox _toolStripTextBoxFind;
        private ToolStripButton _toolStripButtonFind;
        private CheckBox _checkBoxFind;
        private ToolStripButton _toolStripButtonReset;

        public ProductForm()
        {
            InitializeComponent();
            _productRepository = Application.AppContext.Instance.ProductRepository;
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
                _selectedProduct = null;
            };
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
            LoadProductTypes();
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productRepository.GetAll();
                _bindingSource.DataSource = new BindingList<Product>(products);
                dataGridViewProducts.DataSource = _bindingSource;

                dataGridViewProducts.Columns["ProductId"].HeaderText = "ID";
                dataGridViewProducts.Columns["Name"].HeaderText = "Название";
                dataGridViewProducts.Columns["ExpiryDate"].HeaderText = "Срок годности";
                dataGridViewProducts.Columns["ProductType"].HeaderText = "Тип продукта";
                dataGridViewProducts.Columns["IsActive"].HeaderText = "Активен";
                dataGridViewProducts.Columns["Photo"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки продуктов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductTypes()
        {
            try
            {
                var productTypes = Enum.GetValues(typeof(ProductType));
                cmbProductType.DataSource = productTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов продуктов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewProducts.SelectedRows[0];
                _selectedProduct = selectedRow.DataBoundItem as Product;
                if (_selectedProduct != null)
                {
                    txtName.Text = _selectedProduct.Name;
                    dtpExpiryDate.Value = _selectedProduct.ExpiryDate ?? DateTime.Now;
                    dtpExpiryDate.Checked = _selectedProduct.ExpiryDate.HasValue;
                    cmbProductType.SelectedItem = _selectedProduct.ProductType;
                    chkIsActive.Checked = _selectedProduct.IsActive;
                    if (_selectedProduct.Photo != null)
                    {
                        using (var ms = new System.IO.MemoryStream(_selectedProduct.Photo))
                        {
                            picPhoto.Image = System.Drawing.Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        picPhoto.Image = null;
                    }
                }
            }
        }

        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    picPhoto.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var product = new Product
                {
                    Name = txtName.Text,
                    ExpiryDate = dtpExpiryDate.Checked ? dtpExpiryDate.Value : null,
                    ProductType = (ProductType)cmbProductType.SelectedItem,
                    IsActive = chkIsActive.Checked,
                    Photo = picPhoto.Image != null ? ImageToByteArray(picPhoto.Image) : null
                };

                _productRepository.Add(product);
                LoadProducts();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления продукта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedProduct == null)
                {
                    MessageBox.Show("Выберите продукт для обновления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!ValidateInput()) return;

                _selectedProduct.Name = txtName.Text;
                _selectedProduct.ExpiryDate = dtpExpiryDate.Checked ? dtpExpiryDate.Value : null;
                _selectedProduct.ProductType = (ProductType)cmbProductType.SelectedItem;
                _selectedProduct.IsActive = chkIsActive.Checked;
                _selectedProduct.Photo = picPhoto.Image != null ? ImageToByteArray(picPhoto.Image) : null;

                _productRepository.Update(_selectedProduct);
                LoadProducts();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления продукта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedProduct == null)
                {
                    MessageBox.Show("Выберите продукт для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Вы уверены, что хотите удалить продукт '{_selectedProduct.Name}'?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _productRepository.Delete(_selectedProduct.ProductId);
                    LoadProducts();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления продукта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter(string searchText)
        {
            try
            {
                var filteredProducts = _productRepository.GetFiltered(searchText);
                _bindingSource.DataSource = new BindingList<Product>(filteredProducts);
                dataGridViewProducts.DataSource = _bindingSource;

                if (_bindingSource.Count == 0)
                {
                    MessageBox.Show("Таких записей не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
                else
                {
                    _bindingSource.Position = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadProducts();
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
            LoadProducts();
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
                LoadProducts();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название продукта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbProductType.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип продукта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtName.Clear();
            dtpExpiryDate.Checked = false;
            dtpExpiryDate.Value = DateTime.Now;
            cmbProductType.SelectedItem = null;
            chkIsActive.Checked = true;
            picPhoto.Image = null;
            _selectedProduct = null;
        }

        private byte[] ImageToByteArray(System.Drawing.Image image)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }
    }
}