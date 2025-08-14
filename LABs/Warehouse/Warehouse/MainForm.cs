using System;
using System.Windows.Forms;
using Application;
using Infrastructure;

namespace UI
{
    /// <summary>
    /// Главная форма приложения, обеспечивающая доступ к основным функциям системы.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Форма предоставляет меню для открытия дочерних форм, таких как управление продуктами,
    /// поставщиками, поставками, сотрудниками, учётом продукции, зонами хранения, складами,
    /// SQL-запросами и информацией о программе. Также управляет настройками окна и проверкой
    /// подключения к базе данных.
    /// </para>
    /// <para>
    /// Настройки формы (положение, размер, состояние окна) сохраняются при закрытии и
    /// загружаются при запуске.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Запуск формы из точки входа программы
    /// Application.Run(new MainForm());
    /// </code>
    /// </example>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainForm"/>.
        /// </summary>
        /// <remarks>
        /// Вызывает метод <see cref="InitializeComponent"/> для инициализации компонентов формы,
        /// загружает настройки окна через <see cref="LoadSettings"/> и проверяет подключение
        /// к базе данных через <see cref="TestDatabaseConnection"/>.
        /// </remarks>
        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
            TestDatabaseConnection();
        }

        /// <summary>
        /// Проверяет подключение к базе данных и отображает результат пользователю.
        /// </summary>
        /// <remarks>
        /// Использует <see cref="DatabaseConnection.TestConnection"/> для проверки подключения.
        /// При успешном подключении отображает информационное сообщение, при неудаче — сообщение об ошибке.
        /// </remarks>
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

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "Продукты".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="ProductForm"/> в модальном режиме.
        /// </remarks>
        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ProductForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "Поставщики".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="SupplierForm"/> в модальном режиме.
        /// </remarks>
        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SupplierForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "Поставки".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="SupplyForm"/> в модальном режиме.
        /// </remarks>
        private void suppliesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SupplyForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "Сотрудники".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="EmployeeForm"/> в модальном режиме.
        /// </remarks>
        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new EmployeeForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "Учёт продукции".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="ProductAccountingForm"/> в модальном режиме.
        /// </remarks>
        private void productAccountingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ProductAccountingForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "Зоны хранения".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="StorageZoneForm"/> в модальном режиме.
        /// </remarks>
        private void storageZonesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new StorageZoneForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "Склады".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="WarehouseForm"/> в модальном режиме.
        /// </remarks>
        private void warehousesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WarehouseForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "SQL-запросы".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="SQLQueries"/> в модальном режиме.
        /// </remarks>
        private void sqlQueriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SQLQueries();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "О программе".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Открывает форму <see cref="AboutForm"/> в модальном режиме.
        /// </remarks>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Обрабатывает событие нажатия на пункт меню "Выход".
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Закрывает форму, вызывая событие <see cref="MainForm_FormClosing"/>.
        /// </remarks>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обрабатывает событие закрытия формы.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события, содержащие информацию о закрытии формы.</param>
        /// <remarks>
        /// Сохраняет настройки окна (максимальное состояние, положение, размер) в <see cref="Settings.Default"/>.
        /// Запрашивает подтверждение закрытия программы, отменяя событие, если пользователь выбирает "Нет".
        /// </remarks>
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

        /// <summary>
        /// Обрабатывает событие завершения закрытия формы.
        /// </summary>
        /// <param name="sender">Объект, инициировавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        /// <remarks>
        /// Вызывает метод <see cref="SaveSettings"/> для сохранения настроек окна.
        /// </remarks>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Загружает настройки формы из конфигурации.
        /// </summary>
        /// <remarks>
        /// Устанавливает положение, размер и состояние окна на основе значений из <see cref="Settings.Default"/>.
        /// Если состояние окна не удаётся распознать, устанавливается <see cref="FormWindowState.Normal"/>.
        /// </remarks>
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

        /// <summary>
        /// Сохраняет настройки формы в конфигурацию.
        /// </summary>
        /// <remarks>
        /// Сохраняет положение и размер окна, если оно не в нормальном состоянии, и состояние окна
        /// в <see cref="Settings.Default"/>. Вызывает <see cref="Settings.Save"/> для записи изменений.
        /// </remarks>
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