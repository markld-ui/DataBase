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
    public partial class EmployeeForm : Form
    {
        private readonly IEmployeeRepository _employeeRepository;
        private Employee _selectedEmployee;
        private BindingSource _bindingSource;
        private BindingNavigator _bindingNavigator;
        private ToolStripTextBox _toolStripTextBoxFind;
        private ToolStripButton _toolStripButtonFind;
        private CheckBox _checkBoxFind;
        private ToolStripButton _toolStripButtonReset;

        public EmployeeForm()
        {
            InitializeComponent();
            _employeeRepository = Application.AppContext.Instance.EmployeeRepository;
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
                _selectedEmployee = null;
            };
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                var employees = _employeeRepository.GetAll();
                _bindingSource.DataSource = new BindingList<Employee>(employees);
                dataGridViewEmployees.DataSource = _bindingSource;

                dataGridViewEmployees.Columns["EmployeeId"].HeaderText = "ID";
                dataGridViewEmployees.Columns["FullName"].HeaderText = "ФИО";
                dataGridViewEmployees.Columns["Position"].HeaderText = "Должность";
                dataGridViewEmployees.Columns["Phone"].HeaderText = "Телефон";
                dataGridViewEmployees.Columns["Photo"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сотрудников: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewEmployees.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewEmployees.SelectedRows[0];
                _selectedEmployee = selectedRow.DataBoundItem as Employee;
                if (_selectedEmployee != null)
                {
                    txtFullName.Text = _selectedEmployee.FullName;
                    txtPosition.Text = _selectedEmployee.Position;
                    txtPhone.Text = _selectedEmployee.Phone;
                    if (_selectedEmployee.Photo != null)
                    {
                        using (var ms = new System.IO.MemoryStream(_selectedEmployee.Photo))
                        {
                            picPhoto.Image = Image.FromStream(ms);
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
                    picPhoto.Image = Image.FromFile(openFileDialog.FileName);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                var employee = new Employee
                {
                    FullName = txtFullName.Text,
                    Position = txtPosition.Text,
                    Phone = txtPhone.Text,
                    Photo = picPhoto.Image != null ? ImageToByteArray(picPhoto.Image) : null
                };

                _employeeRepository.Add(employee);
                LoadEmployees();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления сотрудника: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedEmployee == null)
                {
                    MessageBox.Show("Выберите сотрудника для обновления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput()) return;

                _selectedEmployee.FullName = txtFullName.Text;
                _selectedEmployee.Position = txtPosition.Text;
                _selectedEmployee.Phone = txtPhone.Text;
                _selectedEmployee.Photo = picPhoto.Image != null ? ImageToByteArray(picPhoto.Image) : null;

                _employeeRepository.Update(_selectedEmployee);
                LoadEmployees();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления сотрудника: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedEmployee == null)
                {
                    MessageBox.Show("Выберите сотрудника для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Вы уверены, что хотите удалить сотрудника '{_selectedEmployee.FullName}'?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _employeeRepository.Delete(_selectedEmployee.EmployeeId);
                    LoadEmployees();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления сотрудника: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter(string searchText)
        {
            try
            {
                var filteredEmployees = _employeeRepository.GetFiltered(searchText);
                _bindingSource.DataSource = new BindingList<Employee>(filteredEmployees);
                dataGridViewEmployees.DataSource = _bindingSource;

                if (_bindingSource.Count == 0)
                {
                    MessageBox.Show("Таких записей не найдено.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployees();
                }
                else
                {
                    _bindingSource.Position = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadEmployees();
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
            LoadEmployees();
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
                LoadEmployees();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО сотрудника.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\+?\d{10,15}$"))
                {
                    MessageBox.Show("Введите корректный номер телефона.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void ClearInputs()
        {
            txtFullName.Clear();
            txtPosition.Clear();
            txtPhone.Clear();
            picPhoto.Image = null;
            _selectedEmployee = null;
        }

        private byte[] ImageToByteArray(Image image)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }
    }
}