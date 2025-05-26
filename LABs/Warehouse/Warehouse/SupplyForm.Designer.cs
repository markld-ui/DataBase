namespace UI
{
    partial class SupplyForm
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
            dataGridViewSupplies = new DataGridView();
            cmbProduct = new ComboBox();
            cmbSupplier = new ComboBox();
            dtpSupplyDate = new DateTimePicker();
            txtQuantity = new TextBox();
            lblProduct = new Label();
            lblSupplier = new Label();
            lblSupplyDate = new Label();
            lblQuantity = new Label();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            flowLayoutPanelSearch = new FlowLayoutPanel();
            tableLayoutPanelInputs = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dataGridViewSupplies).BeginInit();
            tableLayoutPanelInputs.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewSupplies
            // 
            dataGridViewSupplies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSupplies.Dock = DockStyle.Fill;
            dataGridViewSupplies.Location = new Point(0, 54);
            dataGridViewSupplies.Margin = new Padding(4, 5, 4, 5);
            dataGridViewSupplies.Name = "dataGridViewSupplies";
            dataGridViewSupplies.RowHeadersWidth = 51;
            dataGridViewSupplies.Size = new Size(1045, 486);
            dataGridViewSupplies.TabIndex = 0;
            dataGridViewSupplies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSupplies.MultiSelect = false;
            dataGridViewSupplies.SelectionChanged += dataGridViewSupplies_SelectionChanged;
            // 
            // cmbProduct
            // 
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.FormattingEnabled = true;
            cmbProduct.Dock = DockStyle.Fill;
            cmbProduct.Location = new Point(140, 5);
            cmbProduct.Margin = new Padding(4, 5, 4, 5);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(310, 27);
            cmbProduct.TabIndex = 1;
            // 
            // cmbSupplier
            // 
            cmbSupplier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSupplier.FormattingEnabled = true;
            cmbSupplier.Dock = DockStyle.Fill;
            cmbSupplier.Location = new Point(140, 50);
            cmbSupplier.Margin = new Padding(4, 5, 4, 5);
            cmbSupplier.Name = "cmbSupplier";
            cmbSupplier.Size = new Size(310, 27);
            cmbSupplier.TabIndex = 2;
            // 
            // dtpSupplyDate
            // 
            dtpSupplyDate.Dock = DockStyle.Fill;
            dtpSupplyDate.Location = new Point(140, 95);
            dtpSupplyDate.Margin = new Padding(4, 5, 4, 5);
            dtpSupplyDate.Name = "dtpSupplyDate";
            dtpSupplyDate.Size = new Size(310, 27);
            dtpSupplyDate.TabIndex = 3;
            dtpSupplyDate.Format = DateTimePickerFormat.Short;
            // 
            // txtQuantity
            // 
            txtQuantity.Dock = DockStyle.Fill;
            txtQuantity.Location = new Point(140, 140);
            txtQuantity.Margin = new Padding(4, 5, 4, 5);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(310, 27);
            txtQuantity.TabIndex = 4;
            // 
            // lblProduct
            // 
            lblProduct.AutoSize = true;
            lblProduct.Dock = DockStyle.Left;
            lblProduct.Location = new Point(4, 0);
            lblProduct.Margin = new Padding(4, 0, 4, 0);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(80, 45);
            lblProduct.TabIndex = 5;
            lblProduct.Text = "Продукт:";
            // 
            // lblSupplier
            // 
            lblSupplier.AutoSize = true;
            lblSupplier.Dock = DockStyle.Left;
            lblSupplier.Location = new Point(4, 45);
            lblSupplier.Margin = new Padding(4, 0, 4, 0);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new Size(54, 45);
            lblSupplier.TabIndex = 6;
            lblSupplier.Text = "Поставщик:";
            // 
            // lblSupplyDate
            // 
            lblSupplyDate.AutoSize = true;
            lblSupplyDate.Dock = DockStyle.Left;
            lblSupplyDate.Location = new Point(4, 90);
            lblSupplyDate.Margin = new Padding(4, 0, 4, 0);
            lblSupplyDate.Name = "lblSupplyDate";
            lblSupplyDate.Size = new Size(54, 45);
            lblSupplyDate.TabIndex = 7;
            lblSupplyDate.Text = "Дата поставки:";
            // 
            // lblQuantity
            // 
            lblQuantity.AutoSize = true;
            lblQuantity.Dock = DockStyle.Left;
            lblQuantity.Location = new Point(4, 135);
            lblQuantity.Margin = new Padding(4, 0, 4, 0);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(54, 45);
            lblQuantity.TabIndex = 8;
            lblQuantity.Text = "Количество:";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(458, 5);
            btnAdd.Margin = new Padding(4, 5, 4, 5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 35);
            btnAdd.TabIndex = 9;
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
            btnUpdate.TabIndex = 10;
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
            btnDelete.TabIndex = 11;
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
            flowLayoutPanelSearch.TabIndex = 12;
            // 
            // tableLayoutPanelInputs
            // 
            tableLayoutPanelInputs.ColumnCount = 5;
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanelInputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanelInputs.Controls.Add(lblProduct, 0, 0);
            tableLayoutPanelInputs.Controls.Add(cmbProduct, 1, 0);
            tableLayoutPanelInputs.Controls.Add(btnAdd, 2, 0);
            tableLayoutPanelInputs.Controls.Add(lblSupplier, 0, 1);
            tableLayoutPanelInputs.Controls.Add(cmbSupplier, 1, 1);
            tableLayoutPanelInputs.Controls.Add(btnUpdate, 2, 1);
            tableLayoutPanelInputs.Controls.Add(lblSupplyDate, 0, 2);
            tableLayoutPanelInputs.Controls.Add(dtpSupplyDate, 1, 2);
            tableLayoutPanelInputs.Controls.Add(btnDelete, 2, 2);
            tableLayoutPanelInputs.Controls.Add(lblQuantity, 0, 3);
            tableLayoutPanelInputs.Controls.Add(txtQuantity, 1, 3);
            tableLayoutPanelInputs.Dock = DockStyle.Bottom;
            tableLayoutPanelInputs.Location = new Point(0, 492);
            tableLayoutPanelInputs.Margin = new Padding(4, 5, 4, 5);
            tableLayoutPanelInputs.Name = "tableLayoutPanelInputs";
            tableLayoutPanelInputs.RowCount = 4;
            tableLayoutPanelInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanelInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanelInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanelInputs.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanelInputs.Size = new Size(1045, 186);
            tableLayoutPanelInputs.TabIndex = 13;
            // 
            // SupplyForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1045, 678);
            Controls.Add(dataGridViewSupplies);
            Controls.Add(tableLayoutPanelInputs);
            Controls.Add(flowLayoutPanelSearch);
            Margin = new Padding(4, 5, 4, 5);
            Name = "SupplyForm";
            Text = "Поставки";
            Load += SupplyForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewSupplies).EndInit();
            tableLayoutPanelInputs.ResumeLayout(false);
            tableLayoutPanelInputs.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridViewSupplies;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.DateTimePicker dtpSupplyDate;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Label lblSupplyDate;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSearch;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelInputs;
        private System.Windows.Forms.CheckBox checkBoxFind;

        #endregion
    }
}