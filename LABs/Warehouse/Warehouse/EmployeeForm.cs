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
    /// Форма для управления данными сотрудников
    /// </summary>
    /// <remarks>
    /// <para>
    /// Форма предоставляет функционал для просмотра, добавления, редактирования и удаления сотрудников,
    /// а также для поиска и фильтрации записей.
    /// </para>
    /// <para>
    /// Поддерживает загрузку и отображение фотографий сотрудников.
    /// </para>
    /// </remarks>
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

        /// <summary>
        /// Инициализирует новый экземпляр формы EmployeeForm
        /// </summary>
        public EmployeeForm()
        {
            InitializeComponent();
            _employeeRepository = Application.AppContext.Instance.EmployeeRepository;
            _bindingSource = new BindingSource();

            InitializeBindingNavigator();
            InitializeToolStripControls();
        }

        /// <summary>
        /// Инициализирует BindingNavigator для навигации по записям
        /// </summary>
        private void InitializeBindingNavigator()
        {
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
        }

        /// <summary>
        /// Инициализирует элементы управления ToolStrip
        /// </summary>
        private void InitializeToolStripControls()
        {
            ToolStripSeparator separator = new ToolStripSeparator();
            _bindingNavigator.Items.Add(separator);

            InitializeSearchTextBox();
            InitializeSearchButton();
            InitializeResetButton();
            InitializeFilterCheckBox();

            _bindingNavigator.AddNewItem.Click += (s, args) => ClearInputs();
        }

        private void InitializeSearchTextBox()
        {
            _toolStripTextBoxFind = new ToolStripTextBox();
            _toolStripTextBoxFind.Name = "toolStripTextBoxFind";
            _bindingNavigator.Items.Add(_toolStripTextBoxFind);
        }

        private void InitializeSearchButton()
        {
            _toolStripButtonFind = new ToolStripButton();
            _toolStripButtonFind.Name = "toolStripButtonFind";
            _toolStripButtonFind.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _toolStripButtonFind.Text = "Поиск";
            _toolStripButtonFind.TextAlign = ContentAlignment.MiddleRight;
            _toolStripButtonFind.Image = SystemIcons.Information.ToBitmap();
            _toolStripButtonFind.ImageAlign = ContentAlignment.MiddleLeft;
            _toolStripButtonFind.Click += ToolStripButtonFind_Click;
            _bindingNavigator.Items.Add(_toolStripButtonFind);
        }

        private void InitializeResetButton()
        {
            _toolStripButtonReset = new ToolStripButton();
            _toolStripButtonReset.Name = "toolStripButtonReset";
            _toolStripButtonReset.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            _toolStripButtonReset.Text = "Сброс";
            _toolStripButtonReset.TextAlign = ContentAlignment.MiddleRight;
            _toolStripButtonReset.Image = SystemIcons.Error.ToBitmap();
            _toolStripButtonReset.ImageAlign = ContentAlignment.MiddleLeft;
            _toolStripButtonReset.Click += ToolStripButtonReset_Click;
            _bindingNavigator.Items.Add(_toolStripButtonReset);
        }

        private void InitializeFilterCheckBox()
        {
            _checkBoxFind = new CheckBox();
            _checkBoxFind.Name = "checkBoxFind";
            _checkBoxFind.Text = "Фильтр";
            _checkBoxFind.CheckedChanged += CheckBoxFind_CheckedChanged;
            ToolStripControlHost checkBoxHost = new ToolStripControlHost(_checkBoxFind);
            _bindingNavigator.Items.Add(checkBoxHost);
        }

        /// <summary>
        /// Обработчик загрузки формы
        /// </summary>
        private void EmployeeForm_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        /// <summary>
        /// Загружает список сотрудников из репозитория
        /// </summary>
        private void LoadEmployees()
        {
            try
            {
                var employees = _employeeRepository.GetAll();
                _bindingSource.DataSource = new BindingList<Employee>(employees);
                dataGridViewEmployees.DataSource = _bindingSource;

                ConfigureDataGridViewColumns();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Ошибка загрузки сотрудников: {ex.Message}");
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            dataGridViewEmployees.Columns["EmployeeId"].HeaderText = "ID";
            dataGridViewEmployees.Columns["FullName"].HeaderText = "ФИО";
            dataGridViewEmployees.Columns["Position"].HeaderText = "Должность";
            dataGridViewEmployees.Columns["Phone"].HeaderText = "Телефон";
            dataGridViewEmployees.Columns["Photo"].Visible = false;
        }

        /// <summary>
        /// Обработчик изменения выделенной строки в DataGridView
        /// </summary>
        private void dataGridViewEmployees_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewEmployees.SelectedRows.Count > 0)
                UpdateSelectedEmployee();
        }

        private void UpdateSelectedEmployee()
        {
            var selectedRow = dataGridViewEmployees.SelectedRows[0];
            _selectedEmployee = selectedRow.DataBoundItem as Employee;

            if (_selectedEmployee != null)
                UpdateFormFields();
        }

        private void UpdateFormFields()
        {
            txtFullName.Text = _selectedEmployee.FullName;
            txtPosition.Text = _selectedEmployee.Position;
            txtPhone.Text = _selectedEmployee.Phone;
            UpdateEmployeePhoto();
        }

        private void UpdateEmployeePhoto()
        {
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

        /// <summary>
        /// Обработчик загрузки фотографии сотрудника
        /// </summary>
        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    picPhoto.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        /// <summary>
        /// Обработчик добавления нового сотрудника
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var employee = CreateEmployeeFromInput();
                _employeeRepository.Add(employee);
                LoadEmployees();
                ClearInputs();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Ошибка добавления сотрудника: {ex.Message}");
            }
        }

        private Employee CreateEmployeeFromInput()
        {
            return new Employee
            {
                FullName = txtFullName.Text,
                Position = txtPosition.Text,
                Phone = txtPhone.Text,
                Photo = picPhoto.Image != null ? ImageToByteArray(picPhoto.Image) : null
            };
        }

        /// <summary>
        /// Обработчик обновления данных сотрудника
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedEmployee == null)
            {
                ShowWarningMessage("Выберите сотрудника для обновления.");
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                UpdateEmployeeFromInput();
                _employeeRepository.Update(_selectedEmployee);
                LoadEmployees();
                ClearInputs();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Ошибка обновления сотрудника: {ex.Message}");
            }
        }

        private void UpdateEmployeeFromInput()
        {
            _selectedEmployee.FullName = txtFullName.Text;
            _selectedEmployee.Position = txtPosition.Text;
            _selectedEmployee.Phone = txtPhone.Text;
            _selectedEmployee.Photo = picPhoto.Image != null ? ImageToByteArray(picPhoto.Image) : null;
        }

        /// <summary>
        /// Обработчик удаления сотрудника
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedEmployee == null)
            {
                ShowWarningMessage("Выберите сотрудника для удаления.");
                return;
            }

            if (ConfirmDelete())
            {
                try
                {
                    _employeeRepository.Delete(_selectedEmployee.EmployeeId);
                    LoadEmployees();
                    ClearInputs();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Ошибка удаления сотрудника: {ex.Message}");
                }
            }
        }

        private bool ConfirmDelete()
        {
            return MessageBox.Show($"Вы уверены, что хотите удалить сотрудника '{_selectedEmployee.FullName}'?",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Применяет фильтр к списку сотрудников
        /// </summary>
        /// <param name="searchText">Текст для поиска</param>
        private void ApplyFilter(string searchText)
        {
            try
            {
                var filteredEmployees = _employeeRepository.GetFiltered(searchText);
                _bindingSource.DataSource = new BindingList<Employee>(filteredEmployees);
                dataGridViewEmployees.DataSource = _bindingSource;

                HandleFilterResults(filteredEmployees);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Ошибка фильтрации: {ex.Message}");
                LoadEmployees();
            }
        }

        private void HandleFilterResults(List<Employee> filteredEmployees)
        {
            if (filteredEmployees.Count == 0)
            {
                ShowInfoMessage("Таких записей не найдено.");
                LoadEmployees();
            }
            else
            {
                _bindingSource.Position = 0;
            }
        }

        /// <summary>
        /// Обработчик кнопки поиска
        /// </summary>
        private void ToolStripButtonFind_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_toolStripTextBoxFind.Text))
            {
                ShowInfoMessage("Введите текст для поиска.");
                return;
            }

            ApplyFilter(_toolStripTextBoxFind.Text.Trim());
        }

        /// <summary>
        /// Обработчик кнопки сброса
        /// </summary>
        private void ToolStripButtonReset_Click(object sender, EventArgs e)
        {
            LoadEmployees();
            _toolStripTextBoxFind.Text = null;
        }

        /// <summary>
        /// Обработчик изменения состояния чекбокса фильтра
        /// </summary>
        private void CheckBoxFind_CheckedChanged(object sender, EventArgs e)
        {
            if (_checkBoxFind.Checked)
            {
                if (string.IsNullOrWhiteSpace(_toolStripTextBoxFind.Text))
                {
                    ShowInfoMessage("Введите текст для фильтрации.");
                    _checkBoxFind.Checked = false;
                    return;
                }

                ApplyFilter(_toolStripTextBoxFind.Text.Trim());
            }
            else
            {
                LoadEmployees();
            }
        }

        /// <summary>
        /// Проверяет корректность введенных данных
        /// </summary>
        /// <returns>True если данные валидны, иначе False</returns>
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowWarningMessage("Введите ФИО сотрудника.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !IsValidPhoneNumber(txtPhone.Text))
            {
                ShowWarningMessage("Введите корректный номер телефона.");
                return false;
            }

            return true;
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?\d{10,15}$");
        }

        /// <summary>
        /// Очищает поля ввода
        /// </summary>
        private void ClearInputs()
        {
            txtFullName.Clear();
            txtPosition.Clear();
            txtPhone.Clear();
            picPhoto.Image = null;
            _selectedEmployee = null;
        }

        /// <summary>
        /// Конвертирует изображение в массив байтов
        /// </summary>
        /// <param name="image">Изображение для конвертации</param>
        /// <returns>Массив байтов</returns>
        private byte[] ImageToByteArray(Image image)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}