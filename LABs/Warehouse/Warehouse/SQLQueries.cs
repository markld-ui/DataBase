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
using Infrastructure.Repositories;

namespace UI
{
    public partial class SQLQueries : Form
    {
        private readonly ISQLQueriesRepository _sqlQueriesRepository;

        public SQLQueries()
        {
            InitializeComponent();
            _sqlQueriesRepository = Application.AppContext.Instance.SQLQueriesRepository;
            UpdateSelectTabControlsState();
            UpdateSubqueryTabControlsState();
            UpdateDMLTabControlsState();
        }

        // Валидация ввода
        private bool ValidatePositiveInteger(string input, out int value, TextBox textBox, string fieldName)
        {
            value = 0;
            if (string.IsNullOrEmpty(input))
            {
                textBox.BackColor = Color.White;
                toolTip.SetToolTip(textBox, $"Введите {fieldName} (положительное целое число)");
                return false;
            }
            if (!int.TryParse(input, out value) || value <= 0)
            {
                textBox.BackColor = Color.LightPink;
                toolTip.SetToolTip(textBox, $"Некорректный {fieldName}. Введите положительное целое число!");
                return false;
            }
            textBox.BackColor = Color.White;
            toolTip.SetToolTip(textBox, $"Введите {fieldName} (положительное целое число)");
            return true;
        }

        private bool ValidateDate(string input, out DateTime value, TextBox textBox, string fieldName)
        {
            value = DateTime.MinValue;
            if (string.IsNullOrEmpty(input))
            {
                textBox.BackColor = Color.White;
                toolTip.SetToolTip(textBox, $"Введите {fieldName} в формате ДД.ММ.ГГГГ (например, 20.05.2025)");
                return false;
            }
            if (!DateTime.TryParseExact(input, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out value))
            {
                textBox.BackColor = Color.LightPink;
                toolTip.SetToolTip(textBox, $"Некорректная {fieldName}. Введите дату в формате ДД.ММ.ГГГГ (например, 20.05.2025)!");
                return false;
            }
            textBox.BackColor = Color.White;
            toolTip.SetToolTip(textBox, $"Введите {fieldName} в формате ДД.ММ.ГГГГ (например, 20.05.2025)");
            return true;
        }

        private void ValidateAllInputs()
        {
            ValidateSelectTabInputs();
            ValidateSubqueryTabInputs();
            ValidateDMLTabInputs();
            UpdateExecuteButtonsState();
        }

        private void ValidateSelectTabInputs()
        {
            if (radioButtonFilteredSelect.Checked)
            {
                ValidatePositiveInteger(textBoxEmployeeId.Text, out _, textBoxEmployeeId, "ID сотрудника");
            }
            else if (radioButtonAggregateSelect.Checked)
            {
                ValidateDate(textBoxStartDate.Text, out _, textBoxStartDate, "начальная дата");
            }
            else
            {
                textBoxEmployeeId.BackColor = Color.White;
                textBoxStartDate.BackColor = Color.White;
            }
        }

        private void ValidateSubqueryTabInputs()
        {
            if (radioButtonCorrelatedSubquery.Checked || radioButtonNonCorrelatedSubquery.Checked)
            {
                ValidatePositiveInteger(textBoxSupplyId.Text, out _, textBoxSupplyId, "ID поставки");
            }
            else
            {
                textBoxSupplyId.BackColor = Color.White;
            }
        }

