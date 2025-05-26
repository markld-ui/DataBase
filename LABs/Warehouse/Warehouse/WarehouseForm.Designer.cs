namespace UI
{
    partial class WarehouseForm
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
            dataGridViewWarehouses = new DataGridView();
            txtName = new TextBox();
            txtAddress = new TextBox();
            lblName = new Label();
            lblAddress = new Label();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            flowLayoutPanelSearch = new FlowLayoutPanel();
            tableLayoutPanelInputs = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dataGridViewWarehouses).BeginInit();
            tableLayoutPanelInputs.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewWarehouses
            // 
            dataGridViewWarehouses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewWarehouses.Dock = DockStyle.Fill;
            dataGridViewWarehouses.Location = new Point(0, 54);
            dataGridViewWarehouses.Margin = new Padding(4, 5, 4, 5);
            dataGridViewWarehouses.Name = "dataGridViewWarehouses";
            dataGridViewWarehouses.RowHeadersWidth = 51;
            dataGridViewWarehouses.Size = new Size(1045, 486);
            dataGridViewWarehouses.TabIndex = 0;
            dataGridViewWarehouses.SelectionChanged += dataGridViewWarehouses_SelectionChanged;
            // 
            // txtName
            // 
            txtName.Dock = DockStyle.Fill;
            txtName.Location = new Point(140, 5);
            txtName.Margin = new Padding(4, 5, 4, 5);
            txtName.Name = "txtName";
            txtName.Size = new Size(310, 27);
            txtName.TabIndex = 1;
            // 
            // txtAddress
            // 
            txtAddress.Dock = DockStyle.Fill;
            txtAddress.Location = new Point(140, 50);
            txtAddress.Margin = new Padding(4, 5, 4, 5);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(310, 27);
            txtAddress.TabIndex = 2;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Dock = DockStyle.Left;
            lblName.Location = new Point(4, 0);
            lblName.Margin = new Padding(4, 0, 4, 0);
            lblName.Name = "lblName";
            lblName.Size = new Size(80, 45);
            lblName.TabIndex = 3;
            lblName.Text = "Название:";
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Dock = DockStyle.Left;
            lblAddress.Location = new Point(4, 45);
            lblAddress.Margin = new Padding(4, 0, 4, 0);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(54, 45);
            lblAddress.TabIndex = 4;
            lblAddress.Text = "Адрес:";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(458, 5);
            btnAdd.Margin = new Padding(4, 5, 4, 5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 35);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(458, 50);
            btnUpdate.Margin = new Padding(4, 5, 4, 5);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(100, 35);
            btnUpdate.TabIndex = 6;
            btnUpdate.Text = "Обновить";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(458, 95);
            btnDelete.Margin = new Padding(4, 5, 4, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 35);
            btnDelete.TabIndex = 7;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // flowLayoutPanelSearch
            // 
            flowLayoutPanelSearch.Dock = DockStyle.Top;
            flowLayoutPanelSearch.Location = new Point(0, 0);
            flowLayoutPanelSearch.Margin = new Padding(4, 5, 4, 5);
            flowLayoutPanelSearch.Name = "flowLayoutPanelSearch";
            flowLayoutPanelSearch.Size = new Size(1045, 54);
            flowLayoutPanelSearch.TabIndex = 11;
            // 
            // tableLayoutPanelInputs
            // 
            tableLayoutPanelInputs.ColumnCount = 5;
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanelInputs.Controls.Add(lblName, 0, 0);
            tableLayoutPanelInputs.Controls.Add(txtName, 1, 0);
            tableLayoutPanelInputs.Controls.Add(btnAdd, 2, 0);
            tableLayoutPanelInputs.Controls.Add(lblAddress, 0, 1);
            tableLayoutPanelInputs.Controls.Add(txtAddress, 1, 1);
            tableLayoutPanelInputs.Controls.Add(btnUpdate, 2, 1);
            tableLayoutPanelInputs.Controls.Add(btnDelete, 2, 2);
            tableLayoutPanelInputs.Dock = DockStyle.Bottom;
            tableLayoutPanelInputs.Location = new Point(0, 540);
            tableLayoutPanelInputs.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanelInputs.Name = "tableLayoutPanelInputs";
            tableLayoutPanelInputs.RowCount = 3;
            tableLayoutPanelInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableLayoutPanelInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableLayoutPanelInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tableLayoutPanelInputs.Size = new Size(1045, 138);
            tableLayoutPanelInputs.TabIndex = 12;
            // 
            // WarehouseForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1045, 678);
            Controls.Add(dataGridViewWarehouses);
            Controls.Add(tableLayoutPanelInputs);
            Controls.Add(flowLayoutPanelSearch);
            Margin = new Padding(4, 5, 4, 5);
            Name = "WarehouseForm";
            Text = "Склады";
            Load += WarehouseForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewWarehouses).EndInit();
            tableLayoutPanelInputs.ResumeLayout(false);
            tableLayoutPanelInputs.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridViewWarehouses;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelInputs;
        private System.Windows.Forms.CheckBox checkBoxFind;
        #endregion
    }
}