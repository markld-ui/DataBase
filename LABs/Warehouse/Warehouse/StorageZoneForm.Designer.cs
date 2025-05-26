namespace UI
{
    partial class StorageZoneForm
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
            this.dataGridViewStorageZones = new System.Windows.Forms.DataGridView();
            this.cmbWarehouse = new System.Windows.Forms.ComboBox();
            this.txtCapacity = new System.Windows.Forms.TextBox();
            this.cmbZoneType = new System.Windows.Forms.ComboBox();
            this.txtZoneName = new System.Windows.Forms.TextBox();
            this.lblWarehouse = new System.Windows.Forms.Label();
            this.lblCapacity = new System.Windows.Forms.Label();
            this.lblZoneType = new System.Windows.Forms.Label();
            this.lblZoneName = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStorageZones)).BeginInit();
            this.SuspendLayout();

            // dataGridViewStorageZones
            this.dataGridViewStorageZones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStorageZones.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewStorageZones.Name = "dataGridViewStorageZones";
            this.dataGridViewStorageZones.Size = new System.Drawing.Size(760, 262);
            this.dataGridViewStorageZones.TabIndex = 0;
            this.dataGridViewStorageZones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStorageZones.MultiSelect = false;
            this.dataGridViewStorageZones.SelectionChanged += new System.EventHandler(this.dataGridViewStorageZones_SelectionChanged);

            // lblWarehouse
            this.lblWarehouse.AutoSize = true;
            this.lblWarehouse.Location = new System.Drawing.Point(12, 320);
            this.lblWarehouse.Name = "lblWarehouse";
            this.lblWarehouse.Size = new System.Drawing.Size(85, 13);
            this.lblWarehouse.TabIndex = 1;
            this.lblWarehouse.Text = "Склад:";

            // cmbWarehouse
            this.cmbWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWarehouse.FormattingEnabled = true;
            this.cmbWarehouse.Location = new System.Drawing.Point(120, 320);
            this.cmbWarehouse.Name = "cmbWarehouse";
            this.cmbWarehouse.Size = new System.Drawing.Size(200, 21);
            this.cmbWarehouse.TabIndex = 2;

            // lblCapacity
            this.lblCapacity.AutoSize = true;
            this.lblCapacity.Location = new System.Drawing.Point(12, 350);
            this.lblCapacity.Name = "lblCapacity";
            this.lblCapacity.Size = new System.Drawing.Size(85, 13);
            this.lblCapacity.TabIndex = 3;
            this.lblCapacity.Text = "Вместимость:";

            // txtCapacity
            this.txtCapacity.Location = new System.Drawing.Point(120, 350);
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.Size = new System.Drawing.Size(200, 20);
            this.txtCapacity.TabIndex = 4;

            // lblZoneType
            this.lblZoneType.AutoSize = true;
            this.lblZoneType.Location = new System.Drawing.Point(12, 380);
            this.lblZoneType.Name = "lblZoneType";
            this.lblZoneType.Size = new System.Drawing.Size(85, 13);
            this.lblZoneType.TabIndex = 5;
            this.lblZoneType.Text = "Тип зоны:";

            // cmbZoneType
            this.cmbZoneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbZoneType.FormattingEnabled = true;
            this.cmbZoneType.Location = new System.Drawing.Point(120, 380);
            this.cmbZoneType.Name = "cmbZoneType";
            this.cmbZoneType.Size = new System.Drawing.Size(200, 21);
            this.cmbZoneType.TabIndex = 6;

            // lblZoneName
            this.lblZoneName.AutoSize = true;
            this.lblZoneName.Location = new System.Drawing.Point(12, 410);
            this.lblZoneName.Name = "lblZoneName";
            this.lblZoneName.Size = new System.Drawing.Size(85, 13);
            this.lblZoneName.TabIndex = 7;
            this.lblZoneName.Text = "Название зоны:";

            // txtZoneName
            this.txtZoneName.Location = new System.Drawing.Point(120, 410);
            this.txtZoneName.Name = "txtZoneName";
            this.txtZoneName.Size = new System.Drawing.Size(200, 20);
            this.txtZoneName.TabIndex = 8;

            // btnAdd
            this.btnAdd.Location = new System.Drawing.Point(350, 320);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 30);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // btnUpdate
            this.btnUpdate.Location = new System.Drawing.Point(350, 360);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 30);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "Обновить";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(350, 400);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // StorageZoneForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtZoneName);
            this.Controls.Add(this.lblZoneName);
            this.Controls.Add(this.cmbZoneType);
            this.Controls.Add(this.lblZoneType);
            this.Controls.Add(this.txtCapacity);
            this.Controls.Add(this.lblCapacity);
            this.Controls.Add(this.cmbWarehouse);
            this.Controls.Add(this.lblWarehouse);
            this.Controls.Add(this.dataGridViewStorageZones);
            this.Name = "StorageZoneForm";
            this.Text = "Зоны хранения";
            this.Load += new System.EventHandler(this.StorageZoneForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStorageZones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DataGridView dataGridViewStorageZones;
        private System.Windows.Forms.ComboBox cmbWarehouse;
        private System.Windows.Forms.TextBox txtCapacity;
        private System.Windows.Forms.ComboBox cmbZoneType;
        private System.Windows.Forms.TextBox txtZoneName;
        private System.Windows.Forms.Label lblWarehouse;
        private System.Windows.Forms.Label lblCapacity;
        private System.Windows.Forms.Label lblZoneType;
        private System.Windows.Forms.Label lblZoneName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox checkBoxFind;

        #endregion
    }
}