        private void ValidateDMLTabInputs()
        {
            if (radioButtonInsert.Checked)
            {
                ValidateDate(textBoxAccountingDate.Text, out _, textBoxAccountingDate, "дата учёта");
                ValidatePositiveInteger(textBoxQuantity.Text, out _, textBoxQuantity, "количество");
                ValidatePositiveInteger(textBoxEmployeeIdInput.Text, out _, textBoxEmployeeIdInput, "ID сотрудника");
                ValidatePositiveInteger(textBoxSupplyIdInput.Text, out _, textBoxSupplyIdInput, "ID поставки");
                ValidatePositiveInteger(textBoxStorageId.Text, out _, textBoxStorageId, "ID зоны хранения");
            }
            else if (radioButtonUpdate.Checked)
            {
                ValidatePositiveInteger(textBoxId.Text, out _, textBoxId, "ID записи");
                if (!string.IsNullOrEmpty(textBoxAccountingDate.Text))
                    ValidateDate(textBoxAccountingDate.Text, out _, textBoxAccountingDate, "дата учёта");
                else
                    textBoxAccountingDate.BackColor = Color.White;
                if (!string.IsNullOrEmpty(textBoxQuantity.Text))
                    ValidatePositiveInteger(textBoxQuantity.Text, out _, textBoxQuantity, "количество");
                else
                    textBoxQuantity.BackColor = Color.White;
                if (!string.IsNullOrEmpty(textBoxEmployeeIdInput.Text))
                    ValidatePositiveInteger(textBoxEmployeeIdInput.Text, out _, textBoxEmployeeIdInput, "ID сотрудника");
                else
                    textBoxEmployeeIdInput.BackColor = Color.White;
                if (!string.IsNullOrEmpty(textBoxSupplyIdInput.Text))
                    ValidatePositiveInteger(textBoxSupplyIdInput.Text, out _, textBoxSupplyIdInput, "ID поставки");
                else
                    textBoxSupplyIdInput.BackColor = Color.White;
                if (!string.IsNullOrEmpty(textBoxStorageId.Text))
                    ValidatePositiveInteger(textBoxStorageId.Text, out _, textBoxStorageId, "ID зоны хранения");
                else
                    textBoxStorageId.BackColor = Color.White;
            }
            else if (radioButtonDelete.Checked)
            {
                ValidatePositiveInteger(textBoxId.Text, out _, textBoxId, "ID записи");
            }
            else
            {
                textBoxId.BackColor = Color.White;
                textBoxAccountingDate.BackColor = Color.White;
                textBoxQuantity.BackColor = Color.White;
                textBoxEmployeeIdInput.BackColor = Color.White;
                textBoxSupplyIdInput.BackColor = Color.White;
                textBoxStorageId.BackColor = Color.White;
            }
        }

