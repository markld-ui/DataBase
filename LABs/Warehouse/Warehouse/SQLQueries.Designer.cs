namespace UI
{
    partial class SQLQueries
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControlSQL = new System.Windows.Forms.TabControl();
            this.tabPageSelect = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSelect = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxSelect = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelSelectRadio = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButtonSimpleSelect = new System.Windows.Forms.RadioButton();
            this.radioButtonFilteredSelect = new System.Windows.Forms.RadioButton();
            this.radioButtonMultiTableSelect = new System.Windows.Forms.RadioButton();
            this.radioButtonAggregateSelect = new System.Windows.Forms.RadioButton();
            this.panelSelectInput = new System.Windows.Forms.Panel();
            this.tableLayoutPanelSelectInput = new System.Windows.Forms.TableLayoutPanel();
            this.labelEmployeeId = new System.Windows.Forms.Label();
            this.textBoxEmployeeId = new System.Windows.Forms.TextBox();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.textBoxStartDate = new System.Windows.Forms.TextBox();
            this.flowLayoutPanelSelectButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonExecuteSelect = new System.Windows.Forms.Button();
            this.dataGridViewSelect = new System.Windows.Forms.DataGridView();
            this.tabPageSubquery = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelSubquery = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxSubquery = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelSubqueryRadio = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButtonCorrelatedSubquery = new System.Windows.Forms.RadioButton();
            this.radioButtonNonCorrelatedSubquery = new System.Windows.Forms.RadioButton();
            this.panelSubqueryInput = new System.Windows.Forms.Panel();
            this.tableLayoutPanelSubqueryInput = new System.Windows.Forms.TableLayoutPanel();
            this.labelSupplyId = new System.Windows.Forms.Label();
            this.textBoxSupplyId = new System.Windows.Forms.TextBox();
            this.flowLayoutPanelSubqueryButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonExecuteSubquery = new System.Windows.Forms.Button();
            this.dataGridViewSubquery = new System.Windows.Forms.DataGridView();
            this.tabPageDML = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelDML = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxDML = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelDMLRadio = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButtonInsert = new System.Windows.Forms.RadioButton();
            this.radioButtonUpdate = new System.Windows.Forms.RadioButton();
            this.radioButtonDelete = new System.Windows.Forms.RadioButton();
            this.panelInput = new System.Windows.Forms.Panel();
            this.tableLayoutPanelInput = new System.Windows.Forms.TableLayoutPanel();
            this.labelId = new System.Windows.Forms.Label();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.labelAccountingDate = new System.Windows.Forms.Label();
            this.textBoxAccountingDate = new System.Windows.Forms.TextBox();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.textBoxQuantity = new System.Windows.Forms.TextBox();
            this.labelEmployeeIdInput = new System.Windows.Forms.Label();
            this.textBoxEmployeeIdInput = new System.Windows.Forms.TextBox();
            this.labelSupplyIdInput = new System.Windows.Forms.Label();
            this.textBoxSupplyIdInput = new System.Windows.Forms.TextBox();
            this.labelStorageId = new System.Windows.Forms.Label();
            this.textBoxStorageId = new System.Windows.Forms.TextBox();
            this.flowLayoutPanelDMLButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonExecuteDML = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.dataGridViewDML = new System.Windows.Forms.DataGridView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControlSQL.SuspendLayout();
            this.tabPageSelect.SuspendLayout();
            this.tableLayoutPanelSelect.SuspendLayout();
            this.groupBoxSelect.SuspendLayout();
            this.flowLayoutPanelSelectRadio.SuspendLayout();
            this.panelSelectInput.SuspendLayout();
            this.tableLayoutPanelSelectInput.SuspendLayout();
            this.flowLayoutPanelSelectButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelect)).BeginInit();
            this.tabPageSubquery.SuspendLayout();
            this.tableLayoutPanelSubquery.SuspendLayout();
            this.groupBoxSubquery.SuspendLayout();
            this.flowLayoutPanelSubqueryRadio.SuspendLayout();
            this.panelSubqueryInput.SuspendLayout();
            this.tableLayoutPanelSubqueryInput.SuspendLayout();
            this.flowLayoutPanelSubqueryButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubquery)).BeginInit();
            this.tabPageDML.SuspendLayout();
            this.tableLayoutPanelDML.SuspendLayout();
            this.groupBoxDML.SuspendLayout();
            this.flowLayoutPanelDMLRadio.SuspendLayout();
            this.panelInput.SuspendLayout();
            this.tableLayoutPanelInput.SuspendLayout();
            this.flowLayoutPanelDMLButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDML)).BeginInit();
            this.SuspendLayout();

            // tabControlSQL
            this.tabControlSQL.Controls.Add(this.tabPageSelect);
            this.tabControlSQL.Controls.Add(this.tabPageSubquery);
            this.tabControlSQL.Controls.Add(this.tabPageDML);
            this.tabControlSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSQL.Location = new System.Drawing.Point(0, 0);
            this.tabControlSQL.Name = "tabControlSQL";
            this.tabControlSQL.SelectedIndex = 0;
            this.tabControlSQL.Size = new System.Drawing.Size(800, 600);
            this.tabControlSQL.TabIndex = 0;
            this.tabControlSQL.Margin = new System.Windows.Forms.Padding(5);

            // tabPageSelect (4.2.1 - Запросы на выборку)
            this.tabPageSelect.Controls.Add(this.tableLayoutPanelSelect);
            this.tabPageSelect.Location = new System.Drawing.Point(4, 22);
            this.tabPageSelect.Name = "tabPageSelect";
            this.tabPageSelect.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageSelect.Size = new System.Drawing.Size(792, 574);
            this.tabPageSelect.TabIndex = 0;
            this.tabPageSelect.Text = "Запросы на выборку";
            this.tabPageSelect.UseVisualStyleBackColor = true;

            // tableLayoutPanelSelect
            this.tableLayoutPanelSelect.ColumnCount = 1;
            this.tableLayoutPanelSelect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSelect.Controls.Add(this.groupBoxSelect, 0, 0);
            this.tableLayoutPanelSelect.Controls.Add(this.panelSelectInput, 0, 1);
            this.tableLayoutPanelSelect.Controls.Add(this.flowLayoutPanelSelectButtons, 0, 2);
            this.tableLayoutPanelSelect.Controls.Add(this.dataGridViewSelect, 0, 3);
            this.tableLayoutPanelSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSelect.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanelSelect.Name = "tableLayoutPanelSelect";
            this.tableLayoutPanelSelect.RowCount = 4;
            this.tableLayoutPanelSelect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelSelect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelSelect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelSelect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelSelect.Size = new System.Drawing.Size(772, 554);
            this.tableLayoutPanelSelect.TabIndex = 0;
            this.tableLayoutPanelSelect.Margin = new System.Windows.Forms.Padding(5);

            // groupBoxSelect
            this.groupBoxSelect.Controls.Add(this.flowLayoutPanelSelectRadio);
            this.groupBoxSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSelect.Location = new System.Drawing.Point(8, 8);
            this.groupBoxSelect.Name = "groupBoxSelect";
            this.groupBoxSelect.Size = new System.Drawing.Size(756, 110);
            this.groupBoxSelect.TabIndex = 0;
            this.groupBoxSelect.TabStop = false;
            this.groupBoxSelect.Text = "Тип выборки";
            this.groupBoxSelect.Margin = new System.Windows.Forms.Padding(5);

            // flowLayoutPanelSelectRadio
            this.flowLayoutPanelSelectRadio.AutoSize = true;
            this.flowLayoutPanelSelectRadio.Controls.Add(this.radioButtonSimpleSelect);
            this.flowLayoutPanelSelectRadio.Controls.Add(this.radioButtonFilteredSelect);
            this.flowLayoutPanelSelectRadio.Controls.Add(this.radioButtonMultiTableSelect);
            this.flowLayoutPanelSelectRadio.Controls.Add(this.radioButtonAggregateSelect);
            this.flowLayoutPanelSelectRadio.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelSelectRadio.Location = new System.Drawing.Point(15, 25);
            this.flowLayoutPanelSelectRadio.Name = "flowLayoutPanelSelectRadio";
            this.flowLayoutPanelSelectRadio.TabIndex = 0;
            this.flowLayoutPanelSelectRadio.Margin = new System.Windows.Forms.Padding(5);

            // radioButtonSimpleSelect
            this.radioButtonSimpleSelect.AutoSize = true;
            this.radioButtonSimpleSelect.Location = new System.Drawing.Point(5, 5);
            this.radioButtonSimpleSelect.Name = "radioButtonSimpleSelect";
            this.radioButtonSimpleSelect.Size = new System.Drawing.Size(150, 17);
            this.radioButtonSimpleSelect.TabIndex = 0;
            this.radioButtonSimpleSelect.Text = "Выборка из одной таблицы";
            this.radioButtonSimpleSelect.UseVisualStyleBackColor = true;
            this.radioButtonSimpleSelect.CheckedChanged += new System.EventHandler(this.radioButtonSimpleSelect_CheckedChanged);

            // radioButtonFilteredSelect
            this.radioButtonFilteredSelect.AutoSize = true;
            this.radioButtonFilteredSelect.Location = new System.Drawing.Point(5, 28);
            this.radioButtonFilteredSelect.Name = "radioButtonFilteredSelect";
            this.radioButtonFilteredSelect.Size = new System.Drawing.Size(130, 17);
            this.radioButtonFilteredSelect.TabIndex = 1;
            this.radioButtonFilteredSelect.Text = "Выборка с условием";
            this.radioButtonFilteredSelect.UseVisualStyleBackColor = true;
            this.radioButtonFilteredSelect.CheckedChanged += new System.EventHandler(this.radioButtonFilteredSelect_CheckedChanged);

            // radioButtonMultiTableSelect
            this.radioButtonMultiTableSelect.AutoSize = true;
            this.radioButtonMultiTableSelect.Location = new System.Drawing.Point(5, 51);
            this.radioButtonMultiTableSelect.Name = "radioButtonMultiTableSelect";
            this.radioButtonMultiTableSelect.Size = new System.Drawing.Size(180, 17);
            this.radioButtonMultiTableSelect.TabIndex = 2;
            this.radioButtonMultiTableSelect.Text = "Выборка из нескольких таблиц";
            this.radioButtonMultiTableSelect.UseVisualStyleBackColor = true;
            this.radioButtonMultiTableSelect.CheckedChanged += new System.EventHandler(this.radioButtonMultiTableSelect_CheckedChanged);

            // radioButtonAggregateSelect
            this.radioButtonAggregateSelect.AutoSize = true;
            this.radioButtonAggregateSelect.Location = new System.Drawing.Point(5, 74);
            this.radioButtonAggregateSelect.Name = "radioButtonAggregateSelect";
            this.radioButtonAggregateSelect.Size = new System.Drawing.Size(140, 17);
            this.radioButtonAggregateSelect.TabIndex = 3;
            this.radioButtonAggregateSelect.Text = "Агрегирующая выборка";
            this.radioButtonAggregateSelect.UseVisualStyleBackColor = true;
            this.radioButtonAggregateSelect.CheckedChanged += new System.EventHandler(this.radioButtonAggregateSelect_CheckedChanged);

            // panelSelectInput
            this.panelSelectInput.Controls.Add(this.tableLayoutPanelSelectInput);
            this.panelSelectInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectInput.Location = new System.Drawing.Point(8, 118);
            this.panelSelectInput.Name = "panelSelectInput";
            this.panelSelectInput.Size = new System.Drawing.Size(756, 102);
            this.panelSelectInput.TabIndex = 1;
            this.panelSelectInput.Margin = new System.Windows.Forms.Padding(5);

            // tableLayoutPanelSelectInput
            this.tableLayoutPanelSelectInput.ColumnCount = 4;
            this.tableLayoutPanelSelectInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelSelectInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelSelectInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelSelectInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelSelectInput.Controls.Add(this.labelEmployeeId, 0, 0);
            this.tableLayoutPanelSelectInput.Controls.Add(this.textBoxEmployeeId, 1, 0);
            this.tableLayoutPanelSelectInput.Controls.Add(this.labelStartDate, 0, 1);
            this.tableLayoutPanelSelectInput.Controls.Add(this.textBoxStartDate, 1, 1);
            this.tableLayoutPanelSelectInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSelectInput.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSelectInput.Name = "tableLayoutPanelSelectInput";
            this.tableLayoutPanelSelectInput.RowCount = 2;
            this.tableLayoutPanelSelectInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelectInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSelectInput.Size = new System.Drawing.Size(756, 102);
            this.tableLayoutPanelSelectInput.TabIndex = 0;
            this.tableLayoutPanelSelectInput.Margin = new System.Windows.Forms.Padding(5);

            // labelEmployeeId
            this.labelEmployeeId.AutoSize = true;
            this.labelEmployeeId.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelEmployeeId.Location = new System.Drawing.Point(8, 8);
            this.labelEmployeeId.Name = "labelEmployeeId";
            this.labelEmployeeId.Size = new System.Drawing.Size(93, 13);
            this.labelEmployeeId.TabIndex = 0;
            this.labelEmployeeId.Text = "ID сотрудника";
            this.labelEmployeeId.Margin = new System.Windows.Forms.Padding(5);

            // textBoxEmployeeId
            this.textBoxEmployeeId.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxEmployeeId.Location = new System.Drawing.Point(194, 8);
            this.textBoxEmployeeId.Name = "textBoxEmployeeId";
            this.textBoxEmployeeId.Size = new System.Drawing.Size(120, 20);
            this.textBoxEmployeeId.TabIndex = 1;
            this.textBoxEmployeeId.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxEmployeeId.TextChanged += new System.EventHandler(this.textBoxEmployeeId_TextChanged);

            // labelStartDate
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStartDate.Location = new System.Drawing.Point(8, 59);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(93, 13);
            this.labelStartDate.TabIndex = 2;
            this.labelStartDate.Text = "Начальная дата";
            this.labelStartDate.Margin = new System.Windows.Forms.Padding(5);

            // textBoxStartDate
            this.textBoxStartDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxStartDate.Location = new System.Drawing.Point(194, 59);
            this.textBoxStartDate.Name = "textBoxStartDate";
            this.textBoxStartDate.Size = new System.Drawing.Size(120, 20);
            this.textBoxStartDate.TabIndex = 3;
            this.textBoxStartDate.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxStartDate.TextChanged += new System.EventHandler(this.textBoxStartDate_TextChanged);

            // flowLayoutPanelSelectButtons
            this.flowLayoutPanelSelectButtons.Controls.Add(this.buttonExecuteSelect);
            this.flowLayoutPanelSelectButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelSelectButtons.Location = new System.Drawing.Point(8, 228);
            this.flowLayoutPanelSelectButtons.Name = "flowLayoutPanelSelectButtons";
            this.flowLayoutPanelSelectButtons.Size = new System.Drawing.Size(756, 42);
            this.flowLayoutPanelSelectButtons.TabIndex = 2;
            this.flowLayoutPanelSelectButtons.Margin = new System.Windows.Forms.Padding(5);

            // buttonExecuteSelect
            this.buttonExecuteSelect.AutoSize = true;
            this.buttonExecuteSelect.Location = new System.Drawing.Point(606, 8);
            this.buttonExecuteSelect.Name = "buttonExecuteSelect";
            this.buttonExecuteSelect.Size = new System.Drawing.Size(141, 23);
            this.buttonExecuteSelect.TabIndex = 0;
            this.buttonExecuteSelect.Text = "Выполнить запрос";
            this.buttonExecuteSelect.UseVisualStyleBackColor = true;
            this.buttonExecuteSelect.Click += new System.EventHandler(this.buttonExecuteSelect_Click);
            this.buttonExecuteSelect.Margin = new System.Windows.Forms.Padding(5);

            // dataGridViewSelect
            this.dataGridViewSelect.AllowUserToAddRows = false;
            this.dataGridViewSelect.AllowUserToDeleteRows = false;
            this.dataGridViewSelect.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSelect.Location = new System.Drawing.Point(8, 278);
            this.dataGridViewSelect.Name = "dataGridViewSelect";
            this.dataGridViewSelect.ReadOnly = true;
            this.dataGridViewSelect.Size = new System.Drawing.Size(756, 268);
            this.dataGridViewSelect.TabIndex = 3;
            this.dataGridViewSelect.Margin = new System.Windows.Forms.Padding(5);

            // tabPageSubquery (4.2.2 - Подзапросы)
            this.tabPageSubquery.Controls.Add(this.tableLayoutPanelSubquery);
            this.tabPageSubquery.Location = new System.Drawing.Point(4, 22);
            this.tabPageSubquery.Name = "tabPageSubquery";
            this.tabPageSubquery.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageSubquery.Size = new System.Drawing.Size(792, 574);
            this.tabPageSubquery.TabIndex = 1;
            this.tabPageSubquery.Text = "Подзапросы";
            this.tabPageSubquery.UseVisualStyleBackColor = true;

            // tableLayoutPanelSubquery
            this.tableLayoutPanelSubquery.ColumnCount = 1;
            this.tableLayoutPanelSubquery.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSubquery.Controls.Add(this.groupBoxSubquery, 0, 0);
            this.tableLayoutPanelSubquery.Controls.Add(this.panelSubqueryInput, 0, 1);
            this.tableLayoutPanelSubquery.Controls.Add(this.flowLayoutPanelSubqueryButtons, 0, 2);
            this.tableLayoutPanelSubquery.Controls.Add(this.dataGridViewSubquery, 0, 3);
            this.tableLayoutPanelSubquery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSubquery.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanelSubquery.Name = "tableLayoutPanelSubquery";
            this.tableLayoutPanelSubquery.RowCount = 4;
            this.tableLayoutPanelSubquery.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelSubquery.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelSubquery.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelSubquery.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelSubquery.Size = new System.Drawing.Size(772, 554);
            this.tableLayoutPanelSubquery.TabIndex = 0;
            this.tableLayoutPanelSubquery.Margin = new System.Windows.Forms.Padding(5);

            // groupBoxSubquery
            this.groupBoxSubquery.Controls.Add(this.flowLayoutPanelSubqueryRadio);
            this.groupBoxSubquery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxSubquery.Location = new System.Drawing.Point(8, 8);
            this.groupBoxSubquery.Name = "groupBoxSubquery";
            this.groupBoxSubquery.Size = new System.Drawing.Size(756, 102);
            this.groupBoxSubquery.TabIndex = 0;
            this.groupBoxSubquery.TabStop = false;
            this.groupBoxSubquery.Text = "Тип подзапроса";
            this.groupBoxSubquery.Margin = new System.Windows.Forms.Padding(5);

            // flowLayoutPanelSubqueryRadio
            this.flowLayoutPanelSubqueryRadio.AutoSize = true;
            this.flowLayoutPanelSubqueryRadio.Controls.Add(this.radioButtonCorrelatedSubquery);
            this.flowLayoutPanelSubqueryRadio.Controls.Add(this.radioButtonNonCorrelatedSubquery);
            this.flowLayoutPanelSubqueryRadio.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelSubqueryRadio.Location = new System.Drawing.Point(15, 25);
            this.flowLayoutPanelSubqueryRadio.Name = "flowLayoutPanelSubqueryRadio";
            this.flowLayoutPanelSubqueryRadio.Size = new System.Drawing.Size(200, 60);
            this.flowLayoutPanelSubqueryRadio.TabIndex = 0;
            this.flowLayoutPanelSubqueryRadio.Margin = new System.Windows.Forms.Padding(5);

            // radioButtonCorrelatedSubquery
            this.radioButtonCorrelatedSubquery.AutoSize = true;
            this.radioButtonCorrelatedSubquery.Location = new System.Drawing.Point(5, 5);
            this.radioButtonCorrelatedSubquery.Name = "radioButtonCorrelatedSubquery";
            this.radioButtonCorrelatedSubquery.Size = new System.Drawing.Size(170, 17);
            this.radioButtonCorrelatedSubquery.TabIndex = 0;
            this.radioButtonCorrelatedSubquery.Text = "Коррелированный подзапрос";
            this.radioButtonCorrelatedSubquery.UseVisualStyleBackColor = true;
            this.radioButtonCorrelatedSubquery.CheckedChanged += new System.EventHandler(this.radioButtonCorrelatedSubquery_CheckedChanged);

            // radioButtonNonCorrelatedSubquery
            this.radioButtonNonCorrelatedSubquery.AutoSize = true;
            this.radioButtonNonCorrelatedSubquery.Location = new System.Drawing.Point(5, 28);
            this.radioButtonNonCorrelatedSubquery.Name = "radioButtonNonCorrelatedSubquery";
            this.radioButtonNonCorrelatedSubquery.Size = new System.Drawing.Size(180, 17);
            this.radioButtonNonCorrelatedSubquery.TabIndex = 1;
            this.radioButtonNonCorrelatedSubquery.Text = "Некоррелированный подзапрос";
            this.radioButtonNonCorrelatedSubquery.UseVisualStyleBackColor = true;
            this.radioButtonNonCorrelatedSubquery.CheckedChanged += new System.EventHandler(this.radioButtonNonCorrelatedSubquery_CheckedChanged);

            // panelSubqueryInput
            this.panelSubqueryInput.Controls.Add(this.tableLayoutPanelSubqueryInput);
            this.panelSubqueryInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSubqueryInput.Location = new System.Drawing.Point(8, 118);
            this.panelSubqueryInput.Name = "panelSubqueryInput";
            this.panelSubqueryInput.Size = new System.Drawing.Size(756, 102);
            this.panelSubqueryInput.TabIndex = 1;
            this.panelSubqueryInput.Margin = new System.Windows.Forms.Padding(5);

            // tableLayoutPanelSubqueryInput
            this.tableLayoutPanelSubqueryInput.ColumnCount = 2;
            this.tableLayoutPanelSubqueryInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSubqueryInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSubqueryInput.Controls.Add(this.labelSupplyId, 0, 0);
            this.tableLayoutPanelSubqueryInput.Controls.Add(this.textBoxSupplyId, 1, 0);
            this.tableLayoutPanelSubqueryInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSubqueryInput.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSubqueryInput.Name = "tableLayoutPanelSubqueryInput";
            this.tableLayoutPanelSubqueryInput.RowCount = 1;
            this.tableLayoutPanelSubqueryInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSubqueryInput.Size = new System.Drawing.Size(756, 102);
            this.tableLayoutPanelSubqueryInput.TabIndex = 0;
            this.tableLayoutPanelSubqueryInput.Margin = new System.Windows.Forms.Padding(5);

            // labelSupplyId
            this.labelSupplyId.AutoSize = true;
            this.labelSupplyId.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSupplyId.Location = new System.Drawing.Point(8, 8);
            this.labelSupplyId.Name = "labelSupplyId";
            this.labelSupplyId.Size = new System.Drawing.Size(93, 13);
            this.labelSupplyId.TabIndex = 0;
            this.labelSupplyId.Text = "ID поставки";
            this.labelSupplyId.Margin = new System.Windows.Forms.Padding(5);

            // textBoxSupplyId
            this.textBoxSupplyId.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxSupplyId.Location = new System.Drawing.Point(383, 8);
            this.textBoxSupplyId.Name = "textBoxSupplyId";
            this.textBoxSupplyId.Size = new System.Drawing.Size(120, 20);
            this.textBoxSupplyId.TabIndex = 1;
            this.textBoxSupplyId.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxSupplyId.TextChanged += new System.EventHandler(this.textBoxSupplyId_TextChanged);

            // flowLayoutPanelSubqueryButtons
            this.flowLayoutPanelSubqueryButtons.Controls.Add(this.buttonExecuteSubquery);
            this.flowLayoutPanelSubqueryButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelSubqueryButtons.Location = new System.Drawing.Point(8, 228);
            this.flowLayoutPanelSubqueryButtons.Name = "flowLayoutPanelSubqueryButtons";
            this.flowLayoutPanelSubqueryButtons.Size = new System.Drawing.Size(756, 42);
            this.flowLayoutPanelSubqueryButtons.TabIndex = 2;
            this.flowLayoutPanelSubqueryButtons.Margin = new System.Windows.Forms.Padding(5);

            // buttonExecuteSubquery
            this.buttonExecuteSubquery.AutoSize = true;
            this.buttonExecuteSubquery.Location = new System.Drawing.Point(606, 8);
            this.buttonExecuteSubquery.Name = "buttonExecuteSubquery";
            this.buttonExecuteSubquery.Size = new System.Drawing.Size(141, 23);
            this.buttonExecuteSubquery.TabIndex = 0;
            this.buttonExecuteSubquery.Text = "Выполнить запрос";
            this.buttonExecuteSubquery.UseVisualStyleBackColor = true;
            this.buttonExecuteSubquery.Click += new System.EventHandler(this.buttonExecuteSubquery_Click);
            this.buttonExecuteSubquery.Margin = new System.Windows.Forms.Padding(5);

            // dataGridViewSubquery
            this.dataGridViewSubquery.AllowUserToAddRows = false;
            this.dataGridViewSubquery.AllowUserToDeleteRows = false;
            this.dataGridViewSubquery.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSubquery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSubquery.Location = new System.Drawing.Point(8, 278);
            this.dataGridViewSubquery.Name = "dataGridViewSubquery";
            this.dataGridViewSubquery.ReadOnly = true;
            this.dataGridViewSubquery.Size = new System.Drawing.Size(756, 268);
            this.dataGridViewSubquery.TabIndex = 3;
            this.dataGridViewSubquery.Margin = new System.Windows.Forms.Padding(5);

            // tabPageDML (4.2.3 - Изменение данных)
            this.tabPageDML.Controls.Add(this.tableLayoutPanelDML);
            this.tabPageDML.Location = new System.Drawing.Point(4, 22);
            this.tabPageDML.Name = "tabPageDML";
            this.tabPageDML.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageDML.Size = new System.Drawing.Size(792, 574);
            this.tabPageDML.TabIndex = 2;
            this.tabPageDML.Text = "Изменение данных";
            this.tabPageDML.UseVisualStyleBackColor = true;

            // tableLayoutPanelDML
            this.tableLayoutPanelDML.ColumnCount = 1;
            this.tableLayoutPanelDML.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDML.Controls.Add(this.groupBoxDML, 0, 0);
            this.tableLayoutPanelDML.Controls.Add(this.panelInput, 0, 1);
            this.tableLayoutPanelDML.Controls.Add(this.flowLayoutPanelDMLButtons, 0, 2);
            this.tableLayoutPanelDML.Controls.Add(this.dataGridViewDML, 0, 3);
            this.tableLayoutPanelDML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDML.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanelDML.Name = "tableLayoutPanelDML";
            this.tableLayoutPanelDML.RowCount = 4;
            this.tableLayoutPanelDML.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelDML.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelDML.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanelDML.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelDML.Size = new System.Drawing.Size(772, 554);
            this.tableLayoutPanelDML.TabIndex = 0;
            this.tableLayoutPanelDML.Margin = new System.Windows.Forms.Padding(5);

            // groupBoxDML
            this.groupBoxDML.Controls.Add(this.flowLayoutPanelDMLRadio);
            this.groupBoxDML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxDML.Location = new System.Drawing.Point(8, 8);
            this.groupBoxDML.Name = "groupBoxDML";
            this.groupBoxDML.Size = new System.Drawing.Size(756, 102);
            this.groupBoxDML.TabIndex = 0;
            this.groupBoxDML.TabStop = false;
            this.groupBoxDML.Text = "Операции";
            this.groupBoxDML.Margin = new System.Windows.Forms.Padding(5);

            // flowLayoutPanelDMLRadio
            this.flowLayoutPanelDMLRadio.AutoSize = true;
            this.flowLayoutPanelDMLRadio.Controls.Add(this.radioButtonInsert);
            this.flowLayoutPanelDMLRadio.Controls.Add(this.radioButtonUpdate);
            this.flowLayoutPanelDMLRadio.Controls.Add(this.radioButtonDelete);
            this.flowLayoutPanelDMLRadio.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelDMLRadio.Location = new System.Drawing.Point(15, 25);
            this.flowLayoutPanelDMLRadio.Name = "flowLayoutPanelDMLRadio";
            this.flowLayoutPanelDMLRadio.Size = new System.Drawing.Size(200, 80);
            this.flowLayoutPanelDMLRadio.TabIndex = 0;
            this.flowLayoutPanelDMLRadio.Margin = new System.Windows.Forms.Padding(5);

            // radioButtonInsert
            this.radioButtonInsert.AutoSize = true;
            this.radioButtonInsert.Location = new System.Drawing.Point(5, 5);
            this.radioButtonInsert.Name = "radioButtonInsert";
            this.radioButtonInsert.Size = new System.Drawing.Size(150, 17);
            this.radioButtonInsert.TabIndex = 0;
            this.radioButtonInsert.Text = "Добавить запись учёта";
            this.radioButtonInsert.UseVisualStyleBackColor = true;
            this.radioButtonInsert.CheckedChanged += new System.EventHandler(this.radioButtonInsert_CheckedChanged);

            // radioButtonUpdate
            this.radioButtonUpdate.AutoSize = true;
            this.radioButtonUpdate.Location = new System.Drawing.Point(5, 28);
            this.radioButtonUpdate.Name = "radioButtonUpdate";
            this.radioButtonUpdate.Size = new System.Drawing.Size(150, 17);
            this.radioButtonUpdate.TabIndex = 1;
            this.radioButtonUpdate.Text = "Изменить запись учёта";
            this.radioButtonUpdate.UseVisualStyleBackColor = true;
            this.radioButtonUpdate.CheckedChanged += new System.EventHandler(this.radioButtonUpdate_CheckedChanged);

            // radioButtonDelete
            this.radioButtonDelete.AutoSize = true;
            this.radioButtonDelete.Location = new System.Drawing.Point(5, 51);
            this.radioButtonDelete.Name = "radioButtonDelete";
            this.radioButtonDelete.Size = new System.Drawing.Size(130, 17);
            this.radioButtonDelete.TabIndex = 2;
            this.radioButtonDelete.Text = "Удалить запись учёта";
            this.radioButtonDelete.UseVisualStyleBackColor = true;
            this.radioButtonDelete.CheckedChanged += new System.EventHandler(this.radioButtonDelete_CheckedChanged);

            // panelInput
            this.panelInput.Controls.Add(this.tableLayoutPanelInput);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInput.Location = new System.Drawing.Point(8, 118);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(756, 102);
            this.panelInput.TabIndex = 1;
            this.panelInput.Margin = new System.Windows.Forms.Padding(5);

            // tableLayoutPanelInput
            this.tableLayoutPanelInput.ColumnCount = 6;
            this.tableLayoutPanelInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanelInput.Controls.Add(this.labelId, 0, 0);
            this.tableLayoutPanelInput.Controls.Add(this.textBoxId, 1, 0);
            this.tableLayoutPanelInput.Controls.Add(this.labelAccountingDate, 0, 1);
            this.tableLayoutPanelInput.Controls.Add(this.textBoxAccountingDate, 1, 1);
            this.tableLayoutPanelInput.Controls.Add(this.labelQuantity, 2, 0);
            this.tableLayoutPanelInput.Controls.Add(this.textBoxQuantity, 3, 0);
            this.tableLayoutPanelInput.Controls.Add(this.labelEmployeeIdInput, 2, 1);
            this.tableLayoutPanelInput.Controls.Add(this.textBoxEmployeeIdInput, 3, 1);
            this.tableLayoutPanelInput.Controls.Add(this.labelSupplyIdInput, 4, 0);
            this.tableLayoutPanelInput.Controls.Add(this.textBoxSupplyIdInput, 5, 0);
            this.tableLayoutPanelInput.Controls.Add(this.labelStorageId, 4, 1);
            this.tableLayoutPanelInput.Controls.Add(this.textBoxStorageId, 5, 1);
            this.tableLayoutPanelInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelInput.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelInput.Name = "tableLayoutPanelInput";
            this.tableLayoutPanelInput.RowCount = 2;
            this.tableLayoutPanelInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelInput.Size = new System.Drawing.Size(756, 102);
            this.tableLayoutPanelInput.TabIndex = 0;
            this.tableLayoutPanelInput.Margin = new System.Windows.Forms.Padding(5);

            // labelId
            this.labelId.AutoSize = true;
            this.labelId.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelId.Location = new System.Drawing.Point(8, 8);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(104, 13);
            this.labelId.TabIndex = 0;
            this.labelId.Text = "ID записи";
            this.labelId.Margin = new System.Windows.Forms.Padding(5);

            // textBoxId
            this.textBoxId.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxId.Location = new System.Drawing.Point(131, 8);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(120, 20);
            this.textBoxId.TabIndex = 1;
            this.textBoxId.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxId.TextChanged += new System.EventHandler(this.textBoxId_TextChanged);

            // labelAccountingDate
            this.labelAccountingDate.AutoSize = true;
            this.labelAccountingDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelAccountingDate.Location = new System.Drawing.Point(8, 59);
            this.labelAccountingDate.Name = "labelAccountingDate";
            this.labelAccountingDate.Size = new System.Drawing.Size(104, 13);
            this.labelAccountingDate.TabIndex = 2;
            this.labelAccountingDate.Text = "Дата учёта";
            this.labelAccountingDate.Margin = new System.Windows.Forms.Padding(5);

            // textBoxAccountingDate
            this.textBoxAccountingDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxAccountingDate.Location = new System.Drawing.Point(131, 59);
            this.textBoxAccountingDate.Name = "textBoxAccountingDate";
            this.textBoxAccountingDate.Size = new System.Drawing.Size(120, 20);
            this.textBoxAccountingDate.TabIndex = 3;
            this.textBoxAccountingDate.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxAccountingDate.TextChanged += new System.EventHandler(this.textBoxAccountingDate_TextChanged);

            // labelQuantity
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelQuantity.Location = new System.Drawing.Point(254, 8);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(104, 13);
            this.labelQuantity.TabIndex = 4;
            this.labelQuantity.Text = "Количество";
            this.labelQuantity.Margin = new System.Windows.Forms.Padding(5);

            // textBoxQuantity
            this.textBoxQuantity.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxQuantity.Location = new System.Drawing.Point(377, 8);
            this.textBoxQuantity.Name = "textBoxQuantity";
            this.textBoxQuantity.Size = new System.Drawing.Size(120, 20);
            this.textBoxQuantity.TabIndex = 5;
            this.textBoxQuantity.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxQuantity.TextChanged += new System.EventHandler(this.textBoxQuantity_TextChanged);

            // labelEmployeeIdInput
            this.labelEmployeeIdInput.AutoSize = true;
            this.labelEmployeeIdInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelEmployeeIdInput.Location = new System.Drawing.Point(254, 59);
            this.labelEmployeeIdInput.Name = "labelEmployeeIdInput";
            this.labelEmployeeIdInput.Size = new System.Drawing.Size(104, 13);
            this.labelEmployeeIdInput.TabIndex = 6;
            this.labelEmployeeIdInput.Text = "ID сотрудника";
            this.labelEmployeeIdInput.Margin = new System.Windows.Forms.Padding(5);

            // textBoxEmployeeIdInput
            this.textBoxEmployeeIdInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxEmployeeIdInput.Location = new System.Drawing.Point(377, 59);
            this.textBoxEmployeeIdInput.Name = "textBoxEmployeeIdInput";
            this.textBoxEmployeeIdInput.Size = new System.Drawing.Size(120, 20);
            this.textBoxEmployeeIdInput.TabIndex = 7;
            this.textBoxEmployeeIdInput.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxEmployeeIdInput.TextChanged += new System.EventHandler(this.textBoxEmployeeIdInput_TextChanged);

            // labelSupplyIdInput
            this.labelSupplyIdInput.AutoSize = true;
            this.labelSupplyIdInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSupplyIdInput.Location = new System.Drawing.Point(500, 8);
            this.labelSupplyIdInput.Name = "labelSupplyIdInput";
            this.labelSupplyIdInput.Size = new System.Drawing.Size(104, 13);
            this.labelSupplyIdInput.TabIndex = 8;
            this.labelSupplyIdInput.Text = "ID поставки";
            this.labelSupplyIdInput.Margin = new System.Windows.Forms.Padding(5);

            // textBoxSupplyIdInput
            this.textBoxSupplyIdInput.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxSupplyIdInput.Location = new System.Drawing.Point(623, 8);
            this.textBoxSupplyIdInput.Name = "textBoxSupplyIdInput";
            this.textBoxSupplyIdInput.Size = new System.Drawing.Size(120, 20);
            this.textBoxSupplyIdInput.TabIndex = 9;
            this.textBoxSupplyIdInput.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxSupplyIdInput.TextChanged += new System.EventHandler(this.textBoxSupplyIdInput_TextChanged);

            // labelStorageId
            this.labelStorageId.AutoSize = true;
            this.labelStorageId.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelStorageId.Location = new System.Drawing.Point(500, 59);
            this.labelStorageId.Name = "labelStorageId";
            this.labelStorageId.Size = new System.Drawing.Size(104, 13);
            this.labelStorageId.TabIndex = 10;
            this.labelStorageId.Text = "ID зоны хранения";
            this.labelStorageId.Margin = new System.Windows.Forms.Padding(5);

            // textBoxStorageId
            this.textBoxStorageId.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxStorageId.Location = new System.Drawing.Point(623, 59);
            this.textBoxStorageId.Name = "textBoxStorageId";
            this.textBoxStorageId.Size = new System.Drawing.Size(120, 20);
            this.textBoxStorageId.TabIndex = 11;
            this.textBoxStorageId.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxStorageId.TextChanged += new System.EventHandler(this.textBoxStorageId_TextChanged);

            // flowLayoutPanelDMLButtons
            this.flowLayoutPanelDMLButtons.Controls.Add(this.buttonExecuteDML);
            this.flowLayoutPanelDMLButtons.Controls.Add(this.buttonRefresh);
            this.flowLayoutPanelDMLButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelDMLButtons.Location = new System.Drawing.Point(8, 228);
            this.flowLayoutPanelDMLButtons.Name = "flowLayoutPanelDMLButtons";
            this.flowLayoutPanelDMLButtons.Size = new System.Drawing.Size(756, 42);
            this.flowLayoutPanelDMLButtons.TabIndex = 2;
            this.flowLayoutPanelDMLButtons.Margin = new System.Windows.Forms.Padding(5);

            // buttonExecuteDML
            this.buttonExecuteDML.AutoSize = true;
            this.buttonExecuteDML.Location = new System.Drawing.Point(606, 8);
            this.buttonExecuteDML.Name = "buttonExecuteDML";
            this.buttonExecuteDML.Size = new System.Drawing.Size(141, 23);
            this.buttonExecuteDML.TabIndex = 0;
            this.buttonExecuteDML.Text = "Выполнить запрос";
            this.buttonExecuteDML.UseVisualStyleBackColor = true;
            this.buttonExecuteDML.Click += new System.EventHandler(this.buttonExecuteDML_Click);
            this.buttonExecuteDML.Margin = new System.Windows.Forms.Padding(5);

            // buttonRefresh
            this.buttonRefresh.AutoSize = true;
            this.buttonRefresh.Location = new System.Drawing.Point(456, 8);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(141, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Обновить таблицу";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            this.buttonRefresh.Margin = new System.Windows.Forms.Padding(5);

            // dataGridViewDML
            this.dataGridViewDML.AllowUserToAddRows = false;
            this.dataGridViewDML.AllowUserToDeleteRows = false;
            this.dataGridViewDML.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewDML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDML.Location = new System.Drawing.Point(8, 278);
            this.dataGridViewDML.Name = "dataGridViewDML";
            this.dataGridViewDML.ReadOnly = true;
            this.dataGridViewDML.Size = new System.Drawing.Size(756, 268);
            this.dataGridViewDML.TabIndex = 3;
            this.dataGridViewDML.Margin = new System.Windows.Forms.Padding(5);

            // toolTip
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            this.toolTip.SetToolTip(this.textBoxEmployeeId, "Введите ID сотрудника (положительное целое число)");
            this.toolTip.SetToolTip(this.textBoxStartDate, "Введите начальную дату в формате ДД.ММ.ГГГГ (например, 20.05.2025)");
            this.toolTip.SetToolTip(this.textBoxSupplyId, "Введите ID поставки (положительное целое число)");
            this.toolTip.SetToolTip(this.textBoxId, "Введите ID записи (положительное целое число)");
            this.toolTip.SetToolTip(this.textBoxAccountingDate, "Введите дату учёта в формате ДД.ММ.ГГГГ (например, 20.05.2025)");
            this.toolTip.SetToolTip(this.textBoxQuantity, "Введите количество (положительное целое число)");
            this.toolTip.SetToolTip(this.textBoxEmployeeIdInput, "Введите ID сотрудника (положительное целое число)");
            this.toolTip.SetToolTip(this.textBoxSupplyIdInput, "Введите ID поставки (положительное целое число)");
            this.toolTip.SetToolTip(this.textBoxStorageId, "Введите ID зоны хранения (положительное целое число)");

            // SQLQueries
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControlSQL);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "SQLQueries";
            this.Text = "SQL Queries";
            this.tabControlSQL.ResumeLayout(false);
            this.tabPageSelect.ResumeLayout(false);
            this.tableLayoutPanelSelect.ResumeLayout(false);
            this.groupBoxSelect.ResumeLayout(false);
            this.groupBoxSelect.PerformLayout();
            this.flowLayoutPanelSelectRadio.ResumeLayout(false);
            this.flowLayoutPanelSelectRadio.PerformLayout();
            this.panelSelectInput.ResumeLayout(false);
            this.tableLayoutPanelSelectInput.ResumeLayout(false);
            this.tableLayoutPanelSelectInput.PerformLayout();
            this.flowLayoutPanelSelectButtons.ResumeLayout(false);
            this.flowLayoutPanelSelectButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelect)).EndInit();
            this.tabPageSubquery.ResumeLayout(false);
            this.tableLayoutPanelSubquery.ResumeLayout(false);
            this.groupBoxSubquery.ResumeLayout(false);
            this.groupBoxSubquery.PerformLayout();
            this.flowLayoutPanelSubqueryRadio.ResumeLayout(false);
            this.flowLayoutPanelSubqueryRadio.PerformLayout();
            this.panelSubqueryInput.ResumeLayout(false);
            this.tableLayoutPanelSubqueryInput.ResumeLayout(false);
            this.tableLayoutPanelSubqueryInput.PerformLayout();
            this.flowLayoutPanelSubqueryButtons.ResumeLayout(false);
            this.flowLayoutPanelSubqueryButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSubquery)).EndInit();
            this.tabPageDML.ResumeLayout(false);
            this.tableLayoutPanelDML.ResumeLayout(false);
            this.groupBoxDML.ResumeLayout(false);
            this.groupBoxDML.PerformLayout();
            this.flowLayoutPanelDMLRadio.ResumeLayout(false);
            this.flowLayoutPanelDMLRadio.PerformLayout();
            this.panelInput.ResumeLayout(false);
            this.tableLayoutPanelInput.ResumeLayout(false);
            this.tableLayoutPanelInput.PerformLayout();
            this.flowLayoutPanelDMLButtons.ResumeLayout(false);
            this.flowLayoutPanelDMLButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDML)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControlSQL;
        private System.Windows.Forms.TabPage tabPageSelect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSelect;
        private System.Windows.Forms.GroupBox groupBoxSelect;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSelectRadio;
        private System.Windows.Forms.RadioButton radioButtonSimpleSelect;
        private System.Windows.Forms.RadioButton radioButtonFilteredSelect;
        private System.Windows.Forms.RadioButton radioButtonMultiTableSelect;
        private System.Windows.Forms.RadioButton radioButtonAggregateSelect;
        private System.Windows.Forms.Panel panelSelectInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSelectInput;
        private System.Windows.Forms.Label labelEmployeeId;
        private System.Windows.Forms.TextBox textBoxEmployeeId;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.TextBox textBoxStartDate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSelectButtons;
        private System.Windows.Forms.Button buttonExecuteSelect;
        private System.Windows.Forms.DataGridView dataGridViewSelect;
        private System.Windows.Forms.TabPage tabPageSubquery;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSubquery;
        private System.Windows.Forms.GroupBox groupBoxSubquery;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSubqueryRadio;
        private System.Windows.Forms.RadioButton radioButtonCorrelatedSubquery;
        private System.Windows.Forms.RadioButton radioButtonNonCorrelatedSubquery;
        private System.Windows.Forms.Panel panelSubqueryInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSubqueryInput;
        private System.Windows.Forms.Label labelSupplyId;
        private System.Windows.Forms.TextBox textBoxSupplyId;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSubqueryButtons;
        private System.Windows.Forms.Button buttonExecuteSubquery;
        private System.Windows.Forms.DataGridView dataGridViewSubquery;
        private System.Windows.Forms.TabPage tabPageDML;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDML;
        private System.Windows.Forms.GroupBox groupBoxDML;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDMLRadio;
        private System.Windows.Forms.RadioButton radioButtonInsert;
        private System.Windows.Forms.RadioButton radioButtonUpdate;
        private System.Windows.Forms.RadioButton radioButtonDelete;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelInput;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.Label labelAccountingDate;
        private System.Windows.Forms.TextBox textBoxAccountingDate;
        private System.Windows.Forms.Label labelQuantity;
        private System.Windows.Forms.TextBox textBoxQuantity;
        private System.Windows.Forms.Label labelEmployeeIdInput;
        private System.Windows.Forms.TextBox textBoxEmployeeIdInput;
        private System.Windows.Forms.Label labelSupplyIdInput;
        private System.Windows.Forms.TextBox textBoxSupplyIdInput;
        private System.Windows.Forms.Label labelStorageId;
        private System.Windows.Forms.TextBox textBoxStorageId;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDMLButtons;
        private System.Windows.Forms.Button buttonExecuteDML;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.DataGridView dataGridViewDML;
        private System.Windows.Forms.ToolTip toolTip;
        #endregion
    }
}