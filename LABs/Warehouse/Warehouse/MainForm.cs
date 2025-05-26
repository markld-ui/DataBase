using System;
using System.Windows.Forms;
using Application;
using Infrastructure;

namespace UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            var dbConnection = Application.AppContext.Instance.DatabaseConnection;
            if (dbConnection.TestConnection())
            {
                MessageBox.Show("Подключение к базе данных успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к базе данных. Проверьте настройки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ProductForm();
            form.ShowDialog();
        }

        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SupplierForm();
            form.ShowDialog();
        }

        private void suppliesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SupplyForm();
            form.ShowDialog();
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new EmployeeForm();
            form.ShowDialog();
        }

        private void productAccountingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ProductAccountingForm();
            form.ShowDialog();
        }

        private void storageZonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new StorageZoneForm();
            form.ShowDialog();
        }

        private void warehousesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WarehouseForm();
            form.ShowDialog();
        }

        private void sqlQueriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SQLQueries();
            form.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                Settings.Default.MainFormMaximized = true;
            }
            else
            {
                Settings.Default.MainFormMaximized = false;
                Settings.Default.MainFormLocation = this.Location;
                Settings.Default.MainFormSize = this.Size;
            }
            Settings.Default.Save();

            e.Cancel = MessageBox.Show("Вы хотите закрыть программу?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
        }

        private void LoadSettings()
        {
            this.Location = Settings.Default.MainFormLocation;
            this.Size = Settings.Default.MainFormSize;

            if (Enum.TryParse<FormWindowState>(Settings.Default.MainFormState, out var state))
            {
                this.WindowState = state;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void SaveSettings()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.MainFormLocation = this.Location;
                Settings.Default.MainFormSize = this.Size;
            }

            Settings.Default.MainFormState = this.WindowState.ToString();
            Settings.Default.Save();
        }

        private MenuStrip menuStripMain;
        private ContextMenuStrip contextMenuStripMain;
        private ToolStrip toolStripMain;
    }
}