        private void UpdateExecuteButtonsState()
        {
            bool isSelectValid = true;
            if (radioButtonFilteredSelect.Checked && !ValidatePositiveInteger(textBoxEmployeeId.Text, out _, textBoxEmployeeId, "ID сотрудника"))
                isSelectValid = false;
            else if (radioButtonAggregateSelect.Checked && !ValidateDate(textBoxStartDate.Text, out _, textBoxStartDate, "начальная дата"))
                isSelectValid = false;
            buttonExecuteSelect.Enabled = isSelectValid && (radioButtonSimpleSelect.Checked || radioButtonFilteredSelect.Checked || radioButtonMultiTableSelect.Checked || radioButtonAggregateSelect.Checked);

            bool isSubqueryValid = true;
            if ((radioButtonCorrelatedSubquery.Checked || radioButtonNonCorrelatedSubquery.Checked) && !ValidatePositiveInteger(textBoxSupplyId.Text, out _, textBoxSupplyId, "ID поставки"))
                isSubqueryValid = false;
            buttonExecuteSubquery.Enabled = isSubqueryValid && (radioButtonCorrelatedSubquery.Checked || radioButtonNonCorrelatedSubquery.Checked);

            bool isDMLValid = true;
            if (radioButtonInsert.Checked)
            {
                if (!ValidateDate(textBoxAccountingDate.Text, out _, textBoxAccountingDate, "дата учёта") ||
                    !ValidatePositiveInteger(textBoxQuantity.Text, out _, textBoxQuantity, "количество") ||
                    !ValidatePositiveInteger(textBoxEmployeeIdInput.Text, out _, textBoxEmployeeIdInput, "ID сотрудника") ||
                    !ValidatePositiveInteger(textBoxSupplyIdInput.Text, out _, textBoxSupplyIdInput, "ID поставки") ||
                    !ValidatePositiveInteger(textBoxStorageId.Text, out _, textBoxStorageId, "ID зоны хранения"))
                    isDMLValid = false;
            }
            else if (radioButtonUpdate.Checked)
            {
                if (!ValidatePositiveInteger(textBoxId.Text, out _, textBoxId, "ID записи"))
                    isDMLValid = false;
                if (!string.IsNullOrEmpty(textBoxAccountingDate.Text) && !ValidateDate(textBoxAccountingDate.Text, out _, textBoxAccountingDate, "дата учёта"))
                    isDMLValid = false;
                if (!string.IsNullOrEmpty(textBoxQuantity.Text) && !ValidatePositiveInteger(textBoxQuantity.Text, out _, textBoxQuantity, "количество"))
                    isDMLValid = false;
                if (!string.IsNullOrEmpty(textBoxEmployeeIdInput.Text) && !ValidatePositiveInteger(textBoxEmployeeIdInput.Text, out _, textBoxEmployeeIdInput, "ID сотрудника"))
                    isDMLValid = false;
                if (!string.IsNullOrEmpty(textBoxSupplyIdInput.Text) && !ValidatePositiveInteger(textBoxSupplyIdInput.Text, out _, textBoxSupplyIdInput, "ID поставки"))
                    isDMLValid = false;
                if (!string.IsNullOrEmpty(textBoxStorageId.Text) && !ValidatePositiveInteger(textBoxStorageId.Text, out _, textBoxStorageId, "ID зоны хранения"))
                    isDMLValid = false;
            }
            else if (radioButtonDelete.Checked)
            {
                if (!ValidatePositiveInteger(textBoxId.Text, out _, textBoxId, "ID записи"))
                    isDMLValid = false;
            }
            buttonExecuteDML.Enabled = isDMLValid && (radioButtonInsert.Checked || radioButtonUpdate.Checked || radioButtonDelete.Checked);
        }

        private void textBoxEmployeeId_TextChanged(object sender, EventArgs e)
        {
            ValidateSelectTabInputs();
            UpdateExecuteButtonsState();
        }

        private void textBoxStartDate_TextChanged(object sender, EventArgs e)
        {
            ValidateSelectTabInputs();
            UpdateExecuteButtonsState();
        }

        private void textBoxSupplyId_TextChanged(object sender, EventArgs e)
        {
            ValidateSubqueryTabInputs();
            UpdateExecuteButtonsState();
        }

        private void textBoxId_TextChanged(object sender, EventArgs e)
        {
            ValidateDMLTabInputs();
            UpdateExecuteButtonsState();
        }

        private void textBoxAccountingDate_TextChanged(object sender, EventArgs e)
        {
            ValidateDMLTabInputs();
            UpdateExecuteButtonsState();
        }

        private void textBoxQuantity_TextChanged(object sender, EventArgs e)
        {
            ValidateDMLTabInputs();
            UpdateExecuteButtonsState();
        }

        private void textBoxEmployeeIdInput_TextChanged(object sender, EventArgs e)
        {
            ValidateDMLTabInputs();
            UpdateExecuteButtonsState();
        }

        private void textBoxSupplyIdInput_TextChanged(object sender, EventArgs e)
        {
            ValidateDMLTabInputs();
            UpdateExecuteButtonsState();
        }

        private void textBoxStorageId_TextChanged(object sender, EventArgs e)
        {
            ValidateDMLTabInputs();
            UpdateExecuteButtonsState();
        }

