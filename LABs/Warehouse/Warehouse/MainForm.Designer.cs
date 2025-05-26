namespace UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();

            // 
            // menuStripMain
            // 
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                CreateMenuItem("Справочники", "referencesToolStripMenuItem", null, new System.Windows.Forms.ToolStripItem[] {
                    CreateMenuItem("Продукты", "productsToolStripMenuItem", productsToolStripMenuItem_Click),
                    CreateMenuItem("Поставщики", "suppliersToolStripMenuItem", suppliersToolStripMenuItem_Click),
                    CreateMenuItem("Поставки", "suppliesToolStripMenuItem", suppliesToolStripMenuItem_Click),
                    CreateMenuItem("Сотрудники", "employeesToolStripMenuItem", employeesToolStripMenuItem_Click),
                    CreateMenuItem("Учет продукции", "productAccountingToolStripMenuItem", productAccountingToolStripMenuItem_Click),
                    CreateMenuItem("Зоны хранения", "storageZonesToolStripMenuItem", storageZonesToolStripMenuItem_Click),
                    CreateMenuItem("Склады", "warehousesToolStripMenuItem", warehousesToolStripMenuItem_Click),
                    CreateMenuItem("SQL Запросы", "sqlQueriesToolStripMenuItem", sqlQueriesToolStripMenuItem_Click)
                }),
                CreateMenuItem("О программе", "aboutToolStripMenuItem", aboutToolStripMenuItem_Click),
                CreateMenuItem("Выход", "exitToolStripMenuItem", exitToolStripMenuItem_Click)
            });

            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.Name = "contextMenuStripMain";
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                CreateMenuItem("Продукты", "contextProductsToolStripMenuItem", productsToolStripMenuItem_Click),
                CreateMenuItem("Поставщики", "contextSuppliersToolStripMenuItem", suppliersToolStripMenuItem_Click),
                CreateMenuItem("Поставки", "contextSuppliesToolStripMenuItem", suppliesToolStripMenuItem_Click),
                CreateMenuItem("Сотрудники", "contextEmployeesToolStripMenuItem", employeesToolStripMenuItem_Click),
                CreateMenuItem("Учет продукции", "contextProductAccountingToolStripMenuItem", productAccountingToolStripMenuItem_Click),
                CreateMenuItem("Зоны хранения", "contextStorageZonesToolStripMenuItem", storageZonesToolStripMenuItem_Click),
                CreateMenuItem("Склады", "contextWarehousesToolStripMenuItem", warehousesToolStripMenuItem_Click),
                CreateMenuItem("SQL Запросы", "contextSQLQueriesToolStripMenuItem", sqlQueriesToolStripMenuItem_Click),
                new System.Windows.Forms.ToolStripSeparator(),
                CreateMenuItem("Выход", "contextExitToolStripMenuItem", exitToolStripMenuItem_Click)
            });

            // 
            // toolStripMain
            // 
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                CreateToolStripButton("Продукты", "toolStripProductsButton", productsToolStripMenuItem_Click),
                CreateToolStripButton("Поставщики", "toolStripSuppliersButton", suppliersToolStripMenuItem_Click),
                CreateToolStripButton("Поставки", "toolStripSuppliesButton", suppliesToolStripMenuItem_Click),
                CreateToolStripButton("Сотрудники", "toolStripEmployeesButton", employeesToolStripMenuItem_Click),
                CreateToolStripButton("Учет продукции", "toolStripProductAccountingButton", productAccountingToolStripMenuItem_Click),
                CreateToolStripButton("Зоны хранения", "toolStripStorageZonesButton", storageZonesToolStripMenuItem_Click),
                CreateToolStripButton("Склады", "toolStripWarehousesButton", warehousesToolStripMenuItem_Click),
                CreateToolStripButton("SQL Запросы", "toolStripSQLQueriesButton", sqlQueriesToolStripMenuItem_Click),
                new System.Windows.Forms.ToolStripSeparator(),
                CreateToolStripButton("О программе", "toolStripAboutButton", aboutToolStripMenuItem_Click),
                CreateToolStripButton("Выход", "toolStripExitButton", exitToolStripMenuItem_Click)
            });

            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Text = "Система складского учета";
            this.Name = "MainForm";
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.toolStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.ContextMenuStrip = this.contextMenuStripMain;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolStripMenuItem CreateMenuItem(string text, string name, EventHandler clickHandler, System.Windows.Forms.ToolStripItem[] dropDownItems = null)
        {
            var menuItem = new System.Windows.Forms.ToolStripMenuItem
            {
                Text = text,
                Name = name
            };
            if (clickHandler != null)
                menuItem.Click += clickHandler;
            if (dropDownItems != null)
                menuItem.DropDownItems.AddRange(dropDownItems);
            return menuItem;
        }

        private System.Windows.Forms.ToolStripButton CreateToolStripButton(string text, string name, EventHandler clickHandler)
        {
            var button = new System.Windows.Forms.ToolStripButton
            {
                Text = text,
                Name = name
            };
            if (clickHandler != null)
                button.Click += clickHandler;
            return button;
        }

        #endregion
    }
}
