namespace UI
{
    partial class ProductAccountingForm
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
            this.dataGridViewProductAccountings = new System.Windows.Forms.DataGridView();
            this.cmbSupply = new System.Windows.Forms.ComboBox();
            this.cmbEmployee = new System.Windows.Forms.ComboBox();
            this.cmbStorageZone = new System.Windows.Forms.ComboBox();
            this.dtpAccountingDate = new System.Windows.Forms.DateTimePicker();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.dtpLastMovementDate = new System.Windows.Forms.DateTimePicker();
            this.lblSupply = new System.Windows.Forms.Label();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.lblStorageZone = new System.Windows.Forms.Label();
            this.lblAccountingDate = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblLastMovementDate = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.chkLastMovementDate = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductAccountings)).BeginInit();
            this.SuspendLayout();

            // dataGridViewProductAccountings
            this.dataGridViewProductAccountings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProductAccountings.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewProductAccountings.Name = "dataGridViewProductAccountings";
            this.dataGridViewProductAccountings.Size = new System.Drawing.Size(760, 262);
            this.dataGridViewProductAccountings.TabIndex = 0;
            this.dataGridViewProductAccountings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProductAccountings.MultiSelect = false;
            this.dataGridViewProductAccountings.SelectionChanged += new System.EventHandler(this.dataGridViewProductAccountings_SelectionChanged);

            // lblSupply
            this.lblSupply.AutoSize = true;
            this.lblSupply.Location = new System.Drawing.Point(12, 320);
            this.lblSupply.Name = "lblSupply";
            this.lblSupply.Size = new System.Drawing.Size(85, 13);
            this.lblSupply.TabIndex = 1;
            this.lblSupply.Text = "Поставка:";

            // cmbSupply
            this.cmbSupply.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupply.FormattingEnabled = true;
            this.cmbSupply.Location = new System.Drawing.Point(120, 320);
            this.cmbSupply.Name = "cmbSupply";
            this.cmbSupply.Size = new System.Drawing.Size(200, 21);
            this.cmbSupply.TabIndex = 2;

            // lblEmployee
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Location = new System.Drawing.Point(12, 350);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(85, 13);
            this.lblEmployee.TabIndex = 3;
            this.lblEmployee.Text = "Сотрудник:";

            // cmbEmployee
            this.cmbEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmployee.FormattingEnabled = true;
            this.cmbEmployee.Location = new System.Drawing.Point(120, 350);
            this.cmbEmployee.Name = "cmbEmployee";
            this.cmbEmployee.Size = new System.Drawing.Size(200, 21);
            this.cmbEmployee.TabIndex = 4;

            // lblStorageZone
            this.lblStorageZone.AutoSize = true;
            this.lblStorageZone.Location = new System.Drawing.Point(12, 380);
            this.lblStorageZone.Name = "lblStorageZone";
            this.lblStorageZone.Size = new System.Drawing.Size(85, 13);
            this.lblStorageZone.TabIndex = 5;
            this.lblStorageZone.Text = "Зона хранения:";

            // cmbStorageZone
            this.cmbStorageZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorageZone.FormattingEnabled = true;
            this.cmbStorageZone.Location = new System.Drawing.Point(120, 380);
            this.cmbStorageZone.Name = "cmbStorageZone";
            this.cmbStorageZone.Size = new System.Drawing.Size(200, 21);
            this.cmbStorageZone.TabIndex = 6;

            // lblAccountingDate
            this.lblAccountingDate.AutoSize = true;
            this.lblAccountingDate.Location = new System.Drawing.Point(12, 410);
            this.lblAccountingDate.Name = "lblAccountingDate";
            this.lblAccountingDate.Size = new System.Drawing.Size(85, 13);
            this.lblAccountingDate.TabIndex = 7;
            this.lblAccountingDate.Text = "Дата учета:";

            // dtpAccountingDate
            this.dtpAccountingDate.Location = new System.Drawing.Point(120, 410);
            this.dtpAccountingDate.Name = "dtpAccountingDate";
            this.dtpAccountingDate.Size = new System.Drawing.Size(200, 20);
            this.dtpAccountingDate.TabIndex = 8;
            this.dtpAccountingDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(12, 440);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(85, 13);
            this.lblQuantity.TabIndex = 9;
            this.lblQuantity.Text = "Количество:";

            // txtQuantity
            this.txtQuantity.Location = new System.Drawing.Point(120, 440);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(200, 20);
            this.txtQuantity.TabIndex = 10;

            // lblLastMovementDate
            this.lblLastMovementDate.AutoSize = true;
            this.lblLastMovementDate.Location = new System.Drawing.Point(12, 470);
            this.lblLastMovementDate.Name = "lblLastMovementDate";
            this.lblLastMovementDate.Size = new System.Drawing.Size(85, 13);
            this.lblLastMovementDate.TabIndex = 11;
            this.lblLastMovementDate.Text = "Дата движения:";

            // dtpLastMovementDate
            this.dtpLastMovementDate.Location = new System.Drawing.Point(120, 470);
            this.dtpLastMovementDate.Name = "dtpLastMovementDate";
            this.dtpLastMovementDate.Size = new System.Drawing.Size(200, 20);
            this.dtpLastMovementDate.TabIndex = 12;
            this.dtpLastMovementDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;

            // chkLastMovementDate
            this.chkLastMovementDate.AutoSize = true;
            this.chkLastMovementDate.Location = new System.Drawing.Point(330, 470);
            this.chkLastMovementDate.Name = "chkLastMovementDate";
            this.chkLastMovementDate.Size = new System.Drawing.Size(80, 17);
            this.chkLastMovementDate.TabIndex = 13;
            this.chkLastMovementDate.Text = "Указано";
            this.chkLastMovementDate.Checked = false;
            this.chkLastMovementDate.CheckedChanged += new System.EventHandler(this.chkLastMovementDate_CheckedChanged);

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(350, 320);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // btnUpdate
            this.btnUpdate.Location = new System.Drawing.Point(350, 360);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 30);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Обновить";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(350, 400);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // ProductAccountingForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.chkLastMovementDate);
            this.Controls.Add(this.dtpLastMovementDate);
            this.Controls.Add(this.lblLastMovementDate);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.dtpAccountingDate);
            this.Controls.Add(this.lblAccountingDate);
            this.Controls.Add(this.cmbStorageZone);
            this.Controls.Add(this.lblStorageZone);
            this.Controls.Add(this.cmbEmployee);
            this.Controls.Add(this.lblEmployee);
            this.Controls.Add(this.cmbSupply);
            this.Controls.Add(this.lblSupply);
            this.Controls.Add(this.dataGridViewProductAccountings);
            this.Name = "ProductAccountingForm";
            this.Text = "Учет продукции";
            this.Load += new System.EventHandler(this.ProductAccountingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProductAccountings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dataGridViewProductAccountings;
        private System.Windows.Forms.ComboBox cmbSupply;
        private System.Windows.Forms.ComboBox cmbEmployee;
        private System.Windows.Forms.ComboBox cmbStorageZone;
        private System.Windows.Forms.DateTimePicker dtpAccountingDate;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.DateTimePicker dtpLastMovementDate;
        private System.Windows.Forms.Label lblSupply;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.Label lblStorageZone;
        private System.Windows.Forms.Label lblAccountingDate;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.Label lblLastMovementDate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkLastMovementDate;
        private System.Windows.Forms.CheckBox checkBoxFind;

        #endregion
    }
}