        // 4.2.1 - Запросы на выборку
        private void buttonExecuteSelect_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateSelectTabControlsState();
                if (radioButtonSimpleSelect.Checked)
                {
                    dataGridViewSelect.DataSource = _sqlQueriesRepository.GetSimpleProductAccountingRecords();
                }
                else if (radioButtonFilteredSelect.Checked)
                {
                    if (!ValidatePositiveInteger(textBoxEmployeeId.Text, out int employeeId, textBoxEmployeeId, "ID сотрудника"))
                        return;
                    dataGridViewSelect.DataSource = _sqlQueriesRepository.GetRecordsByEmployee(employeeId);
                }
                else if (radioButtonMultiTableSelect.Checked)
                {
                    dataGridViewSelect.DataSource = _sqlQueriesRepository.GetAllRecords();
                }
                else if (radioButtonAggregateSelect.Checked)
                {
                    if (!ValidateDate(textBoxStartDate.Text, out DateTime startDate, textBoxStartDate, "начальная дата"))
                        return;
                    dataGridViewSelect.DataSource = _sqlQueriesRepository.GetAggregateRecords(0, startDate); // 0 для minRecordCount, так как поле удалено
                }
                else
                {
                    MessageBox.Show("Выберите тип выборки!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LocalizeDataGridViewColumns(dataGridViewSelect);
                if (dataGridViewSelect.Rows.Count == 0)
                {
                    MessageBox.Show("Записей не найдено!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                ResetSelectForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButtonSimpleSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSelectTabControlsState();
            ValidateAllInputs();
        }

        private void radioButtonFilteredSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSelectTabControlsState();
            ValidateAllInputs();
        }

        private void radioButtonMultiTableSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSelectTabControlsState();
            ValidateAllInputs();
        }

        private void radioButtonAggregateSelect_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSelectTabControlsState();
            ValidateAllInputs();
        }

        private void UpdateSelectTabControlsState()
        {
            textBoxEmployeeId.Enabled = radioButtonFilteredSelect.Checked;
            textBoxStartDate.Enabled = radioButtonAggregateSelect.Checked;
            if (!radioButtonFilteredSelect.Checked)
                textBoxEmployeeId.Text = string.Empty;
            if (!radioButtonAggregateSelect.Checked)
                textBoxStartDate.Text = string.Empty;
        }

        private void ResetSelectForm()
        {
            textBoxEmployeeId.Text = string.Empty;
            textBoxStartDate.Text = string.Empty;
            radioButtonSimpleSelect.Checked = false;
            radioButtonFilteredSelect.Checked = false;
            radioButtonMultiTableSelect.Checked = false;
            radioButtonAggregateSelect.Checked = false;
            UpdateSelectTabControlsState();
            ValidateAllInputs();
        }

        // 4.2.2 - Подзапросы
        private void buttonExecuteSubquery_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateSubqueryTabControlsState();
                if (radioButtonCorrelatedSubquery.Checked)
                {
                    if (!ValidatePositiveInteger(textBoxSupplyId.Text, out int supplyId, textBoxSupplyId, "ID поставки"))
                        return;
                    dataGridViewSubquery.DataSource = _sqlQueriesRepository.GetCorrelatedSubquery(supplyId);
                }
                else if (radioButtonNonCorrelatedSubquery.Checked)
                {
                    if (!ValidatePositiveInteger(textBoxSupplyId.Text, out int supplyId, textBoxSupplyId, "ID поставки"))
                        return;
                    dataGridViewSubquery.DataSource = _sqlQueriesRepository.GetNonCorrelatedSubquery(supplyId);
                }
                else
                {
                    MessageBox.Show("Выберите тип подзапроса!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                LocalizeDataGridViewColumns(dataGridViewSubquery);
                if (dataGridViewSubquery.Rows.Count == 0)
                {
                    MessageBox.Show("Записей не найдено!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                ResetSubqueryForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButtonCorrelatedSubquery_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSubqueryTabControlsState();
            ValidateAllInputs();
        }

        private void radioButtonNonCorrelatedSubquery_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSubqueryTabControlsState();
            ValidateAllInputs();
        }

        private void UpdateSubqueryTabControlsState()
        {
            textBoxSupplyId.Enabled = radioButtonCorrelatedSubquery.Checked || radioButtonNonCorrelatedSubquery.Checked;
            if (!radioButtonCorrelatedSubquery.Checked && !radioButtonNonCorrelatedSubquery.Checked)
                textBoxSupplyId.Text = string.Empty;
        }

        private void ResetSubqueryForm()
        {
            textBoxSupplyId.Text = string.Empty;
            radioButtonCorrelatedSubquery.Checked = false;
            radioButtonNonCorrelatedSubquery.Checked = false;
            UpdateSubqueryTabControlsState();
            ValidateAllInputs();
        }

        // 4.2.3 - Изменение данных
        private void buttonExecuteDML_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonInsert.Checked)
                {
                    if (!ValidateDate(textBoxAccountingDate.Text, out DateTime accountingDate, textBoxAccountingDate, "дата учёта") ||
                        !ValidatePositiveInteger(textBoxQuantity.Text, out int quantity, textBoxQuantity, "количество") ||
                        !ValidatePositiveInteger(textBoxEmployeeIdInput.Text, out int employeeId, textBoxEmployeeIdInput, "ID сотрудника") ||
                        !ValidatePositiveInteger(textBoxSupplyIdInput.Text, out int supplyId, textBoxSupplyIdInput, "ID поставки") ||
                        !ValidatePositiveInteger(textBoxStorageId.Text, out int storageId, textBoxStorageId, "ID зоны хранения"))
                        return;

                    _sqlQueriesRepository.InsertRecord(accountingDate, quantity, employeeId, supplyId, storageId);
                    MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (radioButtonUpdate.Checked)
                {
                    if (!ValidatePositiveInteger(textBoxId.Text, out int id, textBoxId, "ID записи"))
                        return;
                    if (!_sqlQueriesRepository.RecordExists(id))
                    {
                        MessageBox.Show($"Запись с ID {id} не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    DateTime? accountingDate = null;
                    int? quantity = null;
                    int? employeeId = null;
                    int? supplyId = null;
                    int? storageId = null;

                    if (!string.IsNullOrEmpty(textBoxAccountingDate.Text))
                    {
                        if (!ValidateDate(textBoxAccountingDate.Text, out DateTime parsedDate, textBoxAccountingDate, "дата учёта"))
                            return;
                        accountingDate = parsedDate;
                    }

                    if (!string.IsNullOrEmpty(textBoxQuantity.Text))
                    {
                        if (!ValidatePositiveInteger(textBoxQuantity.Text, out int parsedQuantity, textBoxQuantity, "количество"))
                            return;
                        quantity = parsedQuantity;
                    }

                    if (!string.IsNullOrEmpty(textBoxEmployeeIdInput.Text))
                    {
                        if (!ValidatePositiveInteger(textBoxEmployeeIdInput.Text, out int parsedEmployeeId, textBoxEmployeeIdInput, "ID сотрудника"))
                            return;
                        employeeId = parsedEmployeeId;
                    }

                    if (!string.IsNullOrEmpty(textBoxSupplyIdInput.Text))
                    {
                        if (!ValidatePositiveInteger(textBoxSupplyIdInput.Text, out int parsedSupplyId, textBoxSupplyIdInput, "ID поставки"))
                            return;
                        supplyId = parsedSupplyId;
                    }

                    if (!string.IsNullOrEmpty(textBoxStorageId.Text))
                    {
                        if (!ValidatePositiveInteger(textBoxStorageId.Text, out int parsedStorageId, textBoxStorageId, "ID зоны хранения"))
                            return;
                        storageId = parsedStorageId;
                    }

                    if (!accountingDate.HasValue && !quantity.HasValue && !employeeId.HasValue && !supplyId.HasValue && !storageId.HasValue)
                    {
                        MessageBox.Show("Введите хотя бы одно поле для обновления!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    _sqlQueriesRepository.UpdateRecord(id, accountingDate, quantity, employeeId, supplyId, storageId);
                    MessageBox.Show("Запись обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (radioButtonDelete.Checked)
                {
                    if (!ValidatePositiveInteger(textBoxId.Text, out int id, textBoxId, "ID записи"))
                        return;
                    if (!_sqlQueriesRepository.RecordExists(id))
                    {
                        MessageBox.Show($"Запись с ID {id} не существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    _sqlQueriesRepository.DeleteRecord(id);
                    MessageBox.Show("Запись удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Выберите действие!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ResetDMLForm();
                RefreshDMLTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshDMLTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radioButtonInsert_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDMLTabControlsState();
            ValidateAllInputs();
        }

        private void radioButtonUpdate_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDMLTabControlsState();
            ValidateAllInputs();
        }

        private void radioButtonDelete_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDMLTabControlsState();
            ValidateAllInputs();
        }

        private void UpdateDMLTabControlsState()
        {
            textBoxId.Enabled = radioButtonUpdate.Checked || radioButtonDelete.Checked;
            textBoxAccountingDate.Enabled = radioButtonInsert.Checked || radioButtonUpdate.Checked;
            textBoxQuantity.Enabled = radioButtonInsert.Checked || radioButtonUpdate.Checked;
            textBoxEmployeeIdInput.Enabled = radioButtonInsert.Checked || radioButtonUpdate.Checked;
            textBoxSupplyIdInput.Enabled = radioButtonInsert.Checked || radioButtonUpdate.Checked;
            textBoxStorageId.Enabled = radioButtonInsert.Checked || radioButtonUpdate.Checked;

            if (!radioButtonUpdate.Checked && !radioButtonDelete.Checked)
                textBoxId.Text = string.Empty;
            if (!radioButtonInsert.Checked && !radioButtonUpdate.Checked)
            {
                textBoxAccountingDate.Text = string.Empty;
                textBoxQuantity.Text = string.Empty;
                textBoxEmployeeIdInput.Text = string.Empty;
                textBoxSupplyIdInput.Text = string.Empty;
                textBoxStorageId.Text = string.Empty;
            }
        }

        private void ResetDMLForm()
        {
            textBoxId.Text = string.Empty;
            textBoxAccountingDate.Text = string.Empty;
            textBoxQuantity.Text = string.Empty;
            textBoxEmployeeIdInput.Text = string.Empty;
            textBoxSupplyIdInput.Text = string.Empty;
            textBoxStorageId.Text = string.Empty;
            radioButtonInsert.Checked = false;
            radioButtonUpdate.Checked = false;
            radioButtonDelete.Checked = false;
            UpdateDMLTabControlsState();
            ValidateAllInputs();
        }

        private void RefreshDMLTable()
        {
            dataGridViewDML.DataSource = _sqlQueriesRepository.GetAllRecords();
            LocalizeDataGridViewColumns(dataGridViewDML);
            if (dataGridViewDML.Rows.Count == 0)
            {
                MessageBox.Show("Записей не найдено!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LocalizeDataGridViewColumns(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                switch (column.Name.ToLower())
                {
                    case "id":
                        column.HeaderText = "ID записи";
                        break;
                    case "accountingdate":
                        column.HeaderText = "Дата учёта";
                        break;
                    case "quantity":
                        column.HeaderText = "Количество";
                        break;
                    case "employeeid":
                        column.HeaderText = "ID сотрудника";
                        break;
                    case "supplyid":
                        column.HeaderText = "ID поставки";
                        break;
                    case "storageid":
                        column.HeaderText = "ID зоны хранения";
                        break;
                    case "totalquantity":
                        column.HeaderText = "Общее количество";
                        break;
                    default:
                        column.HeaderText = column.Name;
                        break;
                }
            }
        }
    }
